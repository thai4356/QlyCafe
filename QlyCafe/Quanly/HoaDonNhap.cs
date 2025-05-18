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
    public partial class HoaDonNhap : Form
    {
        public DataTable dtChiTietHDN;
        public string maHDNToLoad = null;
        public HoaDonNhap()
        {
            InitializeComponent();
        }

        // Constructor mới để nhận MaHDN khi muốn xem/sửa hóa đơn cũ
        public HoaDonNhap(string maHDN) : this() // Gọi constructor mặc định trước
        {
            this.maHDNToLoad = maHDN;
        }

        public TextBox Get_txtMaHDN()
        {
            return txtMaHDN;
        }

        public void Set_txtMaHDN(string maHDN)
        {
            txtMaHDN.Text = maHDN;
        }

        private void HoaDonNhap_Load(object sender, EventArgs e)
        {
            // Thiết lập trạng thái ban đầu cho các nút
            btnThemHDN.Enabled = true;  // Cho phép tạo hóa đơn mới
            btnLuuHDN.Enabled = false;  // Chỉ bật khi đang thêm/sửa hóa đơn
            btnHuyHDN.Enabled = false;  // Chỉ bật khi đang thêm/sửa hóa đơn
            btnInHDN.Enabled = false;   // Chỉ bật khi đã có hóa đơn để in
            btnDong.Enabled = true;

            // Thiết lập ReadOnly cho các TextBox không nên sửa trực tiếp
            txtMaHDN.ReadOnly = true;
            txtTenNV.ReadOnly = true;
            txtTenNCC.ReadOnly = true;
            txtTenSP.ReadOnly = true;          // Tên sản phẩm trong phần chi tiết
            txtThanhTienDong.ReadOnly = true;  // Thành tiền của một dòng chi tiết
            txtTongTien.ReadOnly = true;       // Tổng tiền của cả hóa đơn

            // Đặt giá trị mặc định
            dpNgayNhap.Value = DateTime.Now;
            txtSoLuong.Text = "0";
            txtDonGia.Text = "0";
            txtKhuyenMai.Text = "";
            txtThanhTienDong.Text = "0";
            txtTongTien.Text = "0";
            lblTongTienBangChu.Text = "Bằng chữ: Không đồng";

            // Tải dữ liệu cho các ComboBox
            // Lưu ý: Hàm Function.FillCombo của bạn cần được điều chỉnh để dùng connString và
            // các hàm Open/Close Connection hoặc using block cho SqlConnection như trong Function.cs bạn đã upload.
            // Mẫu Function.FillCombo bạn đưa ra (từ frmHoadonban_Load) có vẻ dùng trực tiếp tên bảng.
            // Tôi sẽ giả định Function.FillCombo trong Function.cs của bạn hoạt động đúng.

            Function.FillCombo(cboMaNV, "TenNV", "MaNV", "SELECT MaNV, TenNV FROM dbo.NhanVien ORDER BY TenNV");
            cboMaNV.SelectedIndex = -1;
            txtTenNV.Clear();

            Function.FillCombo(cboMaNCC, "TenNCC", "MaNCC", "SELECT MaNCC, TenNCC FROM dbo.NhaCungCap ORDER BY TenNCC");
            cboMaNCC.SelectedIndex = -1;
            txtTenNCC.Clear();

            Function.FillCombo(cboMaSP, "TenSP", "MaSP", "SELECT MaSP, TenSP FROM dbo.SanPham ORDER BY TenSP");
            cboMaSP.SelectedIndex = -1;
            txtTenSP.Clear();

            // Tải danh sách MaHDN đã có để tìm kiếm (nếu cboMaHDNSearch dùng để này)
            // Giả sử cboMaHDNSearch chỉ hiển thị MaHDN
            Function.FillCombo(cboMaHDNSearch, "MaHDN", "MaHDN", "SELECT MaHDN FROM dbo.HoaDonNhap ORDER BY NgayNhap DESC, MaHDN DESC");
            cboMaHDNSearch.SelectedIndex = -1;

            // Khởi tạo DataTable và cấu hình cho dgvChiTietHDN
            SetupDataGridViewChiTietHDN();

            //Nếu form này được gọi với một MaHDN cụ thể(ví dụ từ form tìm kiếm khác)
            // thì bạn sẽ load thông tin hóa đơn đó.
             if (!string.IsNullOrWhiteSpace(this.maHDNToLoad) && !txtMaHDN.Text.StartsWith("HDN_")) // Giả sử mã mới sẽ bắt đầu bằng HDN_ và chưa lưu
            {
                LoadThongTinHoaDon(this.maHDNToLoad); // Load thông tin HĐN và chi tiết HĐN
                btnHuyHDN.Enabled = false; // Nếu đang xem hóa đơn cũ
                btnLuuHDN.Enabled = false; // (Trừ khi bạn cho phép sửa hóa đơn cũ)
                btnInHDN.Enabled = true;
            }
            else
            {
                //Mặc định, chuẩn bị cho việc tạo hóa đơn mới
                PrepareForNewInvoice();
            }

            // Đăng ký các sự kiện SelectedIndexChanged nếu chưa làm trong Designer
            cboMaNV.SelectedIndexChanged += cboMaNV_SelectedIndexChanged;
            cboMaNCC.SelectedIndexChanged += cboMaNCC_SelectedIndexChanged;
            cboMaSP.SelectedIndexChanged += cboMaSP_SelectedIndexChanged;
            txtSoLuong.TextChanged += UpdateThanhTienDong_TextChanged;
            txtDonGia.TextChanged += UpdateThanhTienDong_TextChanged;
            // dgvChiTietHDN.RowsRemoved += DgvChiTietHDN_RowsChanged; // Để cập nhật tổng tiền
            // (Cần xử lý sự kiện khi dtChiTietHDN thay đổi để cập nhật tổng tiền)
        }

        private void PrepareForNewInvoice()
        {
            txtMaHDN.Text = "HDN_" + DateTime.Now.ToString("ddMMyyyy_HHmmss");
            dpNgayNhap.Value = DateTime.Now;
            cboMaNV.SelectedIndex = -1;
            cboMaNCC.SelectedIndex = -1;
            txtTenNV.Clear();
            txtTenNCC.Clear();

            ClearChiTietSanPhamFields();
            dtChiTietHDN.Rows.Clear(); // Xóa các dòng chi tiết cũ (nếu có)
            CapNhatTongTienHoaDon();    // Cập nhật tổng tiền (sẽ là 0)

            btnThemHDN.Text = "Tạo Mới HĐ"; // Hoặc giữ nguyên "Thêm HĐN"
            btnLuuHDN.Enabled = true;
            btnHuyHDN.Enabled = true;
            btnInHDN.Enabled = false;

            // Cho phép nhập liệu cho hóa đơn mới
            EnableHeaderControls(true);
            EnableDetailEntryControls(false); // Chi tiết sản phẩm chỉ bật khi chọn SP
            cboMaNV.Focus();
        }

        private void EnableHeaderControls(bool enable)
        {
            dpNgayNhap.Enabled = enable;
            cboMaNV.Enabled = enable;
            cboMaNCC.Enabled = enable;
        }

        private void EnableDetailEntryControls(bool enable)
        {
            cboMaSP.Enabled = enable;
            txtSoLuong.Enabled = enable;
            txtDonGia.Enabled = enable;
            txtKhuyenMai.Enabled = enable;
            btnThemDong.Enabled = enable;
            // Các nút sửa dòng, xóa dòng, hủy bỏ dòng sẽ được quản lý riêng
        }


        private void SetupDataGridViewChiTietHDN()
        {
            dtChiTietHDN = new DataTable();
            dtChiTietHDN.Columns.Add("MaSP", typeof(string));
            dtChiTietHDN.Columns.Add("TenSP", typeof(string));
            dtChiTietHDN.Columns.Add("SoLuong", typeof(int));
            dtChiTietHDN.Columns.Add("DonGia", typeof(decimal));
            dtChiTietHDN.Columns.Add("KhuyenMai", typeof(string));
            dtChiTietHDN.Columns.Add("ThanhTien", typeof(decimal));

            dgvChiTietHDN.DataSource = dtChiTietHDN;

            // Tùy chỉnh cột (giống như bạn đã làm)
            if (dgvChiTietHDN.Columns["MaSP"] != null) { dgvChiTietHDN.Columns["MaSP"].HeaderText = "Mã SP"; dgvChiTietHDN.Columns["MaSP"].Width = 80; }
            if (dgvChiTietHDN.Columns["TenSP"] != null) { dgvChiTietHDN.Columns["TenSP"].HeaderText = "Tên Sản Phẩm"; dgvChiTietHDN.Columns["TenSP"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; dgvChiTietHDN.Columns["TenSP"].FillWeight = 30; }
            if (dgvChiTietHDN.Columns["SoLuong"] != null) { dgvChiTietHDN.Columns["SoLuong"].HeaderText = "SL"; dgvChiTietHDN.Columns["SoLuong"].Width = 60; dgvChiTietHDN.Columns["SoLuong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; }
            if (dgvChiTietHDN.Columns["DonGia"] != null) { dgvChiTietHDN.Columns["DonGia"].HeaderText = "Đơn Giá"; dgvChiTietHDN.Columns["DonGia"].Width = 100; dgvChiTietHDN.Columns["DonGia"].DefaultCellStyle.Format = "N0"; dgvChiTietHDN.Columns["DonGia"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; }
            if (dgvChiTietHDN.Columns["KhuyenMai"] != null) { dgvChiTietHDN.Columns["KhuyenMai"].HeaderText = "Khuyến Mãi"; dgvChiTietHDN.Columns["KhuyenMai"].Width = 100; }
            if (dgvChiTietHDN.Columns["ThanhTien"] != null) { dgvChiTietHDN.Columns["ThanhTien"].HeaderText = "Thành Tiền"; dgvChiTietHDN.Columns["ThanhTien"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; dgvChiTietHDN.Columns["ThanhTien"].FillWeight = 20; dgvChiTietHDN.Columns["ThanhTien"].DefaultCellStyle.Format = "N0"; dgvChiTietHDN.Columns["ThanhTien"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; }

            dgvChiTietHDN.AllowUserToAddRows = false;
            dgvChiTietHDN.EditMode = DataGridViewEditMode.EditProgrammatically; // Hoặc ReadOnly = true;
            dgvChiTietHDN.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        // Và định nghĩa hàm đó:
        public void ResetFormForNewInvoice()
        {
            // 1. Tạo Mã Hóa Đơn Nhập mới
            txtMaHDN.Text = "HDN_" + DateTime.Now.ToString("ddMMyyyy_HHmmss");
            txtMaHDN.ReadOnly = true;

            // 2. Thiết lập ngày nhập mặc định là ngày hiện tại
            dpNgayNhap.Value = DateTime.Now;

            // 3. Xóa lựa chọn trên các ComboBox và các TextBox liên quan
            cboMaNV.SelectedIndex = -1;
            txtTenNV.Clear();
            cboMaNCC.SelectedIndex = -1;
            txtTenNCC.Clear();

            // 4. Xóa các trường nhập chi tiết sản phẩm
            ClearChiTietSanPhamFields();

            // 5. Xóa dữ liệu trong DataGridView chi tiết và DataTable tạm
            if (dtChiTietHDN != null) // Kiểm tra dtChiTietHDN đã được khởi tạo chưa
            {
                dtChiTietHDN.Rows.Clear();
            }
            // dgvChiTietHDN.Refresh(); // Refresh nếu cần

            // 6. Reset tổng tiền
            txtTongTien.Text = "0";
            lblTongTienBangChu.Text = "Bằng chữ: Không đồng";

            // 7. Thiết lập trạng thái các nút
            btnThemHDN.Text = "Tạo Mới HĐ"; // Hoặc "Thêm HĐN" tùy theo chức năng của nút này
            btnThemHDN.Enabled = true;
            btnLuuHDN.Enabled = true;  // Sẵn sàng để lưu sau khi thêm chi tiết
            btnHuyHDN.Enabled = true;  // Có thể hủy hóa đơn đang tạo
            btnInHDN.Enabled = false; // Chỉ bật sau khi đã lưu hoặc tải hóa đơn cũ

            btnThemDong.Enabled = false; // Chỉ bật khi đã chọn sản phẩm
            btnSuaDong.Enabled = false;
            btnXoaDong.Enabled = false;
            btnHuyBoDong.Enabled = false;

            EnableHeaderControls(true); // Cho phép nhập thông tin chung

            cboMaNV.Focus();
        }

        private void ClearChiTietSanPhamFields()
        {
            cboMaSP.SelectedIndex = -1;
            txtTenSP.Clear();
            txtSoLuong.Text = "0";
            txtDonGia.Text = "0";
            txtKhuyenMai.Clear();
            txtThanhTienDong.Text = "0";
            btnThemDong.Enabled = false; // Tắt nút thêm dòng khi chưa chọn SP
        }

        // --- Các sự kiện SelectedIndexChanged và TextChanged ---
        private void cboMaNV_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMaNV.SelectedValue != null)
            {
                // Giả sử DisplayMember của cboMaNV là TenNV
                txtTenNV.Text = cboMaNV.Text;
            }
            else
            {
                txtTenNV.Clear();
            }
        }

        private void cboMaNCC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMaNCC.SelectedValue != null)
            {
                // Giả sử DisplayMember của cboMaNCC là TenNCC
                txtTenNCC.Text = cboMaNCC.Text;
            }
            else
            {
                txtTenNCC.Clear();
            }
        }

        private void cboMaSP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMaSP.SelectedValue != null)
            {
                // Giả sử DisplayMember của cboMaSP là TenSP
                txtTenSP.Text = cboMaSP.Text;
                EnableDetailEntryControls(true); // Bật các ô nhập SL, Đơn giá
                txtSoLuong.Focus();

                // Lấy giá nhập gợi ý từ bảng SanPham
                string maSP = cboMaSP.SelectedValue.ToString();
                string giaNhapStr = Function.GetFieldValue("SELECT GiaNhap FROM SanPham WHERE MaSP = @MaSP",
                                                        new SqlParameter("@MaSP", maSP));
                if (decimal.TryParse(giaNhapStr, out decimal giaNhap))
                {
                    txtDonGia.Text = giaNhap.ToString("N0"); // Gán vào ô Đơn giá nhập
                }
                else
                {
                    txtDonGia.Text = "0";
                }
            }
            else
            {
                txtTenSP.Clear();
                txtDonGia.Text = "0";
                EnableDetailEntryControls(false); // Tắt nếu không chọn SP
            }
            UpdateThanhTienDong(); // Tính lại thành tiền
        }

        private void UpdateThanhTienDong_TextChanged(object sender, EventArgs e)
        {
            UpdateThanhTienDong();
        }

        private void UpdateThanhTienDong()
        {
            int soLuong;
            decimal donGia;

            bool isSoLuongValid = int.TryParse(txtSoLuong.Text, out soLuong);
            bool isDonGiaValid = decimal.TryParse(txtDonGia.Text.Replace(",", ""), out donGia);

            if (isSoLuongValid && isDonGiaValid && soLuong >= 0 && donGia >= 0)
            {
                txtThanhTienDong.Text = (soLuong * donGia).ToString("N0");
            }
            else
            {
                txtThanhTienDong.Text = "0";
            }
        }

        public void CapNhatTongTienHoaDon()
        {
            decimal tongTien = 0;
            foreach (DataRow dr in dtChiTietHDN.Rows)
            {
                if (dr["ThanhTien"] != DBNull.Value)
                {
                    tongTien += Convert.ToDecimal(dr["ThanhTien"]);
                }
            }
            txtTongTien.Text = tongTien.ToString("N0");
            // Cập nhật lblTongTienBangChu nếu bạn có hàm chuyển số thành chữ
            // lblTongTienBangChu.Text = "Bằng chữ: " + Function.ChuyenSoSangChu(tongTien.ToString());
        }


        // --- Hàm LoadThongTinHoaDon (Để xem/sửa hóa đơn cũ) ---
        // Hàm này sẽ được gọi khi bạn chọn một MaHDN từ cboMaHDNSearch
        // hoặc nếu Form được mở với một MaHDN cụ thể.
        public void LoadThongTinHoaDon(string maHDNCanLoad)
        {
            if (string.IsNullOrWhiteSpace(maHDNCanLoad)) return;

            // 1. Load thông tin Header Hóa Đơn
            string sqlHeader = "SELECT NgayNhap, MaNV, MaNCC, TongTien FROM dbo.HoaDonNhap WHERE MaHDN = @MaHDN";
            DataTable dtHDNHeader = Function.GetDataToTable(sqlHeader, new SqlParameter("@MaHDN", maHDNCanLoad));

            if (dtHDNHeader.Rows.Count > 0)
            {
                DataRow rowHeader = dtHDNHeader.Rows[0];
                txtMaHDN.Text = maHDNCanLoad;
                dpNgayNhap.Value = Convert.ToDateTime(rowHeader["NgayNhap"]);
                cboMaNV.SelectedValue = rowHeader["MaNV"];
                cboMaNCC.SelectedValue = rowHeader["MaNCC"];
                txtTongTien.Text = Convert.ToDecimal(rowHeader["TongTien"]).ToString("N0");
                // lblTongTienBangChu.Text = "Bằng chữ: " + Function.ChuyenSoSangChu(txtTongTien.Text);

                // 2. Load Chi Tiết Hóa Đơn
                string sqlDetails = @"SELECT ct.MaSP, sp.TenSP, ct.SoLuong, ct.DonGia, ct.KhuyenMai, ct.ThanhTien 
                                FROM dbo.ChiTietHDN ct 
                                JOIN dbo.SanPham sp ON ct.MaSP = sp.MaSP 
                                WHERE ct.MaHDN = @MaHDN";
                DataTable dtDetails = Function.GetDataToTable(sqlDetails, new SqlParameter("@MaHDN", maHDNCanLoad));

                dtChiTietHDN.Rows.Clear(); // Xóa chi tiết cũ
                foreach (DataRow dr in dtDetails.Rows)
                {
                    dtChiTietHDN.ImportRow(dr); // Import các dòng vào DataTable đang bind với Grid
                }
                // dgvChiTietHDN.DataSource = dtDetails; // Hoặc gán trực tiếp nếu dtChiTietHDN không dùng nữa

                // Cập nhật trạng thái nút cho chế độ xem/sửa hóa đơn cũ
                btnThemHDN.Enabled = true; // Cho phép tạo hóa đơn mới khác
                btnLuuHDN.Enabled = false;  // Tắt nút Lưu (trừ khi bạn cho phép sửa trực tiếp HĐN cũ)
                btnHuyHDN.Enabled = false; // Vì đang xem HĐN cũ
                btnInHDN.Enabled = true;
                EnableHeaderControls(false); // Không cho sửa thông tin chung của HĐN cũ
                EnableDetailEntryControls(false); // Không cho thêm/sửa dòng chi tiết của HĐN cũ (trừ khi có logic sửa HĐN)

            }
            else
            {
                MessageBox.Show("Không tìm thấy hóa đơn nhập với mã: " + maHDNCanLoad, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ResetFormForNewInvoice(); // Quay về trạng thái tạo mới nếu không tìm thấy
            }
        }


    }
}
