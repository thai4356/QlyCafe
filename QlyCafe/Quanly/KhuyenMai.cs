using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QlyCafe.Quanly
{
    public partial class KhuyenMai : Form
    {
        // Biến cờ để xác định đang Thêm mới hay Sửa
        private bool isAddingNew = false;

        public KhuyenMai()
        {
            InitializeComponent();
        }

        private void KhuyenMai_Load(object sender, EventArgs e)
        {
            LoadDataGridView();
            LoadFilterComboBoxes();
            LoadDetailLoaiKMComboBox(); // Đảm bảo nạp cboLoaiKM chi tiết
            LoadAllSanPhamToChuaApDung(); // Load tất cả sản phẩm vào lstSPChuaApDung ban đầu
            ResetValues();
            SetButtonStates("initial");
            // SetInputControlsEnabled(false); // Sẽ được gọi trong SetButtonStates hoặc ResetValues
        }

        private void LoadDataGridView()
        {
            string sql = "SELECT MaKM, TenKM, LoaiKM, GiaTri, NgayBatDau, NgayKetThuc, DieuKienApDung, TrangThai, DK_SoLuongCanMua, DK_SoLuongDuocTang FROM KhuyenMai";
            try
            {
                DataTable dt = Function.GetDataToTable(sql);
                dgvKhuyenMai.DataSource = dt;

                dgvKhuyenMai.Columns["MaKM"].HeaderText = "Mã KM";
                dgvKhuyenMai.Columns["TenKM"].HeaderText = "Tên Khuyến Mãi";
                dgvKhuyenMai.Columns["LoaiKM"].HeaderText = "Loại KM";
                dgvKhuyenMai.Columns["GiaTri"].HeaderText = "Giá Trị";
                dgvKhuyenMai.Columns["NgayBatDau"].HeaderText = "Ngày BĐ";
                dgvKhuyenMai.Columns["NgayKetThuc"].HeaderText = "Ngày KT";
                dgvKhuyenMai.Columns["DieuKienApDung"].HeaderText = "Điều Kiện (Text)";
                dgvKhuyenMai.Columns["TrangThai"].HeaderText = "Trạng Thái"; // Sẽ hiển thị True/False
                dgvKhuyenMai.Columns["DK_SoLuongCanMua"].HeaderText = "SL Mua";
                dgvKhuyenMai.Columns["DK_SoLuongDuocTang"].HeaderText = "SL Tặng";

                dgvKhuyenMai.Columns["MaKM"].Width = 70;
                dgvKhuyenMai.Columns["TenKM"].Width = 180;
                dgvKhuyenMai.Columns["LoaiKM"].Width = 120;
                dgvKhuyenMai.Columns["GiaTri"].Width = 80;
                dgvKhuyenMai.Columns["DK_SoLuongCanMua"].Width = 70;
                dgvKhuyenMai.Columns["DK_SoLuongDuocTang"].Width = 70;

                dgvKhuyenMai.AllowUserToAddRows = false;
                dgvKhuyenMai.EditMode = DataGridViewEditMode.EditProgrammatically;
                dgvKhuyenMai.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
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
                DataTable dtLoaiKM = Function.GetDataToTable(sqlLoaiKM);
                DataRow dr = dtLoaiKM.NewRow();
                dr["LoaiKM"] = "Tất cả loại KM";
                dtLoaiKM.Rows.InsertAt(dr, 0);
                Function.FillCombo(cboLocLoaiKM, "LoaiKM", "LoaiKM", dtLoaiKM); // Sử dụng overload mới của FillCombo

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
            if (cboLoaiKM.Items.Count > 0) cboLoaiKM.SelectedIndex = 0; else cboLoaiKM.SelectedIndex = -1;
            txtGiaTri.Text = "0";
            dtpNgayBD.Value = DateTime.Now;
            dtpNgayKT.Value = DateTime.Now.AddDays(7);
            txtMoTa.Text = "";
            txtDKApDung.Text = "";
            txtDKSoLuongCanMua.Text = "0";
            txtDKSoLuongDuocTang.Text = "0";
            chkHoatDong.Checked = true; // Mặc định là hoạt động khi thêm mới hoặc reset

            if (lstSPDaApDung.DataSource != null)
            {
                if (lstSPDaApDung.DataSource is DataTable dt) dt.Clear();
                else lstSPDaApDung.DataSource = null; //Hoặc lstSPDaApDung.Items.Clear();
            }
            else lstSPDaApDung.Items.Clear();
            lstSPDaApDung.SelectedIndex = -1;

            // Khi reset, nếu không có dòng nào đang được xử lý, nạp tất cả sản phẩm vào "chưa áp dụng"
            if (string.IsNullOrEmpty(txtMaKM.Text) && !isAddingNew)
            {
                LoadAllSanPhamToChuaApDung();
            }
            SetInputControlsEnabled(false); // Sau khi reset, các control nhập liệu bị vô hiệu hóa
            UpdateRelatedControlsVisibility();
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
                    // btnNhapExcel.Enabled = true;
                    // btnXuatExcel.Enabled = true;
                    isAddingNew = false;
                    SetInputControlsEnabled(false);
                    break;
                case "selected":
                    btnThem.Enabled = true;
                    btnSua.Enabled = true;
                    BtnXoa.Enabled = true;
                    btnLuu.Enabled = false;
                    btnHuy.Enabled = false; // Hoặc true để cho phép hủy việc chọn và reset
                    isAddingNew = false;
                    SetInputControlsEnabled(false);
                    break;
                case "adding":
                    isAddingNew = true;
                    btnThem.Enabled = false;
                    btnSua.Enabled = false;
                    BtnXoa.Enabled = false;
                    btnLuu.Enabled = true;
                    btnHuy.Enabled = true;
                    // btnNhapExcel.Enabled = false;
                    // btnXuatExcel.Enabled = false;
                    SetInputControlsEnabled(true);
                    txtMaKM.ReadOnly = false;
                    break;
                case "editing":
                    isAddingNew = false;
                    btnThem.Enabled = false;
                    btnSua.Enabled = false;
                    BtnXoa.Enabled = false;
                    btnLuu.Enabled = true;
                    btnHuy.Enabled = true;
                    // btnNhapExcel.Enabled = false;
                    // btnXuatExcel.Enabled = false;
                    SetInputControlsEnabled(true);
                    txtMaKM.ReadOnly = true;
                    break;
            }
        }

        private void SetInputControlsEnabled(bool enabled)
        {
            txtTenKM.ReadOnly = !enabled;
            cboLoaiKM.Enabled = enabled; // cboLoaiKM luôn enabled khi các controls khác enabled
            dtpNgayBD.Enabled = enabled;
            dtpNgayKT.Enabled = enabled;
            txtMoTa.ReadOnly = !enabled;
            txtDKApDung.ReadOnly = !enabled;
            chkHoatDong.Enabled = enabled;

            // Các control liên quan đến loại KM sẽ được quản lý cụ thể hơn bởi UpdateRelatedControlsVisibility
            // nhưng trạng thái enabled tổng thể của chúng phụ thuộc vào 'enabled' này.
            txtGiaTri.Enabled = enabled;
            txtDKSoLuongCanMua.Enabled = enabled;
            txtDKSoLuongDuocTang.Enabled = enabled;


            // Tab Sản phẩm áp dụng
            // Các nút và listbox này cũng sẽ enabled/disabled theo trạng thái chung
            btnThemChonApDung.Enabled = enabled;
            btnThemTatCaApDung.Enabled = enabled;
            btnBoChon.Enabled = enabled;
            btnBoTatCaApDung.Enabled = enabled;
            lstSPChuaApDung.Enabled = enabled;
            lstSPDaApDung.Enabled = enabled;

            UpdateRelatedControlsVisibility(); // Gọi để cập nhật hiển thị dựa trên giá trị hiện tại của cboLoaiKM
        }

        private void dgvKhuyenMai_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvKhuyenMai.Rows.Count && dgvKhuyenMai.Rows[e.RowIndex].Cells["MaKM"].Value != null)
            {
                string maKM = dgvKhuyenMai.Rows[e.RowIndex].Cells["MaKM"].Value.ToString();

                LoadKhuyenMaiDetails(maKM); // Sẽ gọi UpdateRelatedControlsVisibility ở cuối
                LoadSanPhamDaApDung(maKM);
                LoadSanPhamChuaApDung(maKM);

                SetButtonStates("selected"); // Sẽ gọi SetInputControlsEnabled(false) và UpdateRelatedControlsVisibility
                tabChiTietKhuyenMai.SelectedTab = tpThongTinKM;
            }
        }

        private void LoadKhuyenMaiDetails(string maKM)
        {
            string sql = "SELECT * FROM KhuyenMai WHERE MaKM = @MaKM";
            try
            {
                DataTable dt = Function.GetDataToTable(sql, new SqlParameter("@MaKM", maKM));

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    txtMaKM.Text = row["MaKM"].ToString();
                    txtTenKM.Text = row["TenKM"].ToString();

                    string loaiKMFromDB = row["LoaiKM"].ToString();
                    // cboLoaiKM đã dùng DataSource, nên dùng SelectedValue
                    if (cboLoaiKM.Items.Count > 0)
                    {
                        // Kiểm tra xem giá trị từ DB có trong danh sách ValueMember của ComboBox không
                        bool valueExists = false;
                        foreach (KeyValuePair<string, string> item in ((BindingSource)cboLoaiKM.DataSource).DataSource as List<KeyValuePair<string, string>>)
                        {
                            if (item.Key.Equals(loaiKMFromDB, StringComparison.OrdinalIgnoreCase))
                            {
                                valueExists = true;
                                break;
                            }
                        }

                        if (valueExists)
                        {
                            cboLoaiKM.SelectedValue = loaiKMFromDB;
                        }
                        else
                        {
                            if (cboLoaiKM.Items.Count > 0) cboLoaiKM.SelectedIndex = 0; else cboLoaiKM.SelectedIndex = -1; // Về item "--Chọn loại--" hoặc trống
                            Console.WriteLine($"Loại KM '{loaiKMFromDB}' từ DB không có trong danh sách chuẩn của ComboBox chi tiết.");
                        }
                    }


                    txtGiaTri.Text = row["GiaTri"] != DBNull.Value ? Convert.ToDecimal(row["GiaTri"]).ToString("N0") : "0";
                    dtpNgayBD.Value = Convert.ToDateTime(row["NgayBatDau"]);
                    dtpNgayKT.Value = Convert.ToDateTime(row["NgayKetThuc"]);
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
                // Đảm bảo UpdateRelatedControlsVisibility được gọi SAU KHI cboLoaiKM đã được set giá trị
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
                Function.FillListBox(lstSPDaApDung, "TenSP", "MaSP", sql, new SqlParameter("@MaKM", maKM));
                lstSPDaApDung.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách sản phẩm đã áp dụng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (lstSPDaApDung.DataSource is DataTable dt) dt.Clear(); else lstSPDaApDung.Items.Clear();
            }
        }

        private void LoadSanPhamChuaApDung(string maKM)
        {
            try
            {
                string sql = "SELECT MaSP, TenSP " +
                             "FROM SanPham " +
                             "WHERE MaSP NOT IN (SELECT MaSP FROM KhuyenMai_SanPham WHERE MaKM = @MaKM) ORDER BY TenSP";
                Function.FillListBox(lstSPChuaApDung, "TenSP", "MaSP", sql, new SqlParameter("@MaKM", maKM));
                lstSPChuaApDung.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách sản phẩm chưa áp dụng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (lstSPChuaApDung.DataSource is DataTable dt) dt.Clear(); else lstSPChuaApDung.Items.Clear();
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
            // Nếu ComboBox chưa sẵn sàng hoặc đang bị disable bởi SetInputControlsEnabled(false) thì không làm gì.
            if (cboLoaiKM.SelectedValue == null || !txtTenKM.ReadOnly == false /*Kiểm tra một control khác cũng được enabled bởi SetInputControlsEnabled(true)*/)
            {
                // Ẩn tất cả các control đặc thù khi không có lựa chọn hoặc đang ở chế độ chỉ đọc
                lblGiaTri.Visible = false;
                txtGiaTri.Visible = false;
                lblDKSoLuongCanMua.Visible = false;
                txtDKSoLuongCanMua.Visible = false;
                lblDKSoLuongDuocTang.Visible = false;
                txtDKSoLuongDuocTang.Visible = false;
                return;
            }

            string selectedLoaiKM = cboLoaiKM.SelectedValue.ToString();

            // Mặc định ẩn tất cả các control đặc thù trước khi quyết định cái nào sẽ hiển thị
            lblGiaTri.Visible = false; txtGiaTri.Visible = false;
            lblDKSoLuongCanMua.Visible = false; txtDKSoLuongCanMua.Visible = false;
            lblDKSoLuongDuocTang.Visible = false; txtDKSoLuongDuocTang.Visible = false;


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
                txtDKSoLuongCanMua.Visible = true;
                lblDKSoLuongDuocTang.Visible = true;
                txtDKSoLuongDuocTang.Visible = true;
            }
            // Nếu selectedLoaiKM là rỗng (khi chọn "-- Chọn loại KM --"), tất cả đã được ẩn ở trên.
        }

    }
}
