using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClosedXML.Excel;

namespace QlyCafe.Quanly
{
    public partial class KhuyenMai : Form
    {
        // Biến cờ để xác định đang Thêm mới hay Sửa
        private bool isAddingNew = false;
        private DataTable dtKhuyenMai;
        public KhuyenMai()
        {
            InitializeComponent();
        }

        private void KhuyenMai_Load(object sender, EventArgs e)
        {
            // Các control nhập liệu chính (trừ txtMaKM) và các control đặc thù sẽ luôn Enabled.
            // Trạng thái ReadOnly của txtMaKM sẽ được quản lý bởi btnThem/btnSua.
            // Trạng thái Visible của các control đặc thù sẽ do UpdateRelatedControlsVisibility quản lý.
            MakeControlsAlwaysEditable();

            LoadDataGridView();
            LoadFilterComboBoxes();
            LoadDetailLoaiKMComboBox();
            LoadAllSanPhamToChuaApDung();

            ResetValues(); // Đặt lại giá trị và gọi UpdateRelatedControlsVisibility
            SetButtonStates("initial"); // Đặt trạng thái nút ban đầu
        }

        private void MakeControlsAlwaysEditable()
        {
            // Tab Thông tin Khuyến Mãi
            txtTenKM.ReadOnly = false;
            cboLoaiKM.Enabled = true;
            txtGiaTri.ReadOnly = false; // Sẽ được quản lý Visible/Enabled cụ thể hơn trong Update...
            dtpNgayBD.Enabled = true;
            dtpNgayKT.Enabled = true;
            txtMoTa.ReadOnly = false;
            txtDKApDung.ReadOnly = false;
            chkHoatDong.Enabled = true;
            txtDKSoLuongCanMua.ReadOnly = false; // Luôn cho phép nhập liệu nếu visible
            txtDKSoLuongDuocTang.ReadOnly = false; // Luôn cho phép nhập liệu nếu visible

            // Tab Sản phẩm áp dụng
            btnThemChonApDung.Enabled = true;
            btnThemTatCaApDung.Enabled = true;
            btnBoChon.Enabled = true;
            btnBoTatCaApDung.Enabled = true;
            lstSPChuaApDung.Enabled = true;
            lstSPDaApDung.Enabled = true;
        }

        // Phương thức mới để cài đặt hiển thị cột (dựa trên code cũ của bạn)
        private void ApplyDataGridViewStyles()
        {
            if (dgvKhuyenMai.DataSource == null || dgvKhuyenMai.Columns.Count == 0) return;

            // Kiểm tra sự tồn tại của cột trước khi truy cập
            if (dgvKhuyenMai.Columns.Contains("MaKM"))
            {
                dgvKhuyenMai.Columns["MaKM"].HeaderText = "Mã KM";
                dgvKhuyenMai.Columns["MaKM"].Width = 70;
            }
            if (dgvKhuyenMai.Columns.Contains("TenKM"))
            {
                dgvKhuyenMai.Columns["TenKM"].HeaderText = "Tên Khuyến Mãi";
                dgvKhuyenMai.Columns["TenKM"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            if (dgvKhuyenMai.Columns.Contains("LoaiKM"))
            {
                dgvKhuyenMai.Columns["LoaiKM"].HeaderText = "Loại KM";
                dgvKhuyenMai.Columns["LoaiKM"].Width = 120;
            }
            if (dgvKhuyenMai.Columns.Contains("GiaTri"))
            {
                dgvKhuyenMai.Columns["GiaTri"].HeaderText = "Giá Trị";
                dgvKhuyenMai.Columns["GiaTri"].DefaultCellStyle.Format = "N0"; // Định dạng số
                dgvKhuyenMai.Columns["GiaTri"].Width = 80;
            }
            if (dgvKhuyenMai.Columns.Contains("NgayBatDau"))
            {
                dgvKhuyenMai.Columns["NgayBatDau"].HeaderText = "Ngày BĐ";
                dgvKhuyenMai.Columns["NgayBatDau"].Width = 90;
            }
            if (dgvKhuyenMai.Columns.Contains("NgayKetThuc"))
            {
                dgvKhuyenMai.Columns["NgayKetThuc"].HeaderText = "Ngày KT";
                dgvKhuyenMai.Columns["NgayKetThuc"].Width = 90;
            }
            if (dgvKhuyenMai.Columns.Contains("DieuKienApDung"))
            {
                dgvKhuyenMai.Columns["DieuKienApDung"].HeaderText = "Điều Kiện (Text)";
                // Có thể ẩn cột này nếu quá dài hoặc không cần thiết trên lưới chính
                // dgvKhuyenMai.Columns["DieuKienApDung"].Visible = false;
            }
            if (dgvKhuyenMai.Columns.Contains("TrangThai"))
            {
                dgvKhuyenMai.Columns["TrangThai"].HeaderText = "Hoạt Động";
                dgvKhuyenMai.Columns["TrangThai"].Width = 80;
            }
            if (dgvKhuyenMai.Columns.Contains("DK_SoLuongCanMua"))
            {
                dgvKhuyenMai.Columns["DK_SoLuongCanMua"].HeaderText = "SL Mua (ĐK)";
                dgvKhuyenMai.Columns["DK_SoLuongCanMua"].Width = 80;
            }
            if (dgvKhuyenMai.Columns.Contains("DK_SoLuongDuocTang"))
            {
                dgvKhuyenMai.Columns["DK_SoLuongDuocTang"].HeaderText = "SL Tặng (ĐK)";
                dgvKhuyenMai.Columns["DK_SoLuongDuocTang"].Width = 80;
            }

            dgvKhuyenMai.AllowUserToAddRows = false;
            dgvKhuyenMai.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgvKhuyenMai.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        // Cập nhật LoadDataGridView để gọi ApplyDataGridViewStyles
        private void LoadDataGridView()
        {
            string sql = "SELECT MaKM, TenKM, LoaiKM, GiaTri, NgayBatDau, NgayKetThuc, DieuKienApDung, TrangThai, DK_SoLuongCanMua, DK_SoLuongDuocTang FROM KhuyenMai";
            try
            {
                dtKhuyenMai = Function.GetDataToTable(sql); // dtKhuyenMai là biến DataTable ở mức lớp
                dgvKhuyenMai.DataSource = dtKhuyenMai;
                ApplyDataGridViewStyles(); // Gọi phương thức cài đặt cột
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách khuyến mãi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadFilterComboBoxes()
        {
            try
            {
                string sqlLoaiKM = "SELECT DISTINCT LoaiKM FROM KhuyenMai WHERE LoaiKM IS NOT NULL AND LoaiKM <> '' ORDER BY LoaiKM";
                DataTable dtLoaiKMFilter = Function.GetDataToTable(sqlLoaiKM);
                DataRow drFilter = dtLoaiKMFilter.NewRow();
                drFilter["LoaiKM"] = "Tất cả loại KM";
                dtLoaiKMFilter.Rows.InsertAt(drFilter, 0);
                // Giả sử bạn đã có overload Function.FillCombo(ComboBox cbo, string displayMember, string valueMember, DataTable dataSource)
                Function.FillCombo(cboLocLoaiKM, "LoaiKM", "LoaiKM", dtLoaiKMFilter);
                cboLocLoaiKM.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải bộ lọc loại khuyến mãi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            cboLocTrangThai.Items.Clear();
            cboLocTrangThai.Items.Add("Tất cả trạng thái");
            cboLocTrangThai.Items.Add("Đang hoạt động");
            cboLocTrangThai.Items.Add("Không hoạt động");
            cboLocTrangThai.SelectedIndex = 0;
        }

        private void LoadDetailLoaiKMComboBox()
        {
            var loaiKMList = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("", "-- Chọn loại KM --"), // Thêm item trống để người dùng chọn
                new KeyValuePair<string, string>("Phần trăm", "Phần trăm"),
                new KeyValuePair<string, string>("Tặng sản phẩm", "Tặng sản phẩm"),
                new KeyValuePair<string, string>("Giảm giá trực tiếp", "Giảm giá trực tiếp")
            };
            cboLoaiKM.DataSource = new BindingSource(loaiKMList, null);
            cboLoaiKM.DisplayMember = "Value";
            cboLoaiKM.ValueMember = "Key";
            cboLoaiKM.SelectedIndex = 0; // Chọn item "-- Chọn loại KM --"
        }


        private void LoadAllSanPhamToChuaApDung()
        {
            try
            {
                string sql = "SELECT MaSP, TenSP FROM SanPham ORDER BY TenSP";
                // Gọi hàm mới FillListBox
                Function.FillListBox(lstSPChuaApDung, "TenSP", "MaSP", sql);
                lstSPChuaApDung.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetValues()
        {
            txtMaKM.Text = "";
            txtTenKM.Text = "";
            if (cboLoaiKM.Items.Count > 0) cboLoaiKM.SelectedIndex = 0; // Về "-- Chọn loại KM --"
            txtGiaTri.Text = "0";
            dtpNgayBD.Value = DateTime.Now;
            dtpNgayKT.Value = DateTime.Now.AddDays(7);
            txtMoTa.Text = "";
            txtDKApDung.Text = "";
            txtDKSoLuongCanMua.Text = "0"; // Tên control trong designer là textBox2
            txtDKSoLuongDuocTang.Text = "0"; // Tên control trong designer là textBox1
            chkHoatDong.Checked = true;

            // Xóa ListBox sản phẩm đã áp dụng
            if (lstSPDaApDung.DataSource != null)
            {
                if (lstSPDaApDung.DataSource is DataTable dt) dt.Clear();
                else lstSPDaApDung.DataSource = null;
            }
            else
            {
                lstSPDaApDung.Items.Clear();
            }
            lstSPDaApDung.SelectedIndex = -1;


            // Nạp lại tất cả sản phẩm vào lstSPChuaApDung
            LoadAllSanPhamToChuaApDung();
            UpdateRelatedControlsVisibility(); // Gọi để cập nhật giao diện theo cboLoaiKM
        }

        private void SetButtonStates(string state)
        {
            switch (state.ToLower())
            {
                case "initial":
                    btnThem.Enabled = true;
                    btnSua.Enabled = false;
                    BtnXoa.Enabled = false;
                    btnLuu.Enabled = false;
                    btnHuy.Enabled = false;
                    txtMaKM.ReadOnly = true;
                    isAddingNew = false;
                    dgvKhuyenMai.ClearSelection(); // Bỏ chọn dòng trên DataGridView
                    break;
                case "selected":
                    btnThem.Enabled = true;
                    btnSua.Enabled = true;
                    BtnXoa.Enabled = true;
                    btnLuu.Enabled = false; // Không cho lưu khi chỉ chọn
                    btnHuy.Enabled = true;  // Cho phép "Bỏ qua" để quay về trạng thái initial
                    txtMaKM.ReadOnly = true;
                    isAddingNew = false;
                    break;
                case "adding":
                    btnThem.Enabled = false;
                    btnSua.Enabled = false;
                    BtnXoa.Enabled = false;
                    btnLuu.Enabled = true;
                    btnHuy.Enabled = true;
                    txtMaKM.ReadOnly = true; 
                    isAddingNew = true;
                    break;
                case "editing":
                    btnThem.Enabled = false;
                    btnSua.Enabled = false;
                    BtnXoa.Enabled = false;
                    btnLuu.Enabled = true;
                    btnHuy.Enabled = true;
                    txtMaKM.ReadOnly = true; // Không cho sửa Mã KM
                    isAddingNew = false;
                    break;
            }
            // Sau khi thay đổi trạng thái nút, cập nhật hiển thị các control phụ thuộc
            UpdateRelatedControlsVisibility();
        }

        private void SetInputControlsEnabled(bool enabled)
        {
            txtTenKM.ReadOnly = !enabled;
            dtpNgayBD.Enabled = enabled;
            dtpNgayKT.Enabled = enabled;
            txtMoTa.ReadOnly = !enabled;
            txtDKApDung.ReadOnly = !enabled;
            chkHoatDong.Enabled = true;

            txtGiaTri.Enabled = enabled;
            txtDKSoLuongCanMua.Enabled = enabled;
            txtDKSoLuongDuocTang.Enabled = enabled;

            UpdateRelatedControlsVisibility();
        }

        private void dgvKhuyenMai_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (btnLuu.Enabled) // Nếu đang ở chế độ thêm/sửa dở dang
            {
                DialogResult result = MessageBox.Show("Dữ liệu chưa được lưu. Bạn có muốn hủy thay đổi và chọn khuyến mãi khác không?",
                                                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                {
                    return; // Không làm gì cả
                }
            }

            if (e.RowIndex >= 0 && e.RowIndex < dgvKhuyenMai.Rows.Count)
            {
                if (dgvKhuyenMai.Rows[e.RowIndex].Cells["MaKM"].Value != null)
                {
                    string maKM = dgvKhuyenMai.Rows[e.RowIndex].Cells["MaKM"].Value.ToString();
                    LoadKhuyenMaiDetails(maKM);
                    LoadSanPhamDaApDung(maKM);
                    LoadSanPhamChuaApDung(maKM); // Nạp lại SP chưa áp dụng dựa trên KM hiện tại
                    SetButtonStates("selected");
                    tabChiTietKhuyenMai.SelectedTab = tpThongTinKM;
                }
            }
        }

        private void LoadKhuyenMaiDetails(string maKM)
        {
            string sql = "SELECT MaKM, TenKM, LoaiKM, GiaTri, NgayBatDau, NgayKetThuc, MoTa, DieuKienApDung, TrangThai, DK_SoLuongCanMua, DK_SoLuongDuocTang " +
                         "FROM KhuyenMai WHERE MaKM = @MaKM";
            try
            {
                DataTable dt = Function.GetDataToTable(sql, new SqlParameter("@MaKM", maKM));

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    txtMaKM.Text = row["MaKM"].ToString();
                    txtTenKM.Text = row["TenKM"].ToString();

                    string loaiKMFromDB = row["LoaiKM"].ToString();
                    // Đặt SelectedValue cho cboLoaiKM.
                    // Cần đảm bảo loaiKMFromDB là một trong các "Key" của KeyValuePair trong DataSource.
                    if (!string.IsNullOrEmpty(loaiKMFromDB) && ((BindingSource)cboLoaiKM.DataSource).Cast<KeyValuePair<string, string>>().Any(kvp => kvp.Key == loaiKMFromDB))
                    {
                        cboLoaiKM.SelectedValue = loaiKMFromDB;
                    }
                    else
                    {
                        if (cboLoaiKM.Items.Count > 0) cboLoaiKM.SelectedIndex = 0; // Về item "-- Chọn loại KM --"
                        Console.WriteLine($"Giá trị Loại KM '{loaiKMFromDB}' từ DB không hợp lệ hoặc không tìm thấy trong ComboBox.");
                    }

                    txtGiaTri.Text = row["GiaTri"] != DBNull.Value ? Convert.ToDecimal(row["GiaTri"]).ToString("N0") : "0";
                    // Kiểm tra DBNull trước khi convert DateTime
                    dtpNgayBD.Value = row["NgayBatDau"] != DBNull.Value ? Convert.ToDateTime(row["NgayBatDau"]) : DateTime.Now;
                    dtpNgayKT.Value = row["NgayKetThuc"] != DBNull.Value ? Convert.ToDateTime(row["NgayKetThuc"]) : DateTime.Now.AddDays(7);
                    txtMoTa.Text = row["MoTa"].ToString();
                    txtDKApDung.Text = row["DieuKienApDung"].ToString();
                    txtDKSoLuongCanMua.Text = row["DK_SoLuongCanMua"] != DBNull.Value ? row["DK_SoLuongCanMua"].ToString() : "0";
                    txtDKSoLuongDuocTang.Text = row["DK_SoLuongDuocTang"] != DBNull.Value ? row["DK_SoLuongDuocTang"].ToString() : "0";
                    chkHoatDong.Checked = row["TrangThai"] != DBNull.Value ? Convert.ToBoolean(row["TrangThai"]) : false;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy thông tin khuyến mãi với mã: " + maKM, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ResetValues();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải chi tiết khuyến mãi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetValues();
            }
            finally
            {
                UpdateRelatedControlsVisibility();
            }
        }

        private void LoadSanPhamDaApDung(string maKM)
        {
            try
            {
                string sql = "SELECT sp.MaSP, sp.TenSP " +
                             "FROM SanPham sp " +
                             "INNER JOIN KhuyenMai_SanPham kmsp ON sp.MaSP = kmsp.MaSP " +
                             "WHERE kmsp.MaKM = @MaKM ORDER BY sp.TenSP";
                lstSPDaApDung.DataSource = null;
                Function.FillListBox(lstSPDaApDung, "TenSP", "MaSP", sql, new SqlParameter("@MaKM", maKM));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách sản phẩm đã áp dụng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Xóa an toàn hơn
                lstSPDaApDung.DataSource = null;
                lstSPDaApDung.Items.Clear();
            }
        }

        private void LoadSanPhamChuaApDung(string maKM)
        {
            try
            {
                string sql;
                if (string.IsNullOrEmpty(maKM)) // Trường hợp reset hoặc thêm mới, chưa có MaKM cụ thể
                {
                    sql = "SELECT MaSP, TenSP FROM SanPham ORDER BY TenSP";
                    lstSPChuaApDung.DataSource = null;
                    Function.FillListBox(lstSPChuaApDung, "TenSP", "MaSP", sql);
                }
                else // Trường hợp đã chọn một KM cụ thể
                {
                    sql = "SELECT MaSP, TenSP " +
                          "FROM SanPham " +
                          "WHERE MaSP NOT IN (SELECT MaSP FROM KhuyenMai_SanPham WHERE MaKM = @MaKM) ORDER BY TenSP";
                    Function.FillListBox(lstSPChuaApDung, "TenSP", "MaSP", sql, new SqlParameter("@MaKM", maKM));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách sản phẩm chưa áp dụng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lstSPChuaApDung.DataSource = null;
                lstSPChuaApDung.Items.Clear();
            }
        }

        private void cboLoaiKM_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Chỉ gọi UpdateRelatedControlsVisibility nếu ComboBox đang enabled
            // để tránh việc nó chạy khi form đang load hoặc control đang bị disable bởi SetInputControlsEnabled(false)
            if (cboLoaiKM.Enabled)
            {
                UpdateRelatedControlsVisibility();
            }



        }

        private void UpdateRelatedControlsVisibility()
        {
            string selectedLoaiKM = "";
            if (cboLoaiKM.SelectedValue != null)
            {
                selectedLoaiKM = cboLoaiKM.SelectedValue.ToString();
            }

            // Mặc định ẩn các control đặc thù và các label tương ứng
            lblGiaTri.Visible = false;
            txtGiaTri.Visible = false;
            lblDKSoLuongCanMua.Visible = false;
            txtDKSoLuongCanMua.Visible = false; // Tên Designer là textBox2
            lblDKSoLuongDuocTang.Visible = false;
            txtDKSoLuongDuocTang.Visible = false; // Tên Designer là textBox1

            // Các TextBox này sẽ luôn Enabled=true theo yêu cầu, chỉ quản lý Visible
            txtGiaTri.Enabled = true;
            txtDKSoLuongCanMua.Enabled = true; // textBox2
            txtDKSoLuongDuocTang.Enabled = true; // textBox1

            if (selectedLoaiKM == "Phần trăm")
            {
                lblGiaTri.Text = "Giá trị (%):";
                lblGiaTri.Visible = true;
                txtGiaTri.Visible = true;
            }
            else if (selectedLoaiKM == "Giảm giá trực tiếp")
            {
                lblGiaTri.Text = "Giá trị (VNĐ):";
                lblGiaTri.Visible = true;
                txtGiaTri.Visible = true;
            }
            else if (selectedLoaiKM == "Tặng sản phẩm")
            {
                lblDKSoLuongCanMua.Visible = true;
                txtDKSoLuongCanMua.Visible = true; // textBox2
                lblDKSoLuongDuocTang.Visible = true;
                txtDKSoLuongDuocTang.Visible = true; // textBox1
            }
            // Nếu selectedLoaiKM là rỗng (khi chọn "-- Chọn loại KM --"), tất cả đã được ẩn.
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            isAddingNew = true; // Đặt isAddingNew = true trước ResetValues nếu ResetValues có phụ thuộc vào nó
            ResetValues();      //
            txtMaKM.Text = GenerateNewMaKM(); // Tạo và hiển thị mã KM mới
            SetButtonStates("adding");        // Áp dụng trạng thái cho các nút và controls
            txtTenKM.Focus();
        }

        private void btnThemTatCaApDung_Click(object sender, EventArgs e)
        {
            MoveListBoxItems(lstSPChuaApDung, lstSPDaApDung, true);
        }

        private void btnThemChonApDung_Click(object sender, EventArgs e)
        {
            MoveListBoxItems(lstSPChuaApDung, lstSPDaApDung, false);
        }

        private void btnBoChon_Click(object sender, EventArgs e)
        {
            MoveListBoxItems(lstSPDaApDung, lstSPChuaApDung, false);
        }

        private void btnBoTatCaApDung_Click(object sender, EventArgs e)
        {
            MoveListBoxItems(lstSPDaApDung, lstSPChuaApDung, true);
        }

        // Helper method để di chuyển các mục giữa hai ListBox (đã được điều chỉnh)
        private void MoveListBoxItems(ListBox sourceListBox, ListBox destinationListBox, bool moveAll)
        {
            DataTable dtSource = sourceListBox.DataSource as DataTable;
            DataTable dtDestination = destinationListBox.DataSource as DataTable;

            // Kiểm tra xem chương trình khuyến mãi đã được chọn hay đang thêm mới chưa
            if (string.IsNullOrEmpty(txtMaKM.Text) && !isAddingNew)
            {
                MessageBox.Show("Vui lòng chọn một chương trình khuyến mãi hoặc thêm mới trước khi áp dụng sản phẩm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Nếu dtDestination chưa được khởi tạo (ví dụ: khi thêm mới KM và đây là lần đầu thêm SP áp dụng)
            if (dtDestination == null && dtSource != null)
            {
                dtDestination = dtSource.Clone(); // Tạo DataTable mới với cấu trúc giống dtSource
                destinationListBox.DataSource = dtDestination;
                destinationListBox.DisplayMember = sourceListBox.DisplayMember;
                destinationListBox.ValueMember = sourceListBox.ValueMember;
            }
            else if (dtSource == null || dtDestination == null)
            {
                MessageBox.Show("Lỗi: DataSource của ListBox chưa được thiết lập đúng cách.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<DataRow> rowsToMove = new List<DataRow>();

            if (moveAll)
            {
                if (dtSource.Rows.Count == 0) return; // Không có gì để di chuyển
                                                      // Lấy tất cả các hàng từ DataTable nguồn
                foreach (DataRow row in dtSource.Rows)
                {
                    rowsToMove.Add(row);
                }
            }
            else
            {
                if (sourceListBox.SelectedItems.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn ít nhất một sản phẩm để di chuyển.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                // Lấy các hàng được chọn
                foreach (DataRowView drv in sourceListBox.SelectedItems)
                {
                    rowsToMove.Add(drv.Row);
                }
            }

            if (rowsToMove.Any())
            {
                foreach (DataRow row in rowsToMove)
                {
                    // ImportRow sẽ tạo một bản sao của DataRow và thêm vào dtDestination
                    // Điều này đảm bảo DataRow không thuộc về hai DataTable cùng lúc.
                    dtDestination.ImportRow(row);
                }

                // Sau khi đã import hết vào destination, xóa chúng khỏi source
                // Cần duyệt ngược hoặc tạo một bản sao của danh sách các hàng cần xóa
                // để tránh lỗi thay đổi collection khi đang duyệt.
                // Ở đây rowsToMove chứa các DataRow gốc từ dtSource.
                foreach (DataRow rowToRemove in rowsToMove)
                {
                    // Kiểm tra xem hàng đó có thực sự thuộc dtSource không trước khi xóa
                    // (Mặc dù trong logic này thì nó luôn thuộc)
                    if (rowToRemove.Table == dtSource)
                    {
                        dtSource.Rows.Remove(rowToRemove);
                    }
                }

                // Các ListBox sẽ tự động cập nhật vì chúng được bind với DataTable.
                // Đặt lại selected index để tránh lỗi nếu mục được chọn đã bị di chuyển.
                sourceListBox.SelectedIndex = -1;
                destinationListBox.SelectedIndex = -1;
            }
        }

        // Phương thức này bạn đã có hoặc có thể sử dụng lại từ gợi ý trước
        private void MoveSingleListBoxItem(ListBox sourceListBox, ListBox destinationListBox)
        {
            if (sourceListBox.SelectedItem == null)
            {
                return;
            }

            DataRowView drvToMove = sourceListBox.SelectedItem as DataRowView;
            if (drvToMove == null)
            {
                return;
            }

            DataTable dtSource = sourceListBox.DataSource as DataTable;
            DataTable dtDestination = destinationListBox.DataSource as DataTable;

            if (string.IsNullOrEmpty(txtMaKM.Text) && !isAddingNew)
            {
                MessageBox.Show("Vui lòng chọn một chương trình khuyến mãi hoặc thêm mới trước khi thay đổi sản phẩm áp dụng.",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                return;
            }

            if (dtDestination == null && dtSource != null)
            {
                dtDestination = dtSource.Clone();
                destinationListBox.DataSource = dtDestination;
                destinationListBox.DisplayMember = sourceListBox.DisplayMember;
                destinationListBox.ValueMember = sourceListBox.ValueMember;
            }
            else if (dtSource == null || dtDestination == null)
            {
                MessageBox.Show("Lỗi: DataSource của ListBox chưa được thiết lập đúng cách hoặc không thể khởi tạo.",
                                "Lỗi",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            DataRow rowToMove = drvToMove.Row;
            dtDestination.ImportRow(rowToMove);
            dtSource.Rows.Remove(rowToMove);

            sourceListBox.SelectedIndex = -1;
            destinationListBox.SelectedIndex = -1;
        }


        // Đặt hàm này bên trong lớp KhuyenMai của bạn
        private string GenerateNewMaKM()
        {
            string newMaKM = "KM001"; // Giá trị mặc định nếu chưa có khuyến mãi nào
                                      // SQL để lấy MaKM lớn nhất có dạng KMxxx (ví dụ: KM001, KM012, KM123)
            string sql = "SELECT MAX(MaKM) FROM KhuyenMai WHERE MaKM LIKE 'KM[0-9][0-9][0-9]%'";
            try
            {
                object result = Function.GetFieldValue(sql); // Giả sử Function.GetFieldValues trả về giá trị của một trường

                if (result != null && result != DBNull.Value && !string.IsNullOrEmpty(result.ToString()))
                {
                    string lastMaKM = result.ToString(); // Ví dụ: "KM009" hoặc "KM099" hoặc "KM100"
                                                         // Tách phần số từ chuỗi MaKM (bỏ đi 2 ký tự "KM" đầu)
                    string numericPart = lastMaKM.Substring(2);
                    if (int.TryParse(numericPart, out int lastNumber))
                    {
                        int nextNumber = lastNumber + 1;
                        newMaKM = "KM" + nextNumber.ToString("D3"); // "D3" đảm bảo số có 3 chữ số, ví dụ: 001, 010, 100
                    }
                    // Nếu không parse được (định dạng MaKM không như mong đợi), có thể giữ nguyên KM001 hoặc báo lỗi
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tạo mã khuyến mãi mới: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Trong trường hợp lỗi, có thể xem xét việc không cho phép lưu hoặc dùng mã mặc định một cách cẩn trọng
            }
            return newMaKM;
        }

        // Đặt hàm này bên trong lớp KhuyenMai
        private bool ValidatePromotionInputs()
        {
            if (string.IsNullOrWhiteSpace(txtTenKM.Text))
            {
                MessageBox.Show("Tên chương trình khuyến mãi không được để trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenKM.Focus();
                return false;
            }

            if (cboLoaiKM.SelectedValue == null || string.IsNullOrEmpty(cboLoaiKM.SelectedValue.ToString()))
            {
                MessageBox.Show("Vui lòng chọn loại khuyến mãi.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboLoaiKM.Focus();
                return false;
            }

            string loaiKM = cboLoaiKM.SelectedValue.ToString();

            if (loaiKM == "Phần trăm" || loaiKM == "Giảm giá trực tiếp")
            {
                if (string.IsNullOrWhiteSpace(txtGiaTri.Text))
                {
                    MessageBox.Show("Giá trị khuyến mãi không được để trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtGiaTri.Focus();
                    return false;
                }
                if (!decimal.TryParse(txtGiaTri.Text, out decimal giaTri) || giaTri < 0)
                {
                    MessageBox.Show("Giá trị khuyến mãi không hợp lệ. Phải là số không âm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtGiaTri.Focus();
                    return false;
                }
                if (loaiKM == "Phần trăm" && (giaTri > 100)) // Cho phép 0% đến 100%
                {
                    MessageBox.Show("Giá trị khuyến mãi theo phần trăm phải từ 0 đến 100.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtGiaTri.Focus();
                    return false;
                }
            }
            else if (loaiKM == "Tặng sản phẩm")
            {
                if (!int.TryParse(txtDKSoLuongCanMua.Text, out int slCanMua) || slCanMua <= 0)
                {
                    MessageBox.Show("Điều kiện số lượng cần mua không hợp lệ. Phải là số nguyên dương.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDKSoLuongCanMua.Focus();
                    return false;
                }
                if (!int.TryParse(txtDKSoLuongDuocTang.Text, out int slDuocTang) || slDuocTang <= 0)
                {
                    MessageBox.Show("Điều kiện số lượng được tặng không hợp lệ. Phải là số nguyên dương.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDKSoLuongDuocTang.Focus();
                    return false;
                }
            }

            if (dtpNgayKT.Value < dtpNgayBD.Value)
            {
                MessageBox.Show("Ngày kết thúc không được nhỏ hơn ngày bắt đầu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpNgayKT.Focus();
                return false;
            }
            else if (dtpNgayKT.Value < DateTime.Now)
            {
                MessageBox.Show("Ngày kết thúc không được nhỏ hơn ngày hiện tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpNgayKT.Focus();
                return false;
            }

                // Thêm các kiểm tra khác nếu cần
            return true;
        }

        // Đặt hàm này bên trong lớp KhuyenMai
        private void SaveKhuyenMaiSanPham(string maKM)
        {
            // 1. Xóa tất cả các liên kết sản phẩm cũ của MaKM này trong bảng KhuyenMai_SanPham
            string sqlDelete = "DELETE FROM KhuyenMai_SanPham WHERE MaKM = @MaKM_Delete";
            // Function.RunSql cần có khả năng xử lý tham số
            Function.RunSql(sqlDelete, new System.Data.SqlClient.SqlParameter("@MaKM_Delete", maKM));

            // 2. Thêm các liên kết sản phẩm mới từ lstSPDaApDung
            DataTable dtDaApDung = lstSPDaApDung.DataSource as DataTable;
            if (dtDaApDung != null)
            {
                string sqlInsertSP = "INSERT INTO KhuyenMai_SanPham (MaKM, MaSP) VALUES (@MaKM_InsertSP, @MaSP_InsertSP)";
                foreach (DataRow row in dtDaApDung.Rows)
                {
                    if (row["MaSP"] != DBNull.Value) // Đảm bảo MaSP không null
                    {
                        string maSP = row["MaSP"].ToString();
                        List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>
                        {
                            new System.Data.SqlClient.SqlParameter("@MaKM_InsertSP", maKM),
                            new System.Data.SqlClient.SqlParameter("@MaSP_InsertSP", maSP)
                        };
                        Function.RunSql(sqlInsertSP, spParams.ToArray());
                    }
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (!ValidatePromotionInputs())
            {
                return; // Dừng lại nếu dữ liệu không hợp lệ
            }

            // Hiển thị hộp thoại xác nhận trước khi lưu
            DialogResult confirmResult = MessageBox.Show(
                isAddingNew ? "Bạn có chắc chắn muốn thêm mới khuyến mãi này không?" : "Bạn có chắc chắn muốn cập nhật khuyến mãi này không?",
                "Xác nhận lưu",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmResult == DialogResult.No)
            {
                return; // Người dùng chọn No, không làm gì cả
            }

            string currentMaKM = txtMaKM.Text.Trim(); // Mã KM đã được tạo hoặc lấy từ dòng đang sửa

            // Chuẩn bị câu lệnh SQL và danh sách tham số
            string sqlKhuyenMai;
            List<System.Data.SqlClient.SqlParameter> kmParams = new List<System.Data.SqlClient.SqlParameter>();

            // Các tham số chung cho INSERT và UPDATE
            kmParams.Add(new System.Data.SqlClient.SqlParameter("@TenKM", txtTenKM.Text.Trim()));
            kmParams.Add(new System.Data.SqlClient.SqlParameter("@MoTa", string.IsNullOrWhiteSpace(txtMoTa.Text) ? (object)DBNull.Value : txtMoTa.Text.Trim()));
            kmParams.Add(new System.Data.SqlClient.SqlParameter("@LoaiKM", cboLoaiKM.SelectedValue.ToString()));

            // Xử lý GiaTri, DK_SoLuongCanMua, DK_SoLuongDuocTang dựa trên LoaiKM
            string selectedLoaiKM = cboLoaiKM.SelectedValue.ToString();
            if (selectedLoaiKM == "Phần trăm" || selectedLoaiKM == "Giảm giá trực tiếp")
            {
                kmParams.Add(new System.Data.SqlClient.SqlParameter("@GiaTri", Convert.ToDecimal(txtGiaTri.Text)));
                kmParams.Add(new System.Data.SqlClient.SqlParameter("@DK_SoLuongCanMua", DBNull.Value));
                kmParams.Add(new System.Data.SqlClient.SqlParameter("@DK_SoLuongDuocTang", DBNull.Value));
            }
            else if (selectedLoaiKM == "Tặng sản phẩm")
            {
                kmParams.Add(new System.Data.SqlClient.SqlParameter("@GiaTri", DBNull.Value));
                kmParams.Add(new System.Data.SqlClient.SqlParameter("@DK_SoLuongCanMua", Convert.ToInt32(txtDKSoLuongCanMua.Text)));
                kmParams.Add(new System.Data.SqlClient.SqlParameter("@DK_SoLuongDuocTang", Convert.ToInt32(txtDKSoLuongDuocTang.Text)));
            }
            else // Trường hợp khác, không có các giá trị này
            {
                kmParams.Add(new System.Data.SqlClient.SqlParameter("@GiaTri", DBNull.Value));
                kmParams.Add(new System.Data.SqlClient.SqlParameter("@DK_SoLuongCanMua", DBNull.Value));
                kmParams.Add(new System.Data.SqlClient.SqlParameter("@DK_SoLuongDuocTang", DBNull.Value));
            }

            kmParams.Add(new System.Data.SqlClient.SqlParameter("@NgayBatDau", dtpNgayBD.Value.Date));
            kmParams.Add(new System.Data.SqlClient.SqlParameter("@NgayKetThuc", dtpNgayKT.Value.Date));
            kmParams.Add(new System.Data.SqlClient.SqlParameter("@DieuKienApDung", string.IsNullOrWhiteSpace(txtDKApDung.Text) ? (object)DBNull.Value : txtDKApDung.Text.Trim()));
            kmParams.Add(new System.Data.SqlClient.SqlParameter("@TrangThai", chkHoatDong.Checked));


            if (isAddingNew) // Xử lý thêm mới
            {
                // currentMaKM đã được GenerateNewMaKM() tạo và gán vào txtMaKM.Text
                // Kiểm tra lại sự tồn tại của MaKM (dù đã tự sinh, nhưng để chắc chắn)
                string checkSql = "SELECT MaKM FROM KhuyenMai WHERE MaKM = @MaKM_Check";
                // Function.CheckKey cần trả về true nếu key tồn tại.
                // Giả sử Function.CheckKey có thể nhận tham số, hoặc bạn điều chỉnh nó.
                // Ví dụ: DataTable dtCheck = Function.GetDataToTable(checkSql, new SqlParameter("@MaKM_Check", currentMaKM));
                //        if (dtCheck.Rows.Count > 0) { ... }
                if (Function.GetDataToTable(checkSql, new System.Data.SqlClient.SqlParameter("@MaKM_Check", currentMaKM)).Rows.Count > 0)
                {
                    MessageBox.Show("Mã khuyến mãi '" + currentMaKM + "' đã tồn tại một cách không mong muốn. Vui lòng thử lại.", "Lỗi trùng lặp", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtMaKM.Text = GenerateNewMaKM(); // Thử tạo mã mới
                    return;
                }

                sqlKhuyenMai = @"INSERT INTO KhuyenMai 
                            (MaKM, TenKM, MoTa, LoaiKM, GiaTri, NgayBatDau, NgayKetThuc, DieuKienApDung, TrangThai, DK_SoLuongCanMua, DK_SoLuongDuocTang) 
                          VALUES 
                            (@MaKM_Insert, @TenKM, @MoTa, @LoaiKM, @GiaTri, @NgayBatDau, @NgayKetThuc, @DieuKienApDung, @TrangThai, @DK_SoLuongCanMua, @DK_SoLuongDuocTang)";
                kmParams.Insert(0, new System.Data.SqlClient.SqlParameter("@MaKM_Insert", currentMaKM)); // Thêm MaKM vào đầu danh sách tham số
            }
            else // Xử lý sửa
            {
                sqlKhuyenMai = @"UPDATE KhuyenMai SET 
                            TenKM = @TenKM, MoTa = @MoTa, LoaiKM = @LoaiKM, GiaTri = @GiaTri, 
                            NgayBatDau = @NgayBatDau, NgayKetThuc = @NgayKetThuc, DieuKienApDung = @DieuKienApDung, 
                            TrangThai = @TrangThai, DK_SoLuongCanMua = @DK_SoLuongCanMua, DK_SoLuongDuocTang = @DK_SoLuongDuocTang 
                          WHERE MaKM = @MaKM_Update";
                kmParams.Add(new System.Data.SqlClient.SqlParameter("@MaKM_Update", currentMaKM)); // Thêm MaKM vào cuối (hoặc đầu) cho WHERE
            }

            // Thực thi câu lệnh SQL cho bảng KhuyenMai
            try
            {
                Function.RunSql(sqlKhuyenMai, kmParams.ToArray()); // Function.RunSql cần có khả năng xử lý tham số

                // Nếu lưu KhuyenMai thành công, tiếp tục lưu các sản phẩm được áp dụng
                SaveKhuyenMaiSanPham(currentMaKM);

                MessageBox.Show(isAddingNew ? "Thêm mới khuyến mãi thành công!" : "Cập nhật khuyến mãi thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadDataGridView(); // Tải lại dữ liệu trên DataGridView
                                    // Sau khi lưu thành công, đưa form về trạng thái ban đầu hoặc trạng thái "selected" nếu muốn
                                    // ResetValues(); //
                                    // SetButtonStates("initial"); //

                // Nếu muốn giữ lại thông tin vừa sửa/thêm để xem:
                if (!isAddingNew)
                {
                    LoadKhuyenMaiDetails(currentMaKM); // Tải lại chi tiết của KM vừa sửa
                    LoadSanPhamDaApDung(currentMaKM);   // Tải lại SP áp dụng
                    LoadSanPhamChuaApDung(currentMaKM); // Tải lại SP chưa áp dụng
                    SetButtonStates("selected");        // Đặt trạng thái nút là "selected"
                }
                else
                {
                    ResetValues();
                    SetButtonStates("initial");
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu dữ liệu khuyến mãi: " + ex.Message, "Lỗi Cơ Sở Dữ Liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem đã có khuyến mãi nào được chọn từ DataGridView chưa
            // (thường là kiểm tra txtMaKM có rỗng không sau khi CellClick)
            if (string.IsNullOrEmpty(txtMaKM.Text))
            {
                MessageBox.Show("Vui lòng chọn một chương trình khuyến mãi từ danh sách để sửa.",
                                "Chưa chọn khuyến mãi",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            // Đặt cờ isAddingNew thành false vì chúng ta đang ở chế độ sửa
            isAddingNew = false; // (biến này đã được khai báo)

            // Chuyển form sang trạng thái "editing"
            // Trạng thái "editing" trong SetButtonStates của bạn sẽ:
            // - Vô hiệu hóa btnThem, btnSua, BtnXoa
            // - Kích hoạt btnLuu, btnHuy
            // - Đặt txtMaKM ở chế độ chỉ đọc (ReadOnly = true)
            SetButtonStates("editing"); //

            // Cho phép người dùng chỉnh sửa các trường thông tin.
            // Trong KhuyenMai.cs, bạn có MakeControlsAlwaysEditable() được gọi ở Form_Load,
            // nên hầu hết các controls nhập liệu đã cho phép chỉnh sửa.
            // SetButtonStates("editing") chủ yếu thay đổi trạng thái của các nút lệnh.

            // Đưa con trỏ tới trường đầu tiên có thể sửa, ví dụ: Tên Khuyến Mãi
            txtTenKM.Focus();
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem đã có khuyến mãi nào được chọn từ DataGridView chưa
            if (string.IsNullOrEmpty(txtMaKM.Text))
            {
                MessageBox.Show("Vui lòng chọn một chương trình khuyến mãi từ danh sách để xóa.",
                                "Chưa chọn khuyến mãi",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            // Hiển thị hộp thoại xác nhận trước khi xóa
            DialogResult confirmResult = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa chương trình khuyến mãi '" + txtTenKM.Text + "' (Mã: " + txtMaKM.Text + ") không?\n" +
                "Lưu ý: Mọi thông tin liên quan đến sản phẩm áp dụng cho khuyến mãi này cũng sẽ bị xóa.",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                string maKMToDelete = txtMaKM.Text.Trim();
                try
                {
                    // Bước 1: Xóa các liên kết sản phẩm trong bảng KhuyenMai_SanPham
                    // Bảng KhuyenMai_SanPham có MaKM là khóa ngoại tham chiếu đến KhuyenMai
                    string sqlDeleteSanPhamApDung = "DELETE FROM KhuyenMai_SanPham WHERE MaKM = @MaKM";
                    // Giả định Function.RunSql của bạn có thể xử lý SqlParameter
                    // Tương tự như cách bạn dùng trong NhaCungCap.cs
                    Function.RunSql(sqlDeleteSanPhamApDung, new System.Data.SqlClient.SqlParameter("@MaKM", maKMToDelete));

                    // Bước 2: Xóa bản ghi khuyến mãi chính trong bảng KhuyenMai
                    string sqlDeleteKhuyenMai = "DELETE FROM KhuyenMai WHERE MaKM = @MaKM";
                    Function.RunSql(sqlDeleteKhuyenMai, new System.Data.SqlClient.SqlParameter("@MaKM", maKMToDelete));

                    MessageBox.Show("Xóa chương trình khuyến mãi thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Cập nhật lại giao diện người dùng
                    LoadDataGridView(); // Tải lại dữ liệu trên DataGridView
                    ResetValues();      // Xóa trắng các trường nhập liệu trên form
                    SetButtonStates("initial"); // Đặt lại trạng thái các nút về ban đầu
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa chương trình khuyến mãi: " + ex.Message,
                                    "Lỗi Cơ Sở Dữ Liệu",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
            // Nếu người dùng chọn No, không làm gì cả
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            // Hiển thị hộp thoại xác nhận nếu người dùng muốn hủy 
            DialogResult confirmResult = MessageBox.Show(
                "Bạn có chắc chắn muốn hủy bỏ các thay đổi hiện tại không?",
                "Xác nhận hủy",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmResult == DialogResult.No)
            {
                return; // Người dùng chọn không hủy, không làm gì cả
            }

            // 1. Xóa trắng các trường nhập liệu và đặt lại danh sách sản phẩm
            ResetValues(); 

            // 2. Đặt lại trạng thái của các nút và biến cờ isAddingNew về trạng thái ban đầu.
            SetButtonStates("initial"); //
        }

        private void lstSPChuaApDung_DoubleClick(object sender, EventArgs e)
        {
            MoveSingleListBoxItem(lstSPChuaApDung, lstSPDaApDung);
        }

        private void lstSPDaApDung_DoubleClick(object sender, EventArgs e)
        {
            MoveSingleListBoxItem(lstSPDaApDung, lstSPChuaApDung);
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            StringBuilder sqlBuilder = new StringBuilder("SELECT MaKM, TenKM, LoaiKM, GiaTri, NgayBatDau, NgayKetThuc, DieuKienApDung, TrangThai, DK_SoLuongCanMua, DK_SoLuongDuocTang FROM KhuyenMai WHERE 1=1");
            List<System.Data.SqlClient.SqlParameter> parameters = new List<System.Data.SqlClient.SqlParameter>();

            // 1. Lấy giá trị từ TextBox Tìm kiếm
            string searchTerm = txtTimKiem.Text.Trim();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                sqlBuilder.Append(" AND (MaKM LIKE @SearchTerm OR TenKM LIKE @SearchTerm)");
                parameters.Add(new System.Data.SqlClient.SqlParameter("@SearchTerm", SqlDbType.NVarChar) { Value = "%" + searchTerm + "%" });
            }

            // 2. Lấy giá trị từ ComboBox Lọc Loại Khuyến Mãi
            // cboLocLoaiKM được tải với "Tất cả loại KM" ở index 0
            if (cboLocLoaiKM.SelectedIndex > 0 && cboLocLoaiKM.SelectedValue != null)
            {
                string loaiKMFilter = cboLocLoaiKM.SelectedValue.ToString();
                // Kiểm tra lại giá trị thực sự của "Tất cả loại KM" nếu cần,
                // nhưng SelectedIndex > 0 thường là đủ nếu item "Tất cả" luôn ở đầu.
                if (loaiKMFilter != "Tất cả loại KM") // Đảm bảo không phải là giá trị của item "Tất cả"
                {
                    sqlBuilder.Append(" AND LoaiKM = @LoaiKMFilter");
                    parameters.Add(new System.Data.SqlClient.SqlParameter("@LoaiKMFilter", SqlDbType.NVarChar) { Value = loaiKMFilter });
                }
            }

            // 3. Lấy giá trị từ ComboBox Lọc Trạng Thái
            // cboLocTrangThai được tải với "Tất cả trạng thái" ở index 0
            if (cboLocTrangThai.SelectedIndex > 0 && cboLocTrangThai.SelectedItem != null)
            {
                string trangThaiFilter = cboLocTrangThai.SelectedItem.ToString();
                if (trangThaiFilter == "Đang hoạt động")
                {
                    sqlBuilder.Append(" AND TrangThai = 1"); // Giả sử TrangThai là bit (1=true, 0=false)
                }
                else if (trangThaiFilter == "Không hoạt động")
                {
                    sqlBuilder.Append(" AND TrangThai = 0");
                }
            }

            // Thực thi câu lệnh SQL và cập nhật DataGridView
            try
            {
                // dtKhuyenMai (biến ở mức lớp) sẽ được cập nhật với kết quả tìm kiếm
                dtKhuyenMai = Function.GetDataToTable(sqlBuilder.ToString(), parameters.ToArray()); // Function.GetDataToTable cần hỗ trợ mảng SqlParameter
                dgvKhuyenMai.DataSource = dtKhuyenMai;
                ApplyDataGridViewStyles(); // Áp dụng lại style cho cột

                if (dtKhuyenMai.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy chương trình khuyến mãi nào phù hợp với điều kiện đã chọn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm khuyến mãi: " + ex.Message, "Lỗi Cơ Sở Dữ Liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            // 1. Câu lệnh SQL để lấy toàn bộ dữ liệu KhuyenMai
            string sqlGetAllKhuyenMai = @"SELECT 
                                    MaKM, TenKM, MoTa, LoaiKM, GiaTri, 
                                    NgayBatDau, NgayKetThuc, DieuKienApDung, TrangThai, 
                                    DK_SoLuongCanMua, DK_SoLuongDuocTang 
                                  FROM KhuyenMai 
                                  ORDER BY MaKM"; // Thêm ORDER BY để dữ liệu xuất ra có thứ tự nhất quán
            DataTable dtToExport;

            try
            {
                // 2. Thực thi truy vấn để lấy dữ liệu mới nhất từ CSDL
                dtToExport = Function.GetDataToTable(sqlGetAllKhuyenMai); // Giả sử hàm này không cần tham số cho câu lệnh SELECT *
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi truy vấn dữ liệu khuyến mãi từ cơ sở dữ liệu: " + ex.Message,
                                "Lỗi Cơ Sở Dữ Liệu",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            if (dtToExport == null || dtToExport.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu khuyến mãi trong cơ sở dữ liệu để xuất ra Excel.",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx", ValidateNames = true })
            {
                sfd.FileName = "DanhSachKhuyenMai_Full_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx"; // Tên file gợi ý đã thay đổi
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (XLWorkbook workbook = new XLWorkbook())
                        {
                            var worksheet = workbook.Worksheets.Add("DanhSachKhuyenMai");

                            // --- Tạo Dòng Tiêu Đề ---
                            string[] headers = {
                        "Mã Khuyến Mãi", "Tên Khuyến Mãi", "Mô Tả", "Loại Khuyến Mãi", "Giá Trị",
                        "Ngày Bắt Đầu", "Ngày Kết Thúc", "Điều Kiện Áp Dụng (Text)", "Trạng Thái",
                        "ĐK: Số Lượng Cần Mua", "ĐK: Số Lượng Được Tặng", "Các Mã Sản Phẩm Áp Dụng"
                    };

                            for (int i = 0; i < headers.Length; i++)
                            {
                                worksheet.Cell(1, i + 1).Value = headers[i];
                                worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                                worksheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                                worksheet.Cell(1, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            }

                            // --- Đổ Dữ Liệu Từ DataTable Vào Excel ---
                            int currentRow = 2;
                            foreach (DataRow mainRow in dtToExport.Rows)
                            {
                                // Truy cập các cột bằng tên cột chính xác từ câu lệnh SQL
                                worksheet.Cell(currentRow, 1).SetValue(mainRow["MaKM"].ToString());
                                worksheet.Cell(currentRow, 2).SetValue(mainRow["TenKM"].ToString());
                                worksheet.Cell(currentRow, 3).SetValue(mainRow["MoTa"] != DBNull.Value ? mainRow["MoTa"].ToString() : string.Empty);
                                worksheet.Cell(currentRow, 4).SetValue(mainRow["LoaiKM"] != DBNull.Value ? mainRow["LoaiKM"].ToString() : string.Empty);

                                if (mainRow["GiaTri"] != DBNull.Value)
                                {
                                    worksheet.Cell(currentRow, 5).SetValue(Convert.ToDecimal(mainRow["GiaTri"]));
                                    if (mainRow["LoaiKM"] != DBNull.Value) // Kiểm tra LoaiKM trước khi sử dụng
                                    {
                                        if (mainRow["LoaiKM"].ToString() == "Phần trăm")
                                            worksheet.Cell(currentRow, 5).Style.NumberFormat.Format = "0\"%\""; // Hiển thị dạng 10%
                                        else if (mainRow["LoaiKM"].ToString() == "Giảm giá trực tiếp")
                                            worksheet.Cell(currentRow, 5).Style.NumberFormat.Format = "#,##0"; // Định dạng số tiền
                                    }
                                }
                                else { worksheet.Cell(currentRow, 5).SetValue(string.Empty); }

                                if (mainRow["NgayBatDau"] != DBNull.Value)
                                    worksheet.Cell(currentRow, 6).SetValue(Convert.ToDateTime(mainRow["NgayBatDau"])).Style.DateFormat.Format = "dd/MM/yyyy";
                                else { worksheet.Cell(currentRow, 6).SetValue(string.Empty); }

                                if (mainRow["NgayKetThuc"] != DBNull.Value)
                                    worksheet.Cell(currentRow, 7).SetValue(Convert.ToDateTime(mainRow["NgayKetThuc"])).Style.DateFormat.Format = "dd/MM/yyyy";
                                else { worksheet.Cell(currentRow, 7).SetValue(string.Empty); }

                                worksheet.Cell(currentRow, 8).SetValue(mainRow["DieuKienApDung"] != DBNull.Value ? mainRow["DieuKienApDung"].ToString() : string.Empty);

                                if (mainRow["TrangThai"] != DBNull.Value) // TrangThai là BIT trong CSDL
                                {
                                    worksheet.Cell(currentRow, 9).SetValue(Convert.ToBoolean(mainRow["TrangThai"]) ? "Hoạt động" : "Không hoạt động");
                                }
                                else { worksheet.Cell(currentRow, 9).SetValue(string.Empty); }

                                if (mainRow["DK_SoLuongCanMua"] != DBNull.Value)
                                    worksheet.Cell(currentRow, 10).SetValue(Convert.ToInt32(mainRow["DK_SoLuongCanMua"]));
                                else { worksheet.Cell(currentRow, 10).SetValue(string.Empty); }

                                if (mainRow["DK_SoLuongDuocTang"] != DBNull.Value)
                                    worksheet.Cell(currentRow, 11).SetValue(Convert.ToInt32(mainRow["DK_SoLuongDuocTang"]));
                                else { worksheet.Cell(currentRow, 11).SetValue(string.Empty); }

                                // --- Lấy và ghi Các Mã Sản Phẩm Áp Dụng ---
                                string maKM = mainRow["MaKM"].ToString();
                                string sqlSP = "SELECT MaSP FROM KhuyenMai_SanPham WHERE MaKM = @MaKM_SP_Export";
                                // Giả sử Function.GetDataToTable của bạn có thể xử lý SqlParameter
                                DataTable dtSPAssociated = Function.GetDataToTable(sqlSP, new SqlParameter("@MaKM_SP_Export", maKM));

                                List<string> maSPList = new List<string>();
                                if (dtSPAssociated != null)
                                {
                                    foreach (DataRow spRow in dtSPAssociated.Rows)
                                    {
                                        maSPList.Add(spRow["MaSP"].ToString());
                                    }
                                }
                                worksheet.Cell(currentRow, 12).SetValue(string.Join(",", maSPList));

                                currentRow++;
                            }

                            worksheet.Columns().AdjustToContents();
                            workbook.SaveAs(sfd.FileName);
                            MessageBox.Show("Xuất toàn bộ dữ liệu khuyến mãi ra Excel thành công!\nĐường dẫn: " + sfd.FileName,
                                            "Thông báo",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Information);
                        } // Workbook được giải phóng
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Có lỗi xảy ra khi xuất dữ liệu ra Excel: " + ex.Message,
                                        "Lỗi",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                    }
                }
            }
        }

        private bool TryParseDateFromCell(IXLCell cell, out DateTime result)
        {
            result = DateTime.MinValue;
            if (cell.IsEmpty()) // Nếu ô trống, không phải là ngày hợp lệ
            {
                Console.WriteLine($"    ParseDate: Ô trống.");
                return false;
            }

            // Ưu tiên 1: Nếu kiểu dữ liệu của ô đã là DateTime
            if (cell.DataType == XLDataType.DateTime)
            {
                try
                {
                    result = cell.GetDateTime();
                    Console.WriteLine($"    ParseDate: Lấy trực tiếp từ kiểu DateTime, Giá trị: {result:dd/MM/yyyy}");
                    return true;
                }
                catch (Exception ex) // Có thể xảy ra lỗi nếu giá trị không hợp lệ dù kiểu là DateTime
                {
                    Console.WriteLine($"    ParseDate: Lỗi khi GetDateTime dù kiểu là DateTime: {ex.Message}");
                    return false;
                }
            }
            // Ưu tiên 2: Nếu kiểu dữ liệu là Số (Excel lưu ngày tháng dưới dạng số - OADate)
            else if (cell.DataType == XLDataType.Number)
            {
                Console.WriteLine($"    ParseDate: Ô có kiểu Number, giá trị thô: {cell.CachedValue}"); // CachedValue thường là double cho số
                if (cell.TryGetValue(out double oaDate)) // Thử lấy giá trị double
                {
                    if (oaDate > 0) // Giá trị OADate hợp lệ thường là số dương
                    {
                        try
                        {
                            result = DateTime.FromOADate(oaDate);
                            Console.WriteLine($"    ParseDate: Chuyển từ OADate (double: {oaDate}) thành DateTime: {result:dd/MM/yyyy}");
                            return true;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"    ParseDate: Lỗi khi chuyển từ OADate {oaDate}: {ex.Message}");
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"    ParseDate: Giá trị số OADate ({oaDate}) không hợp lệ (không dương).");
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine($"    ParseDate: Không thể lấy giá trị double từ ô kiểu Number.");
                    return false;
                }
            }
            // Ưu tiên 3: Nếu kiểu dữ liệu là Text, thử parse theo các định dạng phổ biến
            else if (cell.DataType == XLDataType.Text)
            {
                string dateString = cell.GetText().Trim(); // Lấy dạng text và trim khoảng trắng
                Console.WriteLine($"    ParseDate: Ô có kiểu Text, giá trị chuỗi: '{dateString}'");
                if (string.IsNullOrWhiteSpace(dateString)) return false;

                string[] formats = {
                    "dd/MM/yyyy", "d/M/yyyy", "dd-MM-yyyy", "d-M-yyyy",
                    "MM/dd/yyyy", "M/d/yyyy", "MM-dd-yyyy", "M-d-yyyy",
                    "yyyy-MM-dd", "yyyy/MM/dd", "yyyy.MM.dd",
                    "dd/MM/yy", "d/M/yy", "MM/dd/yy", "M/d/yy"
                    // Bạn có thể thêm các định dạng khác mà người dùng có thể nhập
                };
                if (DateTime.TryParseExact(dateString, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
                {
                    Console.WriteLine($"    ParseDate: Parse thành công từ chuỗi Text thành DateTime: {result:dd/MM/yyyy}");
                    return true;
                }
                else
                {
                    // Thử parse với Culture hiện tại nếu InvariantCulture không thành công
                    if (DateTime.TryParse(dateString, CultureInfo.CurrentCulture, DateTimeStyles.None, out result))
                    {
                        Console.WriteLine($"    ParseDate: Parse (CurrentCulture) thành công từ chuỗi Text thành DateTime: {result:dd/MM/yyyy}");
                        return true;
                    }
                    Console.WriteLine($"    ParseDate: Không thể parse chuỗi Text '{dateString}' thành DateTime với các định dạng đã cho.");
                    return false;
                }
            }
            else // Các kiểu dữ liệu khác (Boolean, Error, ...) không được coi là ngày hợp lệ
            {
                Console.WriteLine($"    ParseDate: Kiểu dữ liệu của ô ({cell.DataType}) không được hỗ trợ để parse ngày.");
                return false;
            }
        }


        private void btnNhapExcel_Click(object sender, EventArgs e)
        {
            Console.WriteLine($"\n[{DateTime.Now:HH:mm:ss.fff}] --- BẮT ĐẦU QUÁ TRÌNH NHẬP EXCEL ---");
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "Excel Workbook|*.xlsx", ValidateNames = true, Multiselect = false })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string filePath = ofd.FileName;
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Đã chọn file: {filePath}");

                    int successInsertCount = 0;
                    int successUpdateCount = 0;
                    int errorCount = 0;
                    List<string> errorDetails = new List<string>(); // Lưu chi tiết lỗi cho MessageBox

                    try
                    {
                        using (XLWorkbook workbook = new XLWorkbook(filePath))
                        {
                            var worksheet = workbook.Worksheet(1); // Giả định dữ liệu ở sheet đầu tiên
                            if (worksheet == null || worksheet.LastRowUsed() == null || worksheet.LastRowUsed().RowNumber() < 1)
                            {
                                MessageBox.Show("Không tìm thấy dữ liệu hoặc sheet làm việc trong file Excel.", "Lỗi File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Lỗi: Không tìm thấy dữ liệu hoặc sheet làm việc.");
                                return;
                            }
                            Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Đang làm việc với Sheet: {worksheet.Name}, Tổng số dòng có dữ liệu (bao gồm header nếu có): {worksheet.LastRowUsed().RowNumber()}");

                            var rows = worksheet.RowsUsed().Skip(1); // Bỏ qua dòng tiêu đề (dòng 1)

                            foreach (var excelRow in rows)
                            {
                                int currentRowNum = excelRow.RowNumber();
                                Console.WriteLine($"\n[{DateTime.Now:HH:mm:ss.fff}] == Đang xử lý dòng Excel số: {currentRowNum} ==");

                                try
                                {
                                    // --- 1. Đọc Dữ liệu từ Dòng Excel ---
                                    string excelMaKM = excelRow.Cell(1).GetFormattedString().Trim();
                                    string tenKM = excelRow.Cell(2).GetFormattedString().Trim();
                                    string moTa = excelRow.Cell(3).GetFormattedString().Trim();
                                    string loaiKM = excelRow.Cell(4).GetFormattedString().Trim();
                                    string strGiaTri = excelRow.Cell(5).GetFormattedString().Trim();
                                    IXLCell cellNgayBD = excelRow.Cell(6);
                                    IXLCell cellNgayKT = excelRow.Cell(7);
                                    string dieuKienApDungText = excelRow.Cell(8).GetFormattedString().Trim();
                                    string strTrangThai = excelRow.Cell(9).GetFormattedString().Trim();
                                    string strDKSLMua = excelRow.Cell(10).GetFormattedString().Trim();
                                    string strDKSLTang = excelRow.Cell(11).GetFormattedString().Trim();
                                    string strMaSPApDung = excelRow.Cell(12).GetFormattedString().Trim();

                                    Console.WriteLine($"  Dữ liệu thô: MaKM='{excelMaKM}', TenKM='{tenKM}', LoaiKM='{loaiKM}', GiaTri='{strGiaTri}', NgayBD='{cellNgayBD.GetFormattedString()}', NgayKT='{cellNgayKT.GetFormattedString()}', TrangThai='{strTrangThai}', DKSLMua='{strDKSLMua}', DKSLTang='{strDKSLTang}', SP_ApDung='{strMaSPApDung}'");

                                    // --- 2. Xác thực Dữ liệu Cơ Bản ---
                                    if (string.IsNullOrEmpty(tenKM))
                                    { errorDetails.Add($"Dòng {currentRowNum}: Tên khuyến mãi không được trống."); errorCount++; Console.WriteLine($"  LỖI: Tên KM trống."); continue; }
                                    if (tenKM.Length > 100)
                                    { errorDetails.Add($"Dòng {currentRowNum}: Tên khuyến mãi quá dài (tối đa 100 ký tự)."); errorCount++; Console.WriteLine($"  LỖI: Tên KM quá dài."); continue; }

                                    if (string.IsNullOrEmpty(loaiKM) || !new[] { "Phần trăm", "Tặng sản phẩm", "Giảm giá trực tiếp" }.Contains(loaiKM, StringComparer.OrdinalIgnoreCase))
                                    { errorDetails.Add($"Dòng {currentRowNum}: Loại khuyến mãi '{loaiKM}' không hợp lệ. Phải là 'Phần trăm', 'Tặng sản phẩm', hoặc 'Giảm giá trực tiếp'."); errorCount++; Console.WriteLine($"  LỖI: Loại KM không hợp lệ '{loaiKM}'."); continue; }

                                    DateTime ngayBD, ngayKT;
                                    if (!TryParseDateFromCell(cellNgayBD, out ngayBD))
                                    { errorDetails.Add($"Dòng {currentRowNum}: Ngày bắt đầu '{cellNgayBD.GetFormattedString()}' không hợp lệ hoặc trống."); errorCount++; Console.WriteLine($"  LỖI: Ngày BĐ không hợp lệ."); continue; }
                                    if (!TryParseDateFromCell(cellNgayKT, out ngayKT))
                                    { errorDetails.Add($"Dòng {currentRowNum}: Ngày kết thúc '{cellNgayKT.GetFormattedString()}' không hợp lệ hoặc trống."); errorCount++; Console.WriteLine($"  LỖI: Ngày KT không hợp lệ."); continue; }
                                    if (ngayKT.Date < ngayBD.Date)
                                    { errorDetails.Add($"Dòng {currentRowNum}: Ngày kết thúc không được nhỏ hơn ngày bắt đầu."); errorCount++; Console.WriteLine($"  LỖI: Ngày KT < Ngày BĐ."); continue; }

                                    bool trangThai = (strTrangThai.Equals("Hoạt động", StringComparison.OrdinalIgnoreCase)); // Mặc định là false nếu không khớp
                                    Console.WriteLine($"  Trạng thái đã parse: {trangThai} (từ chuỗi '{strTrangThai}')");

                                    // --- 3. Chuyển đổi và Xác thực Giá Trị Số theo Loại KM ---
                                    decimal? giaTri = null; // Sử dụng nullable decimal
                                    int? dkSLMua = null;   // Sử dụng nullable int
                                    int? dkSLTang = null;  // Sử dụng nullable int

                                    if (loaiKM.Equals("Phần trăm", StringComparison.OrdinalIgnoreCase))
                                    {
                                        string cleanedStrGiaTri = strGiaTri.Replace("%", "").Trim(); // Loại bỏ dấu %
                                        if (string.IsNullOrWhiteSpace(cleanedStrGiaTri) ||
                                            !decimal.TryParse(cleanedStrGiaTri, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal tempGiaTri) || // Dùng NumberStyles.Number cho số thập phân
                                            tempGiaTri < 0 || tempGiaTri > 100)
                                        {
                                            errorDetails.Add($"Dòng {currentRowNum}: Giá trị phần trăm ('{strGiaTri}') không hợp lệ. Phải là số từ 0 đến 100.");
                                            errorCount++;
                                            Console.WriteLine($"  LỖI: Giá trị % ('{strGiaTri}') không hợp lệ hoặc ngoài khoảng 0-100.");
                                            continue;
                                        }
                                        giaTri = tempGiaTri;
                                        Console.WriteLine($"  Parse GIÁ TRỊ (%): Chuỗi gốc='{strGiaTri}', Chuỗi đã làm sạch='{cleanedStrGiaTri}', Giá trị parse được={giaTri}");
                                    }
                                    else if (loaiKM.Equals("Giảm giá trực tiếp", StringComparison.OrdinalIgnoreCase))
                                    {
                                        // Đối với số tiền, cho phép NumberStyles.Any để linh hoạt hơn với dấu phân cách hàng ngàn có thể có từ GetFormattedString
                                        // CultureInfo.CurrentCulture có thể tốt hơn nếu file Excel được tạo trên máy có cài đặt vùng tương tự.
                                        // CultureInfo.InvariantCulture thường mong đợi '.' là dấu thập phân.
                                        // Bạn có thể cần thử cả hai hoặc chọn một cái phù hợp với nguồn dữ liệu Excel của bạn.
                                        // Tạm thời dùng InvariantCulture và NumberStyles.Number (chỉ cho phép dấu thập phân, không cho phép ký hiệu tiền tệ hay dấu cách ngàn trực tiếp trong TryParse)
                                        // Nếu GetFormattedString() trả về số có dấu cách ngàn, bạn cần loại bỏ chúng trước.
                                        // Ví dụ: string cleanedNumericString = Regex.Replace(strGiaTri, @"[^\d\.\-]", ""); // Giữ lại số, dấu chấm, dấu trừ

                                        string cleanedStrGiaTriForAmount = strGiaTri.Replace(CultureInfo.CurrentCulture.NumberFormat.CurrencyGroupSeparator, ""); // Loại bỏ dấu cách ngàn theo culture hiện tại
                                                                                                                                                                  // .Replace(CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator, "."); // Chuẩn hóa dấu thập phân nếu cần

                                        if (string.IsNullOrWhiteSpace(cleanedStrGiaTriForAmount) ||
                                            !decimal.TryParse(cleanedStrGiaTriForAmount, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal tempGiaTri) || // Thử với CurrentCulture trước
                                                                                                                                                                  // Nếu thất bại, thử với InvariantCulture (thường dùng '.' làm dấu thập phân)
                                            (!decimal.TryParse(cleanedStrGiaTriForAmount, NumberStyles.Any, CultureInfo.InvariantCulture, out tempGiaTri) &&
                                             !decimal.TryParse(cleanedStrGiaTriForAmount.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out tempGiaTri)) || // Thử thay thế , bằng . cho Invariant
                                            tempGiaTri < 0)
                                        {
                                            errorDetails.Add($"Dòng {currentRowNum}: Giá trị số tiền ('{strGiaTri}') không hợp lệ hoặc âm.");
                                            errorCount++;
                                            Console.WriteLine($"  LỖI: Giá trị số tiền ('{strGiaTri}') không hợp lệ.");
                                            continue;
                                        }
                                        giaTri = tempGiaTri;
                                        Console.WriteLine($"  Parse GIÁ TRỊ (Số tiền): Chuỗi gốc='{strGiaTri}', Chuỗi đã làm sạch='{cleanedStrGiaTriForAmount}', Giá trị parse được={giaTri}");
                                    }
                                    else if (loaiKM.Equals("Tặng sản phẩm", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (string.IsNullOrWhiteSpace(strDKSLMua) || !int.TryParse(strDKSLMua, out int tempSLMua) || tempSLMua <= 0)
                                        { errorDetails.Add($"Dòng {currentRowNum}: Số lượng cần mua ('{strDKSLMua}') cho 'Tặng sản phẩm' không hợp lệ hoặc trống."); errorCount++; Console.WriteLine($"  LỖI: SL cần mua không hợp lệ."); continue; }
                                        dkSLMua = tempSLMua;

                                        if (string.IsNullOrWhiteSpace(strDKSLTang) || !int.TryParse(strDKSLTang, out int tempSLTang) || tempSLTang <= 0)
                                        { errorDetails.Add($"Dòng {currentRowNum}: Số lượng được tặng ('{strDKSLTang}') cho 'Tặng sản phẩm' không hợp lệ hoặc trống."); errorCount++; Console.WriteLine($"  LỖI: SL được tặng không hợp lệ."); continue; }
                                        dkSLTang = tempSLTang;
                                    }
                                    Console.WriteLine($"  Giá trị đã parse: GiaTri={giaTri}, DKSLMua={dkSLMua}, DKSLTang={dkSLTang}");

                                    // --- 4. Chuẩn bị Tham số SQL ---
                                    List<SqlParameter> kmParams = new List<SqlParameter>
                                    {
                                        new SqlParameter("@TenKM", SqlDbType.NVarChar, 100) { Value = tenKM },
                                        new SqlParameter("@MoTa", SqlDbType.NVarChar, 255) { Value = string.IsNullOrWhiteSpace(moTa) ? (object)DBNull.Value : moTa },
                                        new SqlParameter("@LoaiKM", SqlDbType.NVarChar, 50) { Value = loaiKM },
                                        new SqlParameter("@GiaTri", SqlDbType.Decimal) { Precision = 10, Scale = 2, Value = giaTri.HasValue ? (object)giaTri.Value : DBNull.Value },
                                        new SqlParameter("@NgayBatDau", SqlDbType.Date) { Value = ngayBD.Date },
                                        new SqlParameter("@NgayKetThuc", SqlDbType.Date) { Value = ngayKT.Date },
                                        new SqlParameter("@DieuKienApDung", SqlDbType.NVarChar, 255) { Value = string.IsNullOrWhiteSpace(dieuKienApDungText) ? (object)DBNull.Value : dieuKienApDungText },
                                        new SqlParameter("@TrangThai", SqlDbType.Bit) { Value = trangThai },
                                        new SqlParameter("@DK_SoLuongCanMua", SqlDbType.Int) { Value = dkSLMua.HasValue ? (object)dkSLMua.Value : DBNull.Value },
                                        new SqlParameter("@DK_SoLuongDuocTang", SqlDbType.Int) { Value = dkSLTang.HasValue ? (object)dkSLTang.Value : DBNull.Value }
                                    };

                                    // --- 5. Xử lý Thêm mới hoặc Cập nhật ---
                                    string actualMaKMToSave = excelMaKM;
                                    bool isUpdateOperation = false;

                                    if (!string.IsNullOrEmpty(excelMaKM))
                                    {
                                        // Kiểm tra MaKM có tồn tại không
                                        string checkSql = "SELECT COUNT(*) FROM KhuyenMai WHERE MaKM = @MaKM_Check";
                                        int count = Convert.ToInt32(Function.GetFieldValue(checkSql, new SqlParameter("@MaKM_Check", excelMaKM))); // Giả sử GetFieldValues trả về object
                                        if (count > 0)
                                        {
                                            isUpdateOperation = true;
                                            Console.WriteLine($"  Hành động: CẬP NHẬT cho MaKM = {actualMaKMToSave}");
                                        }
                                        else
                                        {
                                            // MaKM có trong Excel nhưng không có trong DB -> có thể là lỗi hoặc người dùng muốn dùng mã này
                                            // Tạm thời coi như thêm mới với mã này nếu nó hợp lệ, hoặc báo lỗi.
                                            // Để an toàn, nếu mã không tồn tại, ta sẽ tạo mã mới.
                                            Console.WriteLine($"  CẢNH BÁO: MaKM '{excelMaKM}' có trong Excel nhưng không tồn tại trong CSDL. Sẽ tạo mã mới.");
                                            actualMaKMToSave = GenerateNewMaKM();
                                            isUpdateOperation = false; // Chuyển sang thêm mới
                                        }
                                    }
                                    else // MaKM trống -> Thêm mới
                                    {
                                        actualMaKMToSave = GenerateNewMaKM();
                                        Console.WriteLine($"  Hành động: THÊM MỚI với MaKM tự sinh = {actualMaKMToSave}");
                                    }


                                    string sqlKhuyenMai;
                                    if (isUpdateOperation)
                                    {
                                        sqlKhuyenMai = @"UPDATE KhuyenMai SET 
                                                TenKM = @TenKM, MoTa = @MoTa, LoaiKM = @LoaiKM, GiaTri = @GiaTri, 
                                                NgayBatDau = @NgayBatDau, NgayKetThuc = @NgayKetThuc, DieuKienApDung = @DieuKienApDung, 
                                                TrangThai = @TrangThai, DK_SoLuongCanMua = @DK_SoLuongCanMua, DK_SoLuongDuocTang = @DK_SoLuongDuocTang 
                                              WHERE MaKM = @MaKM_Update";
                                        kmParams.Add(new SqlParameter("@MaKM_Update", SqlDbType.VarChar, 10) { Value = actualMaKMToSave });
                                    }
                                    else // Thêm mới
                                    {
                                        sqlKhuyenMai = @"INSERT INTO KhuyenMai 
                                                (MaKM, TenKM, MoTa, LoaiKM, GiaTri, NgayBatDau, NgayKetThuc, DieuKienApDung, TrangThai, DK_SoLuongCanMua, DK_SoLuongDuocTang) 
                                              VALUES 
                                                (@MaKM_Insert, @TenKM, @MoTa, @LoaiKM, @GiaTri, @NgayBatDau, @NgayKetThuc, @DieuKienApDung, @TrangThai, @DK_SoLuongCanMua, @DK_SoLuongDuocTang)";
                                        kmParams.Add(new SqlParameter("@MaKM_Insert", SqlDbType.VarChar, 10) { Value = actualMaKMToSave });
                                    }

                                    Function.RunSql(sqlKhuyenMai, kmParams.ToArray());
                                    Console.WriteLine($"  Đã {(isUpdateOperation ? "cập nhật" : "thêm mới")} thông tin chính cho KM: {actualMaKMToSave}");

                                    // --- 6. Xử lý Sản phẩm Áp dụng ---
                                    // Xóa các liên kết cũ
                                    string sqlDeleteSP = "DELETE FROM KhuyenMai_SanPham WHERE MaKM = @MaKM_DeleteSP";
                                    Function.RunSql(sqlDeleteSP, new SqlParameter("@MaKM_DeleteSP", actualMaKMToSave));
                                    Console.WriteLine($"  Đã xóa liên kết sản phẩm cũ cho {actualMaKMToSave} (nếu có).");

                                    // Thêm liên kết mới
                                    if (!string.IsNullOrWhiteSpace(strMaSPApDung))
                                    {
                                        string[] maSPArray = strMaSPApDung.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                                                                       .Select(sp => sp.Trim())
                                                                       .Where(sp => !string.IsNullOrWhiteSpace(sp))
                                                                       .ToArray();
                                        if (maSPArray.Length > 0)
                                        {
                                            string sqlInsertSP = "INSERT INTO KhuyenMai_SanPham (MaKM, MaSP) VALUES (@MaKM_SP, @MaSP_Link)";
                                            int spLinkedCount = 0;
                                            foreach (string maSP in maSPArray)
                                            {
                                                // (Tùy chọn) Kiểm tra MaSP có tồn tại trong bảng SanPham không
                                                string checkSPSql = "SELECT COUNT(*) FROM SanPham WHERE MaSP = @MaSP_CheckSP";
                                                int spExists = Convert.ToInt32(Function.GetFieldValue(checkSPSql, new SqlParameter("@MaSP_CheckSP", maSP)));
                                                if (spExists == 0)
                                                {
                                                    Console.WriteLine($"    CẢNH BÁO: Mã sản phẩm '{maSP}' không tồn tại trong bảng SanPham. Bỏ qua liên kết.");
                                                    errorDetails.Add($"Dòng {currentRowNum}, KM {actualMaKMToSave}: Mã sản phẩm '{maSP}' không tồn tại.");
                                                    continue; // Bỏ qua MaSP không hợp lệ này
                                                }

                                                Function.RunSql(sqlInsertSP, new SqlParameter[] {
                                                    new SqlParameter("@MaKM_SP", actualMaKMToSave),
                                                    new SqlParameter("@MaSP_Link", maSP)
                                                });
                                                spLinkedCount++;
                                            }
                                            Console.WriteLine($"  Đã liên kết {spLinkedCount} sản phẩm với {actualMaKMToSave}.");
                                        }
                                    }

                                    if (isUpdateOperation) successUpdateCount++; else successInsertCount++;
                                }
                                catch (Exception exRow)
                                {
                                    errorCount++;
                                    string errorMsg = $"Dòng {currentRowNum}: Lỗi - {exRow.Message}";
                                    errorDetails.Add(errorMsg);
                                    Console.WriteLine($"  LỖI NGHIÊM TRỌNG: {errorMsg} \nStackTrace: {exRow.StackTrace}");
                                }
                            } // Kết thúc vòng lặp duyệt row
                        } // Workbook được dispose

                        // --- 7. Thông báo Kết quả ---
                        StringBuilder summary = new StringBuilder();
                        summary.AppendLine("Quá trình nhập Excel hoàn tất.");
                        summary.AppendLine($"Số khuyến mãi được thêm mới thành công: {successInsertCount}");
                        summary.AppendLine($"Số khuyến mãi được cập nhật thành công: {successUpdateCount}");
                        summary.AppendLine($"Số dòng gặp lỗi: {errorCount}");

                        if (errorCount > 0)
                        {
                            summary.AppendLine("\nChi tiết các lỗi:");
                            foreach (string err in errorDetails.Take(10)) // Hiển thị tối đa 10 lỗi đầu tiên trong MessageBox
                            {
                                summary.AppendLine("- " + err);
                            }
                            if (errorDetails.Count > 10)
                            {
                                summary.AppendLine($"... và {errorDetails.Count - 10} lỗi khác (xem Console để biết thêm chi tiết).");
                            }
                        }
                        MessageBox.Show(summary.ToString(), "Kết Quả Nhập Liệu", MessageBoxButtons.OK, errorCount > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
                        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] --- KẾT THÚC QUÁ TRÌNH NHẬP EXCEL ---");

                        // Tải lại dữ liệu trên DataGridView
                        LoadDataGridView(); // [cite: KhuyenMai.cs]
                        ResetValues();      // [cite: KhuyenMai.cs]
                        SetButtonStates("initial"); // [cite: KhuyenMai.cs]
                    }
                    catch (IOException ioEx)
                    {
                        MessageBox.Show("Lỗi khi đọc file Excel (có thể file đang được mở bởi chương trình khác): " + ioEx.Message, "Lỗi File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Lỗi IO: {ioEx.Message}");
                    }
                    catch (Exception exFile)
                    {
                        MessageBox.Show("Có lỗi nghiêm trọng xảy ra khi xử lý file Excel: " + exFile.Message, "Lỗi File Excel", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Lỗi File Excel nghiêm trọng: {exFile.Message} \nStackTrace: {exFile.StackTrace}");
                    }
                }
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
