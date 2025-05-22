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
        public KhuyenMai()
        {
            InitializeComponent();
        }

        private void KhuyenMai_Load(object sender, EventArgs e)
        {
            LoadDataGridView();
            LoadFilterComboBoxes();
            LoadAllSanPhamToChuaApDung(); // Tải tất cả sản phẩm ban đầu
            ResetValues(); // Đặt các control về trạng thái ban đầu
            SetButtonStates("initial"); // Đặt trạng thái các nút
            SetInputControlsEnabled(false); // Các ô nhập liệu ban đầu không cho phép sửa
        }

        private void LoadDataGridView()
        {
            string sql = "SELECT MaKM, TenKM, LoaiKM, NgayBatDau, NgayKetThuc, DieuKienApDung, TrangThai FROM KhuyenMai WHERE TrangThai = 1"; // Ưu tiên hiển thị KM đang hoạt động
            try
            {
                DataTable dt = Function.GetDataToTable(sql);
                dgvKhuyenMai.DataSource = dt;

                // Đặt tên cho các cột (Header Text)
                dgvKhuyenMai.Columns["MaKM"].HeaderText = "Mã KM";
                dgvKhuyenMai.Columns["TenKM"].HeaderText = "Tên Khuyến Mãi";
                dgvKhuyenMai.Columns["LoaiKM"].HeaderText = "Loại KM";
                dgvKhuyenMai.Columns["NgayBatDau"].HeaderText = "Ngày BĐ";
                dgvKhuyenMai.Columns["NgayKetThuc"].HeaderText = "Ngày KT";
                dgvKhuyenMai.Columns["DieuKienApDung"].HeaderText = "Điều Kiện";
                dgvKhuyenMai.Columns["TrangThai"].HeaderText = "Trạng Thái";

                // (Tùy chọn) Set độ rộng cột
                dgvKhuyenMai.Columns["MaKM"].Width = 80;
                dgvKhuyenMai.Columns["TenKM"].Width = 200;
                dgvKhuyenMai.Columns["LoaiKM"].Width = 120;
                // ...

                // (Tùy chọn) Ẩn cột nếu không muốn hiển thị
                // dgvKhuyenMai.Columns["MoTa"].Visible = false;

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
            // Nạp cboLocLoaiKM
            try
            {
                // Lấy danh sách các loại khuyến mãi duy nhất từ CSDL
                string sqlLoaiKM = "SELECT DISTINCT LoaiKM FROM KhuyenMai WHERE LoaiKM IS NOT NULL AND LoaiKM <> ''";
                DataTable dtLoaiKM = Function.GetDataToTable(sqlLoaiKM);

                // Tạo một dòng mới cho "Tất cả loại KM"
                DataRow newRow = dtLoaiKM.NewRow();
                newRow["LoaiKM"] = "Tất cả loại KM"; // Giả sử cột trong DataTable trả về là "LoaiKM"
                dtLoaiKM.Rows.InsertAt(newRow, 0); // Chèn vào vị trí đầu tiên

                // Gán DataSource cho ComboBox
                cboLocLoaiKM.DataSource = dtLoaiKM;
                cboLocLoaiKM.DisplayMember = "LoaiKM";
                cboLocLoaiKM.ValueMember = "LoaiKM"; // Hoặc một cột ID nếu có
                cboLocLoaiKM.SelectedIndex = 0; // Chọn "Tất cả loại KM" làm mặc định

                // Nạp cboLoaiKM (trong tab chi tiết) - nên lấy từ một bảng danh mục loại KM nếu có, hoặc hardcode
                // Đoạn này không bị lỗi vì nó không dùng DataSource mà thêm trực tiếp Items
                cboLoaiKM.Items.Clear(); // Xóa các item cũ nếu có
                cboLoaiKM.Items.Add("Giảm giá trực tiếp");
                cboLoaiKM.Items.Add("Phần trăm");
                cboLoaiKM.Items.Add("Tặng sản phẩm");
                cboLoaiKM.Items.Add("Mua X tặng Y");
                // ... thêm các loại KM khác nếu có
                cboLoaiKM.SelectedIndex = -1; // Không chọn gì ban đầu
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải loại khuyến mãi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Nạp cboLocTrangThai
            cboLocTrangThai.Items.Clear(); // Xóa các item cũ nếu có
            cboLocTrangThai.Items.Add("Tất cả trạng thái");
            cboLocTrangThai.Items.Add("Đang hoạt động"); // Tương ứng TrangThai = 1 (true)
            cboLocTrangThai.Items.Add("Không hoạt động"); // Tương ứng TrangThai = 0 (false)
            cboLocTrangThai.SelectedIndex = 0;
        }

        private void LoadAllSanPhamToChuaApDung()
        {
            try
            {
                string sql = "SELECT MaSP, TenSP FROM SanPham";
                DataTable dtSanPham = Function.GetDataToTable(sql);
                lstSPChuaApDung.DataSource = dtSanPham;
                lstSPChuaApDung.DisplayMember = "TenSP";
                lstSPChuaApDung.ValueMember = "MaSP";
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
            cboLoaiKM.SelectedIndex = -1;
            txtGiaTri.Text = "0";
            dtpNgayBD.Value = DateTime.Now;
            dtpNgayKT.Value = DateTime.Now.AddDays(7); // Mặc định KM 7 ngày
            txtMoTa.Text = "";
            txtDKApDung.Text = "";
            // Giả sử TrangThai là CheckBox, đặt tên là chkTrangThaiActive
            // chkTrangThaiActive.Checked = true; // Mặc định là đang hoạt động khi thêm mới

            // Xóa dữ liệu trong ListBox sản phẩm đã áp dụng
            if (lstSPDaApDung.DataSource != null)
            {
                // Nếu dùng DataTable làm DataSource
                if (lstSPDaApDung.DataSource is DataTable dt)
                {
                    dt.Clear();
                }
                else // Nếu là List hoặc Collection khác
                {
                    lstSPDaApDung.DataSource = null; // Hoặc lstSPDaApDung.Items.Clear() nếu không dùng DataSource
                }
            }
            else
            {
                lstSPDaApDung.Items.Clear();
            }
            lstSPDaApDung.SelectedIndex = -1;


            // Đảm bảo lstSPChuaApDung vẫn giữ danh sách tất cả sản phẩm nếu không có KM nào được chọn
            // Nếu chưa có sản phẩm nào được chọn hoặc sau khi hủy, có thể gọi lại LoadAllSanPhamToChuaApDung()
            // hoặc chỉ xóa lstSPDaApDung và đảm bảo lstSPChuaApDung không bị ảnh hưởng.
            // Trong trường hợp này, khi Reset, ta muốn lstSPChuaApDung có lại tất cả SP
            // và lstSPDaApDung trống.
            if (dgvKhuyenMai.CurrentRow == null) // Nếu không có KM nào đang được chọn
            {
                LoadAllSanPhamToChuaApDung();
            }
            // Nếu bạn muốn reset luôn cả lstSPChuaApDung thì thêm code xóa ở đây
            // lstSPChuaApDung.DataSource = null;
            // lstSPChuaApDung.Items.Clear();
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
                    btnNhapExcel.Enabled = true; // Cho phép nhập excel ban đầu
                    btnXuatExcel.Enabled = true; // Cho phép xuất excel ban đầu
                    break;
                case "selected": // Khi chọn 1 dòng trên DataGridView
                    btnThem.Enabled = true;
                    btnSua.Enabled = true;
                    BtnXoa.Enabled = true;
                    btnLuu.Enabled = false;
                    btnHuy.Enabled = false; // Có thể enable nút hủy để bỏ chọn
                    break;
                case "adding":
                case "editing":
                    btnThem.Enabled = false;
                    btnSua.Enabled = false;
                    BtnXoa.Enabled = false;
                    btnLuu.Enabled = true;
                    btnHuy.Enabled = true;
                    btnNhapExcel.Enabled = false;
                    btnXuatExcel.Enabled = false;
                    txtMaKM.ReadOnly = (state.ToLower() == "editing"); // Mã KM không sửa khi edit
                    break;
            }
        }

        private void SetInputControlsEnabled(bool enabled)
        {
            // Tab Thông tin KM
            // txtMaKM.ReadOnly = !enabled; // Mã KM thường là ReadOnly khi hiển thị hoặc tự sinh
            txtTenKM.ReadOnly = !enabled;
            cboLoaiKM.Enabled = enabled;
            txtGiaTri.ReadOnly = !enabled;
            dtpNgayBD.Enabled = enabled;
            dtpNgayKT.Enabled = enabled;
            txtMoTa.ReadOnly = !enabled;
            txtDKApDung.ReadOnly = !enabled;
            // chkTrangThaiActive.Enabled = enabled; // Nếu có CheckBox cho trạng thái

            // Tab Sản phẩm áp dụng
            // Cho phép tương tác với các nút chuyển sản phẩm và listbox khi đang thêm/sửa
            btnThemChonApDung.Enabled = enabled;
            btnThemTatCaApDung.Enabled = enabled;
            btnBoChon.Enabled = enabled;
            btnBoTatCaApDung.Enabled = enabled;
            lstSPChuaApDung.Enabled = enabled;
            lstSPDaApDung.Enabled = enabled;
        }

        private void dgvKhuyenMai_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Đảm bảo người dùng click vào một dòng hợp lệ (không phải header)
            if (e.RowIndex >= 0 && e.RowIndex < dgvKhuyenMai.Rows.Count - 1) // loại trừ dòng AddNew nếu có
            {
                // Lấy MaKM từ dòng được chọn
                string maKM = dgvKhuyenMai.Rows[e.RowIndex].Cells["MaKM"].Value.ToString();

                // Nạp thông tin chi tiết của khuyến mãi
                LoadKhuyenMaiDetails(maKM);

                // Nạp danh sách sản phẩm áp dụng và chưa áp dụng
                LoadSanPhamDaApDung(maKM);
                LoadSanPhamChuaApDung(maKM);

                // Cập nhật trạng thái các nút
                SetButtonStates("selected");
                SetInputControlsEnabled(false); // Sau khi chọn, các ô input chưa cho phép sửa
                tabChiTietKhuyenMai.SelectedTab = tpThongTinKM; // Chuyển về tab thông tin KM
            }
            else
            {
                // Nếu click vào header hoặc vị trí không hợp lệ, có thể reset form
                // ResetValues(); // Hoặc không làm gì cả
                // SetButtonStates("initial");
            }
        }

        private void LoadKhuyenMaiDetails(string maKM)
        {
            string sql = "SELECT MaKM, TenKM, LoaiKM, GiaTri, NgayBatDau, NgayKetThuc, MoTa, DieuKienApDung, TrangThai " +
                         "FROM KhuyenMai WHERE MaKM = @MaKM";
            try
            {
                DataTable dt = Function.GetDataToTable(sql, new SqlParameter("@MaKM", maKM));

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    txtMaKM.Text = row["MaKM"].ToString();
                    txtTenKM.Text = row["TenKM"].ToString();

                    // Xử lý cboLoaiKM
                    string loaiKMFromDB = row["LoaiKM"].ToString();
                    bool foundInCombo = false;
                    for (int i = 0; i < cboLoaiKM.Items.Count; i++)
                    {
                        if (cboLoaiKM.Items[i].ToString().Equals(loaiKMFromDB, StringComparison.OrdinalIgnoreCase))
                        {
                            cboLoaiKM.SelectedIndex = i;
                            foundInCombo = true;
                            break;
                        }
                    }
                    if (!foundInCombo)
                    {
                        // Nếu loại KM từ DB không có trong danh sách hardcode của ComboBox
                        // Có thể thêm nó vào, hoặc hiển thị một giá trị mặc định, hoặc báo lỗi
                        // Tạm thời để trống nếu không tìm thấy
                        cboLoaiKM.SelectedIndex = -1;
                        // Hoặc cboLoaiKM.Text = loaiKMFromDB; // Nếu ComboBox cho phép nhập tự do
                        Console.WriteLine($"Loại KM '{loaiKMFromDB}' từ DB không tìm thấy trong ComboBox chi tiết.");
                    }


                    txtGiaTri.Text = row["GiaTri"] != DBNull.Value ? Convert.ToDecimal(row["GiaTri"]).ToString("N0") : "0"; // Định dạng số
                    dtpNgayBD.Value = Convert.ToDateTime(row["NgayBatDau"]);
                    dtpNgayKT.Value = Convert.ToDateTime(row["NgayKetThuc"]);
                    txtMoTa.Text = row["MoTa"].ToString();
                    txtDKApDung.Text = row["DieuKienApDung"].ToString();

                    // Xử lý TrangThai (BIT trong DB)
                    // Giả sử bạn có một CheckBox tên là chkTrangThaiChiTiet để hiển thị trạng thái này (tùy chọn)
                    // Nếu không có, bạn có thể bỏ qua phần này hoặc hiển thị bằng cách khác
                    // if (this.Controls.Find("chkTrangThaiChiTiet", true).FirstOrDefault() is CheckBox chk)
                    // {
                    //    chk.Checked = Convert.ToBoolean(row["TrangThai"]);
                    // }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy thông tin khuyến mãi với mã: " + maKM, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ResetValues(); // Nếu không tìm thấy thì reset form
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải chi tiết khuyến mãi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetValues();
            }
        }

        private void LoadSanPhamDaApDung(string maKM)
        {
            try
            {
                string sql = "SELECT sp.MaSP, sp.TenSP " +
                             "FROM SanPham sp " +
                             "INNER JOIN KhuyenMai_SanPham kmsp ON sp.MaSP = kmsp.MaSP " +
                             "WHERE kmsp.MaKM = @MaKM";
                DataTable dtSPDaApDung = Function.GetDataToTable(sql, new SqlParameter("@MaKM", maKM));

                lstSPDaApDung.DataSource = dtSPDaApDung;
                lstSPDaApDung.DisplayMember = "TenSP";
                lstSPDaApDung.ValueMember = "MaSP";
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
                // Lấy tất cả sản phẩm KHÔNG nằm trong danh sách đã áp dụng cho MaKM hiện tại
                string sql = "SELECT MaSP, TenSP " +
                             "FROM SanPham " +
                             "WHERE MaSP NOT IN (SELECT MaSP FROM KhuyenMai_SanPham WHERE MaKM = @MaKM)";
                DataTable dtSPChuaApDung = Function.GetDataToTable(sql, new SqlParameter("@MaKM", maKM));

                lstSPChuaApDung.DataSource = dtSPChuaApDung;
                lstSPChuaApDung.DisplayMember = "TenSP";
                lstSPChuaApDung.ValueMember = "MaSP";
                lstSPChuaApDung.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách sản phẩm chưa áp dụng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (lstSPChuaApDung.DataSource is DataTable dt) dt.Clear(); else lstSPChuaApDung.Items.Clear();

            }
        }


    }
}
