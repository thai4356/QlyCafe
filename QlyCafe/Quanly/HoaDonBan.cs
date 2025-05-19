using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClosedXML.Excel;

namespace QlyCafe.Quanly
{
    public partial class HoaDonBan : Form
    {
        public DataTable dtChiTietHDB; // Đổi tên từ dtChiTietHDN
        public string maHDBToLoad = null; // Đổi tên từ maHDNToLoad
        private DataTable dtChiTietHDBGoc; // Đổi tên từ dtChiTietHDNGoc
        public HoaDonBan()
        {
            InitializeComponent();
        }

        public HoaDonBan(string maHDB) : this()
        {
            this.maHDBToLoad = maHDB;
        }

        private void HoaDonBan_Load(object sender, EventArgs e)
        {
            LoadComboBoxes();
            CustomizeDgvChiTietHDB(); // Đổi tên hàm

            txtTenNV.ReadOnly = true;
            txtTenKH.ReadOnly = true; // Đổi từ txtTenNCC
            txtTenSP.ReadOnly = true;
            txtThanhTienDong.ReadOnly = true;
            txtTongTien.ReadOnly = true;
            txtMaHDB.ReadOnly = true; // Đổi từ txtMaHDN

            if (!string.IsNullOrEmpty(maHDBToLoad))
            {
                txtMaHDB.Text = maHDBToLoad;
                LoadThongTinHoaDon(maHDBToLoad);
                btnSuaHDB.Enabled = true; // Nút sửa HĐB
            }
            else
            {
                ResetValuesToInitialState();
                if (dtChiTietHDB == null)
                {
                    dtChiTietHDB = new DataTable("ChiTietHDB_Table");
                    dtChiTietHDB.Columns.Add("MaSP", typeof(string));
                    dtChiTietHDB.Columns.Add("TenSP", typeof(string));
                    dtChiTietHDB.Columns.Add("SoLuong", typeof(decimal));
                    dtChiTietHDB.Columns.Add("DonGia", typeof(decimal)); // Đây sẽ là Đơn giá bán
                    dtChiTietHDB.Columns.Add("KhuyenMai", typeof(string));
                    dtChiTietHDB.Columns.Add("ThanhTien", typeof(decimal));
                }
                if (dgvChiTietHDB.DataSource == null)
                {
                    dgvChiTietHDB.DataSource = dtChiTietHDB;
                }
                // CustomizeDgvChiTietHDB(); // Đã gọi ở trên
            }
        }

        private void LoadComboBoxes()
        {
            Function.FillCombo(cboMaNV, "MaNV", "MaNV", "SELECT MaNV, TenNV FROM dbo.NhanVien ORDER BY TenNV");
            cboMaNV.SelectedIndex = -1;

            // Load Mã Khách Hàng (thay vì Nhà Cung Cấp)
            Function.FillCombo(cboMaKH, "MaKH", "MaKH", "SELECT MaKH, ISNULL(TenKH, DiaChi) AS TenHienThiKH FROM dbo.KhachHang ORDER BY MaKH");
            cboMaKH.SelectedIndex = -1;

            Function.FillCombo(cboMaSP, "MaSP", "MaSP", "SELECT MaSP, TenSP FROM dbo.SanPham ORDER BY TenSP");
            cboMaSP.SelectedIndex = -1;

            // Đổi tên ComboBox tìm kiếm từ cboMaHDNSearch thành cboMaHDBSearch trong Designer
            Function.FillCombo(cboMaHDBSearch, "MaHDB", "MaHDB", "SELECT MaHDB FROM dbo.HoaDonBan WHERE IsDeleted = 0 ORDER BY NgayBan DESC, MaHDB DESC");
            cboMaHDBSearch.SelectedIndex = -1;
        }

        private void CustomizeDgvChiTietHDB() // Đổi tên hàm
        {
            dgvChiTietHDB.AutoGenerateColumns = false;
            dgvChiTietHDB.Columns.Clear();

            dgvChiTietHDB.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaSP", HeaderText = "Mã SP", Name = "MaSPCol", Width = 90 });
            dgvChiTietHDB.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TenSP", HeaderText = "Tên Sản Phẩm", Name = "TenSPCol", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 30 });
            dgvChiTietHDB.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "SoLuong", HeaderText = "Số Lượng", Name = "SoLuongCol", Width = 80, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight } });
            dgvChiTietHDB.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "DonGia", HeaderText = "Đơn Giá Bán", Name = "DonGiaCol", Width = 100, DefaultCellStyle = { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight } }); // HeaderText đổi
            dgvChiTietHDB.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "KhuyenMai", HeaderText = "Khuyến Mãi", Name = "KhuyenMaiCol", Width = 110 });
            dgvChiTietHDB.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ThanhTien", HeaderText = "Thành Tiền", Name = "ThanhTienCol", Width = 120, DefaultCellStyle = { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight } });

            dgvChiTietHDB.AllowUserToAddRows = false;
            dgvChiTietHDB.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgvChiTietHDB.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvChiTietHDB.MultiSelect = false;
        }

        private void SetInitialButtonStatesAndControls()
        {
            SetControlsEnabledState(false);
            btnThemHDB.Enabled = true; // Đổi tên nút
            btnLuuHDB.Enabled = false;
            btnSuaHDB.Enabled = false;
            btnHuyHDB.Enabled = false; // Đổi tên nút (btnHuyHDN -> btnHuyHDB trong Designer)
            btnInHDB.Enabled = false;
            btnDong.Enabled = true;

            btnThemDong.Enabled = false;
            btnSuaDong.Enabled = false;
            btnXoaDong.Enabled = false;
            btnHuyBoDong.Enabled = false;

            cboMaHDBSearch.Enabled = true; // Đổi tên ComboBox
            if (cboMaHDBSearch.Items.Count > 0 && cboMaHDBSearch.DataSource != null) cboMaHDBSearch.SelectedIndex = -1;
        }

        private void ResetChiTietSanPhamFields()
        {
            cboMaSP.SelectedIndex = -1;
            txtTenSP.Text = "";
            txtSoLuong.Text = "0";
            txtDonGia.Text = "0"; // Sẽ là đơn giá bán
            txtKhuyenMai.Text = "";
            txtThanhTienDong.Text = "0";
            if (cboMaSP.Enabled)
                cboMaSP.Focus();
        }

        private void ResetValuesToInitialState()
        {
            txtMaHDB.Text = ""; // Đổi tên
            dpNgayBan.Value = DateTime.Now; // Đổi tên
            cboMaNV.SelectedIndex = -1;
            txtTenNV.Text = "";
            cboMaKH.SelectedIndex = -1; // Đổi từ cboMaNCC
            txtTenKH.Text = "";         // Đổi từ txtTenNCC
            txtSDT.Text = "";
            txtTongTien.Text = "0";
            UpdateTongTienBangChu();

            ResetChiTietSanPhamFields();

            if (dtChiTietHDB != null) // Đổi tên
            {
                dtChiTietHDB.Rows.Clear();
            }

            maHDBToLoad = null; // Đổi tên
            SetInitialButtonStatesAndControls();
        }

        private void SetControlsEnabledState(bool isEnabled)
        {
            dpNgayBan.Enabled = isEnabled; // Đổi tên
            cboMaNV.Enabled = isEnabled;
            cboMaKH.Enabled = isEnabled; // Đổi từ cboMaNCC

            cboMaSP.Enabled = isEnabled;
            txtSoLuong.Enabled = isEnabled;
            txtDonGia.Enabled = isEnabled; // Sẽ là đơn giá bán
            txtKhuyenMai.Enabled = isEnabled;
        }

        private void SetActiveProcessingButtonStates(bool isEditingOldInvoice)
        {
            SetControlsEnabledState(true);
            btnThemHDB.Enabled = true;

            if (isEditingOldInvoice)
            {
                btnLuuHDB.Enabled = false;
                btnSuaHDB.Enabled = true;
                btnHuyHDB.Enabled = true; // Đổi tên nút (btnHuyHDN -> btnHuyHDB)
                btnInHDB.Enabled = true;
            }
            else
            {
                btnLuuHDB.Enabled = true;
                btnSuaHDB.Enabled = false;
                btnHuyHDB.Enabled = true; // Đổi tên nút
                btnInHDB.Enabled = false;
            }

            btnThemDong.Enabled = true;
            btnSuaDong.Enabled = false;
            btnXoaDong.Enabled = false;
            btnHuyBoDong.Enabled = false;

            cboMaHDBSearch.Enabled = true; // Đổi tên ComboBox
        }

        private void LoadThongTinHoaDon(string maHDB) // Đổi tên tham số
        {
            string sql = "SELECT NgayBan, MaNV, MaKH, TongTien FROM dbo.HoaDonBan WHERE MaHDB = @MaHDB AND IsDeleted = 0"; // Đổi bảng và cột
            SqlParameter param = new SqlParameter("@MaHDB", maHDB);
            DataTable dtThongTinChung = Function.GetDataToTable(sql, param);

            if (dtThongTinChung.Rows.Count > 0)
            {
                DataRow row = dtThongTinChung.Rows[0];
                dpNgayBan.Value = Convert.ToDateTime(row["NgayBan"]); // Đổi cột

                if (cboMaNV.DataSource != null) cboMaNV.SelectedValue = row["MaNV"]; else cboMaNV.Text = row["MaNV"].ToString();
                if (cboMaKH.DataSource != null) cboMaKH.SelectedValue = row["MaKH"]; else cboMaKH.Text = row["MaKH"].ToString(); // Đổi cboMaNCC thành cboMaKH

                UpdateTenNVFromMaNV();
                UpdateTenKHFromMaKH(); // Đổi hàm

                txtTongTien.Text = Convert.ToDecimal(row["TongTien"]).ToString("N0", CultureInfo.InvariantCulture);
                UpdateTongTienBangChu();

                LoadChiTietHoaDon(maHDB); // Tham số vẫn là mã hóa đơn
                SetActiveProcessingButtonStates(true);
            }
            else
            {
                MessageBox.Show("Không tìm thấy thông tin hóa đơn bán hoặc hóa đơn đã bị xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ResetValuesToInitialState();
            }
        }

        private void LoadChiTietHoaDon(string maHDB) // Đổi tên tham số
        {
            // Khi load chi tiết HĐ Bán, chúng ta cần lấy GiaBan từ SanPham để hiển thị là DonGia trên form
            string sql = @"SELECT cthdb.MaSP, sp.TenSP, cthdb.SoLuong, sp.GiaBan AS DonGia, cthdb.KhuyenMai, cthdb.ThanhTien
                           FROM dbo.ChiTietHDB cthdb
                           INNER JOIN dbo.SanPham sp ON cthdb.MaSP = sp.MaSP
                           WHERE cthdb.MaHDB = @MaHDB"; // Đổi bảng và JOIN
            SqlParameter param = new SqlParameter("@MaHDB", maHDB);
            DataTable tempDt = Function.GetDataToTable(sql, param);

            dtChiTietHDBGoc = tempDt.Copy(); // Đổi tên
            dtChiTietHDB = tempDt.Copy();    // Đổi tên
            dgvChiTietHDB.DataSource = dtChiTietHDB; // Đổi tên
        }

        private void UpdateTenNVFromMaNV()
        {
            if (cboMaNV.SelectedValue != null && cboMaNV.SelectedIndex != -1)
            {
                string tenNV = Function.GetFieldValue("SELECT TenNV FROM dbo.NhanVien WHERE MaNV = @MaNV", new SqlParameter("@MaNV", cboMaNV.SelectedValue.ToString()));
                txtTenNV.Text = tenNV;
            }
            else
            {
                txtTenNV.Text = "";
            }
        }

        // Hàm mới cho Khách Hàng
        private void UpdateTenKHFromMaKH()
        {
            if (cboMaKH.SelectedValue != null && cboMaKH.SelectedIndex != -1)
            {
                // Lấy TenKH (hoặc DiaChi nếu TenKH là NULL)
                string tenKH = Function.GetFieldValue("SELECT ISNULL(TenKH, DiaChi) FROM dbo.KhachHang WHERE MaKH = @MaKH", new SqlParameter("@MaKH", cboMaKH.SelectedValue.ToString()));
                txtTenKH.Text = tenKH;

                // LẤY VÀ HIỂN THỊ SỐ ĐIỆN THOẠI
                string sdtKH = Function.GetFieldValue("SELECT SDT FROM dbo.KhachHang WHERE MaKH = @MaKH",
                                                   new SqlParameter("@MaKH", cboMaKH.SelectedValue.ToString()));
                txtSDT.Text = sdtKH ?? ""; // Hiển thị SĐT, nếu là NULL thì hiển thị chuỗi rỗng
            }
            else
            {
                txtTenKH.Text = "";
                txtSDT.Text = "";
            }
        }

        private void cboMaKH_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTenKHFromMaKH();
        }

        private void cboMaNV_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTenNVFromMaNV();
        }

        private void cboMaSP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMaSP.SelectedValue != null && cboMaSP.SelectedIndex != -1)
            {
                string maSPChon = cboMaSP.SelectedValue.ToString();
                DataRowView drv = cboMaSP.SelectedItem as DataRowView;
                if (drv != null)
                {
                    txtTenSP.Text = drv["TenSP"].ToString();
                }
                else
                {
                    txtTenSP.Text = Function.GetFieldValue("SELECT TenSP FROM dbo.SanPham WHERE MaSP = @MaSP", new SqlParameter("@MaSP", maSPChon));
                }

                // LẤY ĐƠN GIÁ BÁN (GiaBan) từ bảng SanPham
                string giaBanStr = Function.GetFieldValue("SELECT GiaBan FROM dbo.SanPham WHERE MaSP = @MaSP", new SqlParameter("@MaSP", maSPChon));
                decimal giaBanDecimal;
                if (decimal.TryParse(giaBanStr, NumberStyles.Any, CultureInfo.InvariantCulture, out giaBanDecimal))
                {
                    txtDonGia.Text = giaBanDecimal.ToString(CultureInfo.InvariantCulture); // Hiển thị giá bán
                }
                else
                {
                    txtDonGia.Text = "0";
                }

                txtSoLuong.Text = "1";
                txtKhuyenMai.Text = ""; // Hoặc giá trị mặc định như "0%"
                TinhThanhTienDong();
                txtSoLuong.Focus();
            }
            else
            {
                txtTenSP.Text = "";
                txtDonGia.Text = "0";
                txtSoLuong.Text = "0";
                txtKhuyenMai.Text = "";
                txtThanhTienDong.Text = "0";
            }
        }

        private void UpdateTongTienBangChu()
        {
            if (string.IsNullOrWhiteSpace(txtTongTien.Text))
            {
                lblTongTienBangChu.Text = "Bằng chữ: Không đồng";
                return;
            }
            decimal tongTienSo;
            if (decimal.TryParse(txtTongTien.Text.Replace(",", ""), NumberStyles.Any, CultureInfo.InvariantCulture, out tongTienSo))
            {
                lblTongTienBangChu.Text = "Bằng chữ: " + ChuyenSoSangChuHelper(tongTienSo.ToString("F0"));
            }
            else
            {
                lblTongTienBangChu.Text = "Bằng chữ: (Số không hợp lệ)";
            }
        }

        private string ChuyenSoSangChuHelper(string number)
        {
            if (string.IsNullOrEmpty(number) || number == "0") return "Không đồng";
            long num;
            if (!long.TryParse(number, out num)) return "(Không thể chuyển đổi)";
            if (num == 0) return "Không";
            string[] units = { "", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] thousands = { "", "nghìn", "triệu", "tỷ" };
            if (num < 0) return "âm " + ChuyenSoSangChuHelper(Math.Abs(num).ToString());
            string words = "";
            for (int i = 0; num > 0; i++)
            {
                if (num % 1000 != 0)
                {
                    words = DocNhomBaChuSo((int)(num % 1000)) + " " + thousands[i] + " " + words;
                }
                num /= 1000;
            }
            words = words.Trim();
            return string.IsNullOrEmpty(words) ? "Không" : (char.ToUpper(words[0]) + words.Substring(1) + " đồng");
        }

        private string DocNhomBaChuSo(int num)
        {
            string[] units = { "", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string tram = units[num / 100];
            string chuc = units[(num % 100) / 10];
            string donvi = units[num % 10];
            string result = "";
            if (num == 0) return "";
            if (!string.IsNullOrEmpty(tram)) result += tram + " trăm ";
            if ((num % 100) > 0)
            {
                if (string.IsNullOrEmpty(tram) && string.IsNullOrEmpty(chuc) && (num % 100 < 10)) { }
                else if (string.IsNullOrEmpty(tram) && (num % 100 < 10))
                {
                    if (result != "") result += "không trăm linh ";
                    else if (num < 10 && (num % 1000 > 0)) result += "linh ";
                }
                else if (string.IsNullOrEmpty(chuc) && !string.IsNullOrEmpty(donvi))
                {
                    if (!string.IsNullOrEmpty(tram)) result += "linh ";
                }
                if ((num % 100) < 10) { }
                else if ((num % 100) < 20)
                {
                    result += ((num % 100) == 15) ? "mười lăm " : ("mười " + ((donvi == "một") ? "mốt " : donvi + " "));
                }
                else
                {
                    result += chuc + " mươi ";
                    if (donvi == "một" && chuc != "mười") result += "mốt ";
                    else if (donvi == "bốn" && chuc != "mười") result += "tư ";
                    else if (donvi == "năm" && chuc != "mười" && !string.IsNullOrEmpty(chuc)) result += "lăm ";
                    else if (!string.IsNullOrEmpty(donvi)) result += donvi + " ";
                }
                if ((num % 100) > 0 && (num % 100) < 10 && string.IsNullOrEmpty(chuc) && string.IsNullOrEmpty(tram)) { }
                else if ((num % 100) > 0 && (num % 100) < 10 && string.IsNullOrEmpty(chuc) && !string.IsNullOrEmpty(tram))
                { result += donvi + " "; }
                else if ((num % 100) > 0 && (num % 100) < 10 && !string.IsNullOrEmpty(chuc)) { }
                if (string.IsNullOrEmpty(donvi) && (num % 100) >= 10 && !string.IsNullOrEmpty(chuc)) { }
                else if ((num % 100) > 0 && (num % 100) < 10 && string.IsNullOrEmpty(chuc))
                { result += donvi + " "; }
            }
            return result.Trim().Replace("  ", " ");
        }

        private void TinhThanhTienDong()
        {
            decimal soLuong = 0;
            decimal donGia = 0; // Sẽ là đơn giá bán
            decimal khuyenMaiPercent = 0;
            decimal thanhTienDong = 0;

            decimal.TryParse(txtSoLuong.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out soLuong);
            decimal.TryParse(txtDonGia.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out donGia); // Đơn giá bán từ txtDonGia
            decimal.TryParse(txtKhuyenMai.Text.Replace("%", ""), NumberStyles.Any, CultureInfo.InvariantCulture, out khuyenMaiPercent);

            if (soLuong < 0) soLuong = 0;
            if (donGia < 0) donGia = 0;
            if (khuyenMaiPercent < 0) khuyenMaiPercent = 0;
            if (khuyenMaiPercent > 100) khuyenMaiPercent = 100;

            thanhTienDong = (soLuong * donGia) * (1 - (khuyenMaiPercent / 100));
            txtThanhTienDong.Text = thanhTienDong.ToString("N0", CultureInfo.InvariantCulture);
        }

        private void txtChiTietSP_TextChanged(object sender, EventArgs e)
        {
            TinhThanhTienDong();
        }

        public void CapNhatTongTienHoaDon()
        {
            decimal tongTienHoaDon = 0;
            if (dtChiTietHDB != null) // Đổi tên
            {
                foreach (DataRow row in dtChiTietHDB.Rows)
                {
                    if (row.RowState != DataRowState.Deleted && row["ThanhTien"] != DBNull.Value)
                    {
                        tongTienHoaDon += Convert.ToDecimal(row["ThanhTien"]);
                    }
                }
            }
            txtTongTien.Text = tongTienHoaDon.ToString("N0", CultureInfo.InvariantCulture);
            UpdateTongTienBangChu();
        }

        private void dgvChiTietHDB_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvChiTietHDB.SelectedRows.Count > 0 && dgvChiTietHDB.CurrentRow != null && dgvChiTietHDB.CurrentRow.DataBoundItem != null)
            {
                DataGridViewRow selectedRow = dgvChiTietHDB.CurrentRow;
                string maSPCurrentInRow = selectedRow.Cells["MaSPCol"].Value?.ToString(); // Sử dụng Name của cột

                if (!string.IsNullOrEmpty(maSPCurrentInRow))
                {
                    cboMaSP.SelectedValue = maSPCurrentInRow;
                    txtTenSP.Text = selectedRow.Cells["TenSPCol"].Value?.ToString();
                    decimal donGiaDecimal;
                    if (decimal.TryParse(selectedRow.Cells["DonGiaCol"].Value?.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out donGiaDecimal))
                    { txtDonGia.Text = donGiaDecimal.ToString(CultureInfo.InvariantCulture); }
                    else { txtDonGia.Text = "0"; }
                }
                else
                {
                    cboMaSP.SelectedIndex = -1;
                    txtTenSP.Text = "";
                    txtDonGia.Text = "0";
                }
                txtSoLuong.Text = selectedRow.Cells["SoLuongCol"].Value?.ToString() ?? "0";
                txtKhuyenMai.Text = selectedRow.Cells["KhuyenMaiCol"].Value?.ToString() ?? "";
                decimal thanhTienDecimal;
                if (decimal.TryParse(selectedRow.Cells["ThanhTienCol"].Value?.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out thanhTienDecimal))
                { txtThanhTienDong.Text = thanhTienDecimal.ToString("N0", CultureInfo.InvariantCulture); }
                else { txtThanhTienDong.Text = "0"; }

                // Kiểm tra nút Lưu HĐB (btnLuuHDB) thay vì btnLuuHDN
                if (btnLuuHDB.Enabled || btnSuaHDB.Enabled) // Nếu đang trong quá trình tạo mới hoặc sửa
                {
                    btnThemDong.Enabled = false;
                    btnSuaDong.Enabled = true;
                    btnXoaDong.Enabled = true;
                    btnHuyBoDong.Enabled = true; // Nếu có nút này
                    cboMaSP.Enabled = true;
                }
            }
            else
            {
                if (btnLuuHDB.Enabled || btnSuaHDB.Enabled)
                {
                    btnThemDong.Enabled = true;
                    btnSuaDong.Enabled = false;
                    btnXoaDong.Enabled = false;
                    btnHuyBoDong.Enabled = false; // Nếu có
                }
                // cboMaSP.Enabled nên được set trong SetControlsEnabledState hoặc SetActiveProcessingButtonStates
            }
        }

        private void dgvChiTietHDB_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvChiTietHDB_SelectionChanged(sender, e);
        }

        public static string GenerateMaHDB() // Đổi tên hàm
        {
            return "HDB_" + DateTime.Now.ToString("ddMMyyyy_HHmmss"); // Đổi tiền tố
        }

        private void btnThemHDB_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtMaHDB.Text) && string.IsNullOrEmpty(maHDBToLoad) && dtChiTietHDB.Rows.Count > 0)
            {
                DialogResult confirm = MessageBox.Show("Bạn có hóa đơn bán đang làm dở. Bạn có muốn hủy và tạo mới không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.No) return;
            }
            else if (!string.IsNullOrEmpty(maHDBToLoad))
            {
                DialogResult confirm = MessageBox.Show("Bạn đang xem/sửa hóa đơn bán cũ. Tạo hóa đơn mới?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.No) return;
            }

            ResetValuesToInitialState();
            txtMaHDB.Text = GenerateMaHDB(); // Gọi hàm tạo mã HĐB
            SetActiveProcessingButtonStates(false); // isEditingOldInvoice = false
            dpNgayBan.Focus(); // Đổi dpNgayNhap
        }

        private void btnThemDong_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaHDB.Text)) // Đổi txtMaHDN
            {
                MessageBox.Show("Vui lòng tạo hoặc chọn hóa đơn bán.", "Chưa có Hóa Đơn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnThemHDB.Focus(); // Đổi btnThemHDN
                return;
            }
            if (cboMaSP.SelectedValue == null || cboMaSP.SelectedIndex == -1)
            {
                MessageBox.Show("Bạn phải chọn sản phẩm.", "Chưa chọn sản phẩm", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboMaSP.Focus();
                return;
            }
            decimal soLuong;
            if (!decimal.TryParse(txtSoLuong.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out soLuong) || soLuong <= 0)
            {
                MessageBox.Show("Số lượng phải là số dương.", "Số lượng không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSoLuong.Focus(); txtSoLuong.SelectAll(); return;
            }
            decimal donGia; // Sẽ là đơn giá bán
            if (!decimal.TryParse(txtDonGia.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out donGia) || donGia < 0)
            {
                MessageBox.Show("Đơn giá bán phải là số không âm.", "Đơn giá không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtDonGia.Focus(); txtDonGia.SelectAll(); return;
            }
            // Kiểm tra số lượng tồn kho trước khi thêm vào hóa đơn bán
            decimal soLuongTonKho = 0;
            string soLuongTonStr = Function.GetFieldValue("SELECT SoLuong FROM dbo.SanPham WHERE MaSP = @MaSP", new SqlParameter("@MaSP", cboMaSP.SelectedValue.ToString()));
            decimal.TryParse(soLuongTonStr, out soLuongTonKho);

            if (soLuong > soLuongTonKho)
            {
                MessageBox.Show($"Số lượng bán ({soLuong}) vượt quá số lượng tồn kho ({soLuongTonKho}) của sản phẩm '{txtTenSP.Text}'.", "Không đủ hàng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuong.Focus(); txtSoLuong.SelectAll(); return;
            }


            decimal thanhTienDong;
            if (!decimal.TryParse(txtThanhTienDong.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out thanhTienDong))
            {
                MessageBox.Show("Thành tiền dòng không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string maSPDaChon = cboMaSP.SelectedValue.ToString();
            string tenSPDaChon = txtTenSP.Text;
            string khuyenMai = txtKhuyenMai.Text.Trim();

            if (dtChiTietHDB != null) // Đổi tên dtChiTietHDN
            {
                foreach (DataRow existingRow in dtChiTietHDB.Rows)
                {
                    if (existingRow.RowState != DataRowState.Deleted && existingRow["MaSP"].ToString().Equals(maSPDaChon, StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show($"Sản phẩm '{tenSPDaChon}' đã có trong hóa đơn.", "Sản phẩm đã tồn tại", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }

            if (dtChiTietHDB == null) return; // Nên được khởi tạo ở Load
            try
            {
                DataRow newRow = dtChiTietHDB.NewRow();
                newRow["MaSP"] = maSPDaChon;
                newRow["TenSP"] = tenSPDaChon;
                newRow["SoLuong"] = soLuong;
                newRow["DonGia"] = donGia; // Đơn giá bán
                newRow["KhuyenMai"] = khuyenMai;
                newRow["ThanhTien"] = thanhTienDong;
                dtChiTietHDB.Rows.Add(newRow);
            }
            catch (Exception ex) { MessageBox.Show("Lỗi thêm dòng: " + ex.Message); return; }

            CapNhatTongTienHoaDon();
            ResetChiTietSanPhamFields();
            dgvChiTietHDB.ClearSelection(); // Đổi tên
            cboMaSP.Focus();
        }

        private void btnLuuHDB_Click(object sender, EventArgs e)
        {
            string maHDBCCanIn = txtMaHDB.Text.Trim();
            if (string.IsNullOrWhiteSpace(maHDBCCanIn) || string.IsNullOrEmpty(maHDBToLoad) || !maHDBCCanIn.Equals(maHDBToLoad, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Vui lòng tải hóa đơn bán đã lưu để in.", "Chưa có hóa đơn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (dtChiTietHDB == null || dtChiTietHDB.Rows.Count == 0)
            {
                MessageBox.Show("Hóa đơn không có chi tiết.", "Không có dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string ngayBanHienThi = dpNgayBan.Text;
            string tenNVHienThi = txtTenNV.Text;
            string tenKHHienThi = txtTenKH.Text;
            string diaChiKH = ""; // Sẽ lấy từ CSDL nếu cần thiết kế chi tiết hơn
            string sdtKHHienThi = txtSDT.Text; // Lấy SĐT từ TextBox đã có trên form

            // Nếu bạn muốn đảm bảo lấy SĐT mới nhất từ CSDL thay vì từ TextBox (phòng trường hợp TextBox chưa cập nhật)
            if (cboMaKH.SelectedValue != null)
            {
                DataTable dtKHInfo = Function.GetDataToTable("SELECT TenKH, DiaChi, SDT FROM dbo.KhachHang WHERE MaKH = @MaKH",
                                                             new SqlParameter("@MaKH", cboMaKH.SelectedValue.ToString()));
                if (dtKHInfo.Rows.Count > 0)
                {
                    tenKHHienThi = dtKHInfo.Rows[0]["TenKH"]?.ToString() ?? tenKHHienThi;
                    diaChiKH = dtKHInfo.Rows[0]["DiaChi"]?.ToString();
                    sdtKHHienThi = dtKHInfo.Rows[0]["SDT"]?.ToString() ?? sdtKHHienThi; // Ưu tiên SĐT từ CSDL
                }
            }

            string tongTienHienThi = txtTongTien.Text;
            string tongTienBangChuHienThi = lblTongTienBangChu.Text;

            try
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("HoaDonBan_" + maHDBCCanIn);
                    int currentRow = 1;

                    // Tiêu đề Hóa Đơn Bán
                    worksheet.Cell(currentRow, 3).Value = "HÓA ĐƠN BÁN HÀNG";
                    var titleRange = worksheet.Range(currentRow, 3, currentRow, 5);
                    titleRange.Merge().Style.Font.SetBold().Font.SetFontSize(16).Font.SetFontColor(XLColor.DarkBlue);
                    titleRange.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    currentRow += 2;

                    // Thông tin chung HĐ Bán
                    worksheet.Cell(currentRow, 1).Value = "Mã hóa đơn:"; worksheet.Cell(currentRow, 1).Style.Font.SetBold();
                    worksheet.Cell(currentRow, 2).Value = maHDBCCanIn; worksheet.Range(currentRow, 2, currentRow, 3).Merge();
                    worksheet.Cell(currentRow, 5).Value = "Ngày bán:"; worksheet.Cell(currentRow, 5).Style.Font.SetBold();
                    worksheet.Cell(currentRow, 6).Value = ngayBanHienThi;
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = "Nhân viên:"; worksheet.Cell(currentRow, 1).Style.Font.SetBold();
                    worksheet.Cell(currentRow, 2).Value = tenNVHienThi; worksheet.Range(currentRow, 2, currentRow, 3).Merge();
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = "Khách hàng:"; worksheet.Cell(currentRow, 1).Style.Font.SetBold();
                    worksheet.Cell(currentRow, 2).Value = tenKHHienThi; worksheet.Range(currentRow, 2, currentRow, 6).Merge();
                    currentRow++;

                    // THÊM SỐ ĐIỆN THOẠI KHÁCH HÀNG VÀO EXCEL
                    if (!string.IsNullOrEmpty(sdtKHHienThi))
                    {
                        worksheet.Cell(currentRow, 1).Value = "SĐT Khách:"; worksheet.Cell(currentRow, 1).Style.Font.SetBold();
                        worksheet.Cell(currentRow, 2).Value = sdtKHHienThi;
                        worksheet.Range(currentRow, 2, currentRow, 3).Merge(); // Merge nếu cần
                                                                               // Nếu muốn SĐT ở cột khác hoặc không merge, điều chỉnh ở đây
                                                                               // Ví dụ: worksheet.Cell(currentRow, 4).Value = "SĐT Khách:"; worksheet.Cell(currentRow, 4).Style.Font.SetBold();
                                                                               //        worksheet.Cell(currentRow, 5).Value = sdtKHHienThi;
                        currentRow++;
                    }

                    if (!string.IsNullOrEmpty(diaChiKH))
                    {
                        worksheet.Cell(currentRow, 1).Value = "Địa chỉ KH:"; worksheet.Cell(currentRow, 1).Style.Font.SetBold();
                        worksheet.Cell(currentRow, 2).Value = diaChiKH; worksheet.Range(currentRow, 2, currentRow, 6).Merge();
                        currentRow++;
                    }
                    currentRow++; // Dòng trống sau thông tin khách hàng

                    // Bảng chi tiết (giữ nguyên như trước)
                    int headerDetailRow = currentRow;
                    worksheet.Cell(headerDetailRow, 1).Value = "STT";
                    worksheet.Cell(headerDetailRow, 2).Value = "Tên Sản Phẩm";
                    worksheet.Cell(headerDetailRow, 3).Value = "Số Lượng";
                    worksheet.Cell(headerDetailRow, 4).Value = "Đơn Giá Bán";
                    worksheet.Cell(headerDetailRow, 5).Value = "Khuyến Mãi";
                    worksheet.Cell(headerDetailRow, 6).Value = "Thành Tiền";
                    var headerDetailRange = worksheet.Range(headerDetailRow, 1, headerDetailRow, 6);
                    headerDetailRange.Style.Font.SetBold().Fill.SetBackgroundColor(XLColor.LightSkyBlue);
                    headerDetailRange.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    currentRow++;

                    int stt = 0;
                    foreach (DataRow dr in dtChiTietHDB.Rows)
                    {
                        if (dr.RowState == DataRowState.Deleted) continue;
                        stt++;
                        worksheet.Cell(currentRow, 1).Value = stt;
                        worksheet.Cell(currentRow, 2).Value = dr["TenSP"]?.ToString();
                        worksheet.Cell(currentRow, 3).Value = Convert.ToDecimal(dr["SoLuong"]); worksheet.Cell(currentRow, 3).Style.NumberFormat.Format = "#,##0";
                        worksheet.Cell(currentRow, 4).Value = Convert.ToDecimal(dr["DonGia"]); worksheet.Cell(currentRow, 4).Style.NumberFormat.Format = "#,##0";
                        worksheet.Cell(currentRow, 5).Value = dr["KhuyenMai"]?.ToString();
                        worksheet.Cell(currentRow, 6).Value = Convert.ToDecimal(dr["ThanhTien"]); worksheet.Cell(currentRow, 6).Style.NumberFormat.Format = "#,##0";
                        currentRow++;
                    }
                    currentRow++;

                    // Tổng tiền (giữ nguyên)
                    worksheet.Cell(currentRow, 5).Value = "Tổng cộng:"; worksheet.Cell(currentRow, 5).Style.Font.SetBold().Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    worksheet.Cell(currentRow, 6).Value = Convert.ToDecimal(tongTienHienThi.Replace(",", "")); worksheet.Cell(currentRow, 6).Style.Font.SetBold().NumberFormat.Format = "#,##0";
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = tongTienBangChuHienThi; worksheet.Range(currentRow, 1, currentRow, 6).Merge().Style.Font.SetBold().Font.SetItalic();
                    currentRow += 2;

                    // Ký tên (giữ nguyên)
                    worksheet.Cell(currentRow, 2).Value = "Người Lập Phiếu"; worksheet.Cell(currentRow, 2).Style.Font.SetBold().Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Range(currentRow, 1, currentRow, 2).Merge();
                    worksheet.Cell(currentRow, 5).Value = "Khách Hàng"; worksheet.Cell(currentRow, 5).Style.Font.SetBold().Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Range(currentRow, 4, currentRow, 6).Merge();
                    currentRow += 3; // Tăng thêm dòng cho chữ ký
                    worksheet.Cell(currentRow, 2).Value = tenNVHienThi; worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Range(currentRow, 1, currentRow, 2).Merge();
                    worksheet.Cell(currentRow, 5).Value = tenKHHienThi; worksheet.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Range(currentRow, 4, currentRow, 6).Merge();

                    worksheet.Columns().AdjustToContents();
                    worksheet.Column(2).Width = Math.Max(worksheet.Column(2).Width, 30);
                    // Điều chỉnh lại dòng cuối của bảng chi tiết để kẻ khung cho đúng
                    var detailTableRange = worksheet.Range(headerDetailRow, 1, currentRow - 4, 6); // -4 là để trừ đi 3 dòng ký tên và 1 dòng trống trước đó
                    detailTableRange.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Border.SetInsideBorder(XLBorderStyleValues.Thin);

                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        Filter = "Excel Files|*.xlsx",
                        Title = "Lưu Hóa Đơn Bán",
                        FileName = "HDB_" + maHDBCCanIn.Replace("/", "_").Replace(":", "") + ".xlsx"
                    };
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        workbook.SaveAs(saveFileDialog.FileName);
                        MessageBox.Show($"Đã xuất hóa đơn bán '{maHDBCCanIn}' thành công!\nĐường dẫn: {saveFileDialog.FileName}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (MessageBox.Show("Mở file vừa xuất?", "Mở file", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            try { System.Diagnostics.Process.Start(saveFileDialog.FileName); }
                            catch (Exception exOpen) { MessageBox.Show("Lỗi mở file: " + exOpen.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất Excel: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSuaDong_Click(object sender, EventArgs e)
        {
            if (dgvChiTietHDB.CurrentRow == null || dgvChiTietHDB.CurrentRow.Index == -1 || dgvChiTietHDB.CurrentRow.DataBoundItem == null) // Đổi tên dgv
            {
                MessageBox.Show("Chọn dòng để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int rowIndexToEdit = dgvChiTietHDB.CurrentRow.Index;
            string maSPGocCuaDong = dtChiTietHDB.Rows[rowIndexToEdit]["MaSP"].ToString(); // Đổi tên dt

            if (cboMaSP.SelectedValue == null || cboMaSP.SelectedIndex == -1)
            {
                MessageBox.Show("Chọn sản phẩm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cboMaSP.Focus(); return;
            }
            string maSPMoi = cboMaSP.SelectedValue.ToString();
            string tenSPMoi = txtTenSP.Text;
            decimal soLuongMoi;
            if (!decimal.TryParse(txtSoLuong.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out soLuongMoi) || soLuongMoi <= 0) { /*...*/ return; }

            // Kiểm tra số lượng tồn kho khi sửa (nếu số lượng tăng)
            decimal soLuongCu = Convert.ToDecimal(dtChiTietHDB.Rows[rowIndexToEdit]["SoLuong"]);
            decimal chenhLechSoLuong = soLuongMoi - soLuongCu;

            if (chenhLechSoLuong > 0) // Nếu số lượng mới lớn hơn số lượng cũ (tức là tăng số lượng bán)
            {
                decimal soLuongTonKho = 0;
                string soLuongTonStr = Function.GetFieldValue("SELECT SoLuong FROM dbo.SanPham WHERE MaSP = @MaSP", new SqlParameter("@MaSP", maSPMoi));
                decimal.TryParse(soLuongTonStr, out soLuongTonKho);
                // Số lượng tồn kho hiện tại (trước khi áp dụng thay đổi này) đã bao gồm việc trừ đi soLuongCu
                // Vậy, số lượng có thể bán thêm là soLuongTonKho (hiện tại)
                if (chenhLechSoLuong > soLuongTonKho)
                {
                    MessageBox.Show($"Không đủ hàng để tăng số lượng. Tồn kho hiện tại (sau khi trừ SL cũ của dòng này): {soLuongTonKho}. Bạn muốn tăng thêm: {chenhLechSoLuong}", "Không đủ hàng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSoLuong.Focus(); txtSoLuong.SelectAll(); return;
                }
            }


            decimal donGiaMoi; // Đơn giá bán
            if (!decimal.TryParse(txtDonGia.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out donGiaMoi) || donGiaMoi < 0) { /*...*/ return; }
            string khuyenMaiMoi = txtKhuyenMai.Text.Trim();
            decimal thanhTienDongMoi;
            if (!decimal.TryParse(txtThanhTienDong.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out thanhTienDongMoi)) { /*...*/ return; }

            if (!maSPMoi.Equals(maSPGocCuaDong, StringComparison.OrdinalIgnoreCase))
            {
                // Logic kiểm tra trùng nếu đổi mã SP (giữ nguyên từ HĐ Nhập)
                if (dtChiTietHDB != null)
                {
                    for (int i = 0; i < dtChiTietHDB.Rows.Count; i++)
                    {
                        if (i == rowIndexToEdit) continue;
                        DataRow dr = dtChiTietHDB.Rows[i];
                        if (dr.RowState != DataRowState.Deleted && dr["MaSP"].ToString().Equals(maSPMoi, StringComparison.OrdinalIgnoreCase))
                        {
                            MessageBox.Show($"Sản phẩm '{tenSPMoi}' đã có trong hóa đơn.", "Sản phẩm trùng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            cboMaSP.Focus(); return;
                        }
                    }
                }
            }

            try
            {
                DataRow rowToEdit = dtChiTietHDB.Rows[rowIndexToEdit]; // Đổi tên dt
                rowToEdit.BeginEdit();
                rowToEdit["MaSP"] = maSPMoi;
                rowToEdit["TenSP"] = tenSPMoi;
                rowToEdit["SoLuong"] = soLuongMoi;
                rowToEdit["DonGia"] = donGiaMoi; // Đơn giá bán
                rowToEdit["KhuyenMai"] = khuyenMaiMoi;
                rowToEdit["ThanhTien"] = thanhTienDongMoi;
                rowToEdit.EndEdit();
            }
            catch (Exception ex) { MessageBox.Show("Lỗi cập nhật dòng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            CapNhatTongTienHoaDon();
            ResetChiTietSanPhamFields();
            dgvChiTietHDB.ClearSelection(); // Đổi tên dgv
            MessageBox.Show("Cập nhật dòng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnXoaDong_Click(object sender, EventArgs e)
        {
            if (dgvChiTietHDB.CurrentRow == null || dgvChiTietHDB.CurrentRow.Index == -1 || dgvChiTietHDB.CurrentRow.DataBoundItem == null) // Đổi tên dgv
            {
                MessageBox.Show("Chọn dòng để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int rowIndexToDelete = dgvChiTietHDB.CurrentRow.Index;
            string tenSPToDelete = dgvChiTietHDB.CurrentRow.Cells["TenSPCol"].Value?.ToString(); // Sử dụng Name cột

            DialogResult confirmResult = MessageBox.Show($"Xóa sản phẩm '{tenSPToDelete}' khỏi hóa đơn?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                if (dtChiTietHDB != null && rowIndexToDelete >= 0 && rowIndexToDelete < dtChiTietHDB.Rows.Count) // Đổi tên dt
                {
                    try
                    {
                        DataRowView drv = dgvChiTietHDB.CurrentRow.DataBoundItem as DataRowView;
                        if (drv != null) drv.Row.Delete();
                        else if (dgvChiTietHDB.CurrentRow.DataBoundItem is DataRow) ((DataRow)dgvChiTietHDB.CurrentRow.DataBoundItem).Delete();
                        else dtChiTietHDB.Rows[rowIndexToDelete].Delete();
                    }
                    catch (Exception ex) { MessageBox.Show("Lỗi xóa dòng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                    CapNhatTongTienHoaDon();
                    ResetChiTietSanPhamFields();
                    dgvChiTietHDB.ClearSelection(); // Đổi tên dgv
                    MessageBox.Show($"Đã xóa '{tenSPToDelete}'.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboMaSP.Focus();
                }
            }
        }

        private void btnSuaHDB_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(maHDBToLoad) || !txtMaHDB.Text.Equals(maHDBToLoad, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Không có hóa đơn bán hợp lệ để sửa.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string maHDBCCanSua = maHDBToLoad; // Đổi tên biến

            if (cboMaNV.SelectedValue == null || cboMaKH.SelectedValue == null) // Đổi cboMaNCC
            {
                MessageBox.Show("Chọn Nhân viên và Khách hàng.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string maNVMoi = cboMaNV.SelectedValue.ToString();
            string maKHMoi = cboMaKH.SelectedValue.ToString(); // Đổi biến
            DateTime ngayBanMoi = dpNgayBan.Value; // Đổi biến

            if (dtChiTietHDBGoc == null) // Đổi tên dt
            {
                MessageBox.Show("Lỗi dữ liệu gốc. Tải lại hóa đơn.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult confirmUpdate = MessageBox.Show($"Cập nhật hóa đơn '{maHDBCCanSua}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmUpdate == DialogResult.No) return;

            CapNhatTongTienHoaDon();
            decimal tongTienMoi;
            if (!decimal.TryParse(txtTongTien.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out tongTienMoi))
            {
                MessageBox.Show("Tổng tiền không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                Function.ExecuteTransaction((conn, trans) =>
                {
                    // Cập nhật HoaDonBan
                    string sqlUpdateHDB = @"UPDATE dbo.HoaDonBan SET NgayBan = @NgayBan, MaNV = @MaNV, MaKH = @MaKH, TongTien = @TongTien WHERE MaHDB = @MaHDB"; // Đổi bảng và cột
                    SqlParameter[] paramsUpdateHDB =
                    {
                        new SqlParameter("@NgayBan", ngayBanMoi.Date),
                        new SqlParameter("@MaNV", maNVMoi),
                        new SqlParameter("@MaKH", maKHMoi), // Đổi MaNCC
                        new SqlParameter("@TongTien", tongTienMoi),
                        new SqlParameter("@MaHDB", maHDBCCanSua)
                    };
                    Function.ExecuteNonQuery(sqlUpdateHDB, conn, trans, paramsUpdateHDB);

                    // Xử lý ChiTietHDB và SanPham (logic phức tạp hơn)
                    // 1. Các dòng bị xóa khỏi chi tiết (có trong dtChiTietHDBGoc, không có trong dtChiTietHDB)
                    List<DataRow> rowsToDeleteFromDB = new List<DataRow>();
                    foreach (DataRow rowGoc in dtChiTietHDBGoc.Rows) // Đổi tên dt
                    {
                        string maSPGoc = rowGoc["MaSP"].ToString();
                        DataRow[] foundRowsMoi = dtChiTietHDB.Select($"MaSP = '{maSPGoc.Replace("'", "''")}'"); // Đổi tên dt
                        if (foundRowsMoi.Length == 0) rowsToDeleteFromDB.Add(rowGoc);
                    }
                    foreach (DataRow rowBiXoa in rowsToDeleteFromDB)
                    {
                        string maSPBiXoa = rowBiXoa["MaSP"].ToString();
                        decimal soLuongGocCuaDongBiXoa = Convert.ToDecimal(rowBiXoa["SoLuong"]);
                        // Xóa khỏi ChiTietHDB
                        string sqlDeleteChiTiet = "DELETE FROM dbo.ChiTietHDB WHERE MaHDB = @MaHDB AND MaSP = @MaSP"; // Đổi bảng
                        Function.ExecuteNonQuery(sqlDeleteChiTiet, conn, trans, new SqlParameter("@MaHDB", maHDBCCanSua), new SqlParameter("@MaSP", maSPBiXoa));
                        // CỘNG TRẢ LẠI KHO
                        string sqlUpdateKhoCong = "UPDATE dbo.SanPham SET SoLuong = ISNULL(SoLuong, 0) + @SoLuongTraLai WHERE MaSP = @MaSPKho"; // Cộng trả
                        Function.ExecuteNonQuery(sqlUpdateKhoCong, conn, trans, new SqlParameter("@SoLuongTraLai", soLuongGocCuaDongBiXoa), new SqlParameter("@MaSPKho", maSPBiXoa));
                    }

                    // 2. Các dòng mới thêm hoặc sửa đổi
                    foreach (DataRow rowMoiTrenForm in dtChiTietHDB.Rows) // Đổi tên dt
                    {
                        if (rowMoiTrenForm.RowState == DataRowState.Deleted) continue;
                        string maSPMoi = rowMoiTrenForm["MaSP"].ToString();
                        decimal soLuongMoi = Convert.ToDecimal(rowMoiTrenForm["SoLuong"]);
                        // DonGia không lưu vào ChiTietHDB theo schema, ThanhTien đã có
                        string khuyenMaiMoi = rowMoiTrenForm["KhuyenMai"]?.ToString();
                        decimal thanhTienMoi = Convert.ToDecimal(rowMoiTrenForm["ThanhTien"]);

                        DataRow[] foundRowsGoc = dtChiTietHDBGoc.Select($"MaSP = '{maSPMoi.Replace("'", "''")}'"); // Đổi tên dt
                        if (foundRowsGoc.Length == 0) // Mới thêm
                        {
                            // Thêm vào ChiTietHDB
                            string sqlInsertChiTiet = @"INSERT INTO dbo.ChiTietHDB (MaHDB, MaSP, SoLuong, KhuyenMai, ThanhTien) VALUES (@MaHDB, @MaSP, @SoLuong, @KhuyenMai, @ThanhTien)"; // Đổi bảng
                            SqlParameter[] paramsInsertCT = {
                                new SqlParameter("@MaHDB", maHDBCCanSua), new SqlParameter("@MaSP", maSPMoi),
                                new SqlParameter("@SoLuong", soLuongMoi),
                                new SqlParameter("@KhuyenMai", string.IsNullOrEmpty(khuyenMaiMoi) ? (object)DBNull.Value : khuyenMaiMoi),
                                new SqlParameter("@ThanhTien", thanhTienMoi)
                            };
                            Function.ExecuteNonQuery(sqlInsertChiTiet, conn, trans, paramsInsertCT);
                            // TRỪ KHO
                            string sqlUpdateKhoTru = "UPDATE dbo.SanPham SET SoLuong = ISNULL(SoLuong, 0) - @SoLuongMoiBan WHERE MaSP = @MaSPKho"; // Trừ đi
                            Function.ExecuteNonQuery(sqlUpdateKhoTru, conn, trans, new SqlParameter("@SoLuongMoiBan", soLuongMoi), new SqlParameter("@MaSPKho", maSPMoi));
                        }
                        else // Đã có, kiểm tra sửa đổi
                        {
                            DataRow rowGocDeSoSanh = foundRowsGoc[0];
                            decimal soLuongGoc = Convert.ToDecimal(rowGocDeSoSanh["SoLuong"]);
                            // DonGia không so sánh vì không lưu trong ChiTietHDB
                            string khuyenMaiGoc = rowGocDeSoSanh["KhuyenMai"]?.ToString() ?? "";
                            // ThanhTien sẽ khác nếu SoLuong hoặc KhuyenMai khác

                            bool coThayDoiChiTiet = (soLuongMoi != soLuongGoc || (khuyenMaiMoi ?? "") != khuyenMaiGoc);
                            if (coThayDoiChiTiet)
                            {
                                // Cập nhật ChiTietHDB
                                string sqlUpdateChiTiet = @"UPDATE dbo.ChiTietHDB SET SoLuong = @SoLuong, KhuyenMai = @KhuyenMai, ThanhTien = @ThanhTien WHERE MaHDB = @MaHDB AND MaSP = @MaSP"; // Đổi bảng
                                SqlParameter[] paramsUpdateCT = {
                                    new SqlParameter("@SoLuong", soLuongMoi),
                                    new SqlParameter("@KhuyenMai", string.IsNullOrEmpty(khuyenMaiMoi) ? (object)DBNull.Value : khuyenMaiMoi),
                                    new SqlParameter("@ThanhTien", thanhTienMoi),
                                    new SqlParameter("@MaHDB", maHDBCCanSua), new SqlParameter("@MaSP", maSPMoi)
                                };
                                Function.ExecuteNonQuery(sqlUpdateChiTiet, conn, trans, paramsUpdateCT);
                                // Cập nhật kho (chênh lệch)
                                decimal soLuongChenhLech = soLuongMoi - soLuongGoc; // Nếu dương: bán thêm, âm: trả lại
                                if (soLuongChenhLech != 0)
                                {
                                    string sqlUpdateKhoChenhLech = "UPDATE dbo.SanPham SET SoLuong = ISNULL(SoLuong, 0) - @SoLuongChenhLech WHERE MaSP = @MaSPKho"; // Trừ đi chênh lệch
                                    Function.ExecuteNonQuery(sqlUpdateKhoChenhLech, conn, trans, new SqlParameter("@SoLuongChenhLech", soLuongChenhLech), new SqlParameter("@MaSPKho", maSPMoi));
                                }
                            }
                        }
                    }
                });

                MessageBox.Show($"Cập nhật hóa đơn bán '{maHDBCCanSua}' thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                string maHDVVuaCapNhat = maHDBToLoad;
                ResetValuesToInitialState();
                maHDBToLoad = maHDVVuaCapNhat;
                txtMaHDB.Text = maHDBToLoad;
                LoadThongTinHoaDon(maHDBToLoad);

                int selectedIndexSearch = cboMaHDBSearch.SelectedIndex; // Đổi tên ComboBox
                Function.FillCombo(cboMaHDBSearch, "MaHDB", "MaHDB", "SELECT MaHDB FROM dbo.HoaDonBan WHERE IsDeleted = 0 ORDER BY NgayBan DESC, MaHDB DESC"); // Đổi bảng
                if (selectedIndexSearch != -1 && cboMaHDBSearch.Items.Count > selectedIndexSearch) cboMaHDBSearch.SelectedIndex = selectedIndexSearch;
                else cboMaHDBSearch.SelectedValue = maHDBToLoad;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật hóa đơn bán:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHuyHDB_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(maHDBToLoad) && !string.IsNullOrWhiteSpace(txtMaHDB.Text))
            {
                DialogResult confirmCancel = MessageBox.Show("Hủy tạo hóa đơn bán này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmCancel == DialogResult.Yes)
                {
                    ResetValuesToInitialState();
                    MessageBox.Show("Đã hủy tạo hóa đơn bán mới.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (!string.IsNullOrEmpty(maHDBToLoad))
            {
                DialogResult confirmDelete = MessageBox.Show($"XÓA hóa đơn bán '{maHDBToLoad}'? Thao tác này sẽ cập nhật lại kho.", "XÁC NHẬN XÓA", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirmDelete == DialogResult.Yes)
                {
                    XoaHoaDonBanKhoiCSDL(maHDBToLoad); // Đổi tên hàm
                }
            }
            else
            {
                ResetValuesToInitialState();
            }
        }

        private void XoaHoaDonBanKhoiCSDL(string maHDBToDelete) // Đổi tên hàm
        {
            DataTable dtChiTietCanXoa = Function.GetDataToTable(
               "SELECT MaSP, SoLuong FROM dbo.ChiTietHDB WHERE MaHDB = @MaHDB", new SqlParameter("@MaHDB", maHDBToDelete)); // Đổi bảng

            if (dtChiTietCanXoa == null)
            {
                MessageBox.Show($"Không thể lấy chi tiết hóa đơn bán '{maHDBToDelete}'.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                Function.ExecuteTransaction((conn, trans) =>
                {
                    if (dtChiTietCanXoa.Rows.Count > 0)
                    {
                        foreach (DataRow drChiTiet in dtChiTietCanXoa.Rows)
                        {
                            string maSP = drChiTiet["MaSP"].ToString();
                            decimal soLuongDaBan = Convert.ToDecimal(drChiTiet["SoLuong"]); // Đổi tên biến
                            // CỘNG TRẢ LẠI KHO
                            string sqlUpdateKho = @"UPDATE dbo.SanPham SET SoLuong = ISNULL(SoLuong, 0) + @SoLuongTraLai WHERE MaSP = @MaSPKho"; // Cộng trả
                            SqlParameter[] paramsKho = { new SqlParameter("@SoLuongTraLai", soLuongDaBan), new SqlParameter("@MaSPKho", maSP) };
                            Function.ExecuteNonQuery(sqlUpdateKho, conn, trans, paramsKho);
                        }
                    }
                    // Xóa ChiTietHDB
                    string sqlDeleteChiTiet = "DELETE FROM dbo.ChiTietHDB WHERE MaHDB = @MaHDB"; // Đổi bảng
                    Function.ExecuteNonQuery(sqlDeleteChiTiet, conn, trans, new SqlParameter("@MaHDB", maHDBToDelete));
                    // Đánh dấu xóa HoaDonBan
                    string sqlMarkAsDeletedHDB = "UPDATE dbo.HoaDonBan SET IsDeleted = 1, TongTien = 0 WHERE MaHDB = @MaHDB"; // Đổi bảng
                    Function.ExecuteNonQuery(sqlMarkAsDeletedHDB, conn, trans, new SqlParameter("@MaHDB", maHDBToDelete));
                });

                MessageBox.Show($"Đã xóa hóa đơn bán '{maHDBToDelete}'.", "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetValuesToInitialState();
                // Đổi ComboBox tìm kiếm
                Function.FillCombo(cboMaHDBSearch, "MaHDB", "MaHDB", "SELECT MaHDB FROM dbo.HoaDonBan WHERE IsDeleted = 0 ORDER BY NgayBan DESC, MaHDB DESC");
                if (cboMaHDBSearch.Items.Count > 0) cboMaHDBSearch.SelectedIndex = -1; else cboMaHDBSearch.DataSource = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa hóa đơn bán:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnInHDB_Click(object sender, EventArgs e)
        {
            string maHDBCCanIn = txtMaHDB.Text.Trim(); // Đổi biến
            if (string.IsNullOrWhiteSpace(maHDBCCanIn) || string.IsNullOrEmpty(maHDBToLoad) || !maHDBCCanIn.Equals(maHDBToLoad, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Vui lòng tải hóa đơn bán đã lưu để in.", "Chưa có hóa đơn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (dtChiTietHDB == null || dtChiTietHDB.Rows.Count == 0) // Đổi tên dt
            {
                MessageBox.Show("Hóa đơn không có chi tiết.", "Không có dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string ngayBanHienThi = dpNgayBan.Text; // Đổi biến
            string tenNVHienThi = txtTenNV.Text;
            string tenKHHienThi = txtTenKH.Text; // Đổi biến
            string diaChiKH = ""; // Lấy thêm địa chỉ khách hàng nếu cần cho hóa đơn bán
            string sdtKH = "";   // Lấy thêm SĐT khách hàng nếu cần
            if (cboMaKH.SelectedValue != null) // Đổi cboMaNCC
            {
                // Bạn có thể lấy TenKH từ cboMaKH.Text nếu DisplayMember là TenKH, hoặc truy vấn lại CSDL
                // Ví dụ truy vấn lại để lấy các thông tin khác:
                DataTable dtKHInfo = Function.GetDataToTable("SELECT TenKH, DiaChi, SDT FROM dbo.KhachHang WHERE MaKH = @MaKH",
                                                            new SqlParameter("@MaKH", cboMaKH.SelectedValue.ToString()));
                if (dtKHInfo.Rows.Count > 0)
                {
                    tenKHHienThi = dtKHInfo.Rows[0]["TenKH"]?.ToString() ?? tenKHHienThi; // Ưu tiên tên từ CSDL nếu có
                    diaChiKH = dtKHInfo.Rows[0]["DiaChi"]?.ToString();
                    sdtKH = dtKHInfo.Rows[0]["SDT"]?.ToString(); // Nếu có cột SDT trong KhachHang
                }
            }

            string tongTienHienThi = txtTongTien.Text;
            string tongTienBangChuHienThi = lblTongTienBangChu.Text;

            try
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("HoaDonBan_" + maHDBCCanIn); // Đổi tên sheet
                    int currentRow = 1;

                    // Tiêu đề Hóa Đơn Bán
                    worksheet.Cell(currentRow, 3).Value = "HÓA ĐƠN BÁN HÀNG";
                    var titleRange = worksheet.Range(currentRow, 3, currentRow, 5);
                    titleRange.Merge().Style.Font.SetBold().Font.SetFontSize(16).Font.SetFontColor(XLColor.DarkBlue); // Màu khác cho HĐ Bán
                    titleRange.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    currentRow += 2;

                    // Thông tin chung HĐ Bán
                    worksheet.Cell(currentRow, 1).Value = "Mã hóa đơn:"; worksheet.Cell(currentRow, 1).Style.Font.SetBold();
                    worksheet.Cell(currentRow, 2).Value = maHDBCCanIn; worksheet.Range(currentRow, 2, currentRow, 3).Merge();
                    worksheet.Cell(currentRow, 5).Value = "Ngày bán:"; worksheet.Cell(currentRow, 5).Style.Font.SetBold();
                    worksheet.Cell(currentRow, 6).Value = ngayBanHienThi;
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = "Nhân viên:"; worksheet.Cell(currentRow, 1).Style.Font.SetBold();
                    worksheet.Cell(currentRow, 2).Value = tenNVHienThi; worksheet.Range(currentRow, 2, currentRow, 3).Merge();
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = "Khách hàng:"; worksheet.Cell(currentRow, 1).Style.Font.SetBold();
                    worksheet.Cell(currentRow, 2).Value = tenKHHienThi; worksheet.Range(currentRow, 2, currentRow, 6).Merge();
                    currentRow++;
                    if (!string.IsNullOrEmpty(diaChiKH))
                    {
                        worksheet.Cell(currentRow, 1).Value = "Địa chỉ KH:"; worksheet.Cell(currentRow, 1).Style.Font.SetBold();
                        worksheet.Cell(currentRow, 2).Value = diaChiKH; worksheet.Range(currentRow, 2, currentRow, 6).Merge();
                        currentRow++;
                    }
                    // if (!string.IsNullOrEmpty(sdtKH)) { ... } // Tương tự cho SĐT KH
                    currentRow++;

                    // Bảng chi tiết
                    int headerDetailRow = currentRow;
                    worksheet.Cell(headerDetailRow, 1).Value = "STT";
                    worksheet.Cell(headerDetailRow, 2).Value = "Tên Sản Phẩm";
                    worksheet.Cell(headerDetailRow, 3).Value = "Số Lượng";
                    worksheet.Cell(headerDetailRow, 4).Value = "Đơn Giá Bán"; // Đổi
                    worksheet.Cell(headerDetailRow, 5).Value = "Khuyến Mãi";
                    worksheet.Cell(headerDetailRow, 6).Value = "Thành Tiền";
                    var headerDetailRange = worksheet.Range(headerDetailRow, 1, headerDetailRow, 6);
                    headerDetailRange.Style.Font.SetBold().Fill.SetBackgroundColor(XLColor.LightSkyBlue); // Màu khác
                    headerDetailRange.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    currentRow++;

                    int stt = 0;
                    foreach (DataRow dr in dtChiTietHDB.Rows) // Đổi dt
                    {
                        if (dr.RowState == DataRowState.Deleted) continue;
                        stt++;
                        worksheet.Cell(currentRow, 1).Value = stt;
                        worksheet.Cell(currentRow, 2).Value = dr["TenSP"]?.ToString();
                        worksheet.Cell(currentRow, 3).Value = Convert.ToDecimal(dr["SoLuong"]); worksheet.Cell(currentRow, 3).Style.NumberFormat.Format = "#,##0";
                        worksheet.Cell(currentRow, 4).Value = Convert.ToDecimal(dr["DonGia"]); worksheet.Cell(currentRow, 4).Style.NumberFormat.Format = "#,##0"; // Lấy DonGia từ dtChiTietHDB
                        worksheet.Cell(currentRow, 5).Value = dr["KhuyenMai"]?.ToString();
                        worksheet.Cell(currentRow, 6).Value = Convert.ToDecimal(dr["ThanhTien"]); worksheet.Cell(currentRow, 6).Style.NumberFormat.Format = "#,##0";
                        currentRow++;
                    }
                    currentRow++;

                    // Tổng tiền
                    worksheet.Cell(currentRow, 5).Value = "Tổng cộng:"; worksheet.Cell(currentRow, 5).Style.Font.SetBold().Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    worksheet.Cell(currentRow, 6).Value = Convert.ToDecimal(tongTienHienThi.Replace(",", "")); worksheet.Cell(currentRow, 6).Style.Font.SetBold().NumberFormat.Format = "#,##0";
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = tongTienBangChuHienThi; worksheet.Range(currentRow, 1, currentRow, 6).Merge().Style.Font.SetBold().Font.SetItalic();
                    currentRow += 2;

                    // Ký tên
                    worksheet.Cell(currentRow, 2).Value = "Người Lập Phiếu"; worksheet.Cell(currentRow, 2).Style.Font.SetBold().Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Range(currentRow, 1, currentRow, 2).Merge();
                    worksheet.Cell(currentRow, 5).Value = "Khách Hàng"; worksheet.Cell(currentRow, 5).Style.Font.SetBold().Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Range(currentRow, 4, currentRow, 6).Merge();
                    currentRow += 3;
                    worksheet.Cell(currentRow, 2).Value = tenNVHienThi; worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Range(currentRow, 1, currentRow, 2).Merge();
                    worksheet.Cell(currentRow, 5).Value = tenKHHienThi; worksheet.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Range(currentRow, 4, currentRow, 6).Merge();

                    worksheet.Columns().AdjustToContents();
                    worksheet.Column(2).Width = Math.Max(worksheet.Column(2).Width, 30);
                    var detailTableRange = worksheet.Range(headerDetailRow, 1, currentRow - 4, 6);
                    detailTableRange.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Border.SetInsideBorder(XLBorderStyleValues.Thin);

                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        Filter = "Excel Files|*.xlsx",
                        Title = "Lưu Hóa Đơn Bán",
                        FileName = "HDB_" + maHDBCCanIn.Replace("/", "_").Replace(":", "") + ".xlsx" // Đổi tên file
                    };
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        workbook.SaveAs(saveFileDialog.FileName);
                        MessageBox.Show($"Đã xuất hóa đơn bán '{maHDBCCanIn}' thành công!\nĐường dẫn: {saveFileDialog.FileName}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (MessageBox.Show("Mở file vừa xuất?", "Mở file", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            try { System.Diagnostics.Process.Start(saveFileDialog.FileName); }
                            catch (Exception exOpen) { MessageBox.Show("Lỗi mở file: " + exOpen.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất Excel: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            if (KiemTraThayDoiChuaLuu())
            {
                DialogResult confirmClose = MessageBox.Show("Bạn có thay đổi chưa được lưu. Bạn có chắc chắn muốn đóng không?",
                                                          "Xác nhận đóng", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirmClose == DialogResult.No)
                {
                    return;
                }
            }
            this.Close();
        }

        private void cboMaHDBSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.Visible || !cboMaHDBSearch.Focused || cboMaHDBSearch.SelectedValue == null || cboMaHDBSearch.SelectedIndex == -1) // Đổi tên
            {
                if (cboMaHDBSearch.Focused && cboMaHDBSearch.SelectedIndex == -1 && !string.IsNullOrEmpty(maHDBToLoad)) // Đổi tên
                {
                    if (KiemTraThayDoiChuaLuu())
                    {
                        DialogResult confirmClear = MessageBox.Show("Thay đổi chưa lưu. Bỏ qua và làm mới form?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (confirmClear == DialogResult.Yes) ResetValuesToInitialState();
                        else
                        {
                            this.cboMaHDBSearch.SelectedIndexChanged -= new System.EventHandler(this.cboMaHDBSearch_SelectedIndexChanged); // Đổi tên
                            cboMaHDBSearch.SelectedValue = maHDBToLoad; // Đổi tên
                            this.cboMaHDBSearch.SelectedIndexChanged += new System.EventHandler(this.cboMaHDBSearch_SelectedIndexChanged); // Đổi tên
                        }
                    }
                    else ResetValuesToInitialState();
                }
                return;
            }
            string maHDBDuocChon = cboMaHDBSearch.SelectedValue.ToString(); // Đổi tên
            if (!string.IsNullOrWhiteSpace(txtMaHDB.Text) && txtMaHDB.Text.Equals(maHDBDuocChon, StringComparison.OrdinalIgnoreCase)) return; // Đổi tên

            if (KiemTraThayDoiChuaLuu())
            {
                DialogResult confirm = MessageBox.Show($"Thay đổi chưa lưu. Tải hóa đơn '{maHDBDuocChon}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm == DialogResult.No)
                {
                    this.cboMaHDBSearch.SelectedIndexChanged -= new System.EventHandler(this.cboMaHDBSearch_SelectedIndexChanged); // Đổi tên
                    if (!string.IsNullOrEmpty(maHDBToLoad)) cboMaHDBSearch.SelectedValue = maHDBToLoad; // Đổi tên
                    else if (!string.IsNullOrWhiteSpace(txtMaHDB.Text)) cboMaHDBSearch.SelectedIndex = -1; // Đổi tên
                    this.cboMaHDBSearch.SelectedIndexChanged += new System.EventHandler(this.cboMaHDBSearch_SelectedIndexChanged); // Đổi tên
                    return;
                }
            }
            ResetValuesToInitialState();
            maHDBToLoad = maHDBDuocChon; // Đổi tên
            txtMaHDB.Text = maHDBToLoad;   // Đổi tên
            LoadThongTinHoaDon(maHDBToLoad); // Đổi tên
        }

        private bool KiemTraThayDoiChuaLuu()
        {
            if (string.IsNullOrEmpty(maHDBToLoad) && !string.IsNullOrWhiteSpace(txtMaHDB.Text)) // Đổi tên
            {
                if (dtChiTietHDB != null && dtChiTietHDB.Rows.Cast<DataRow>().Any(row => row.RowState != DataRowState.Deleted)) return true; // Đổi tên
                if (cboMaNV.SelectedIndex != -1 || cboMaKH.SelectedIndex != -1) return true; // Đổi cboMaNCC
            }
            else if (!string.IsNullOrEmpty(maHDBToLoad) && dtChiTietHDBGoc != null) // Đổi tên
            {
                int soDongMoi = dtChiTietHDB.Rows.Cast<DataRow>().Count(row => row.RowState != DataRowState.Deleted); // Đổi tên
                if (soDongMoi != dtChiTietHDBGoc.Rows.Count) return true; // Đổi tên
                foreach (DataRow rowMoi in dtChiTietHDB.Rows) // Đổi tên
                {
                    if (rowMoi.RowState == DataRowState.Deleted) continue;
                    string maSPMoi = rowMoi["MaSP"].ToString();
                    DataRow[] rowsGoc = dtChiTietHDBGoc.Select($"MaSP = '{maSPMoi.Replace("'", "''")}'"); // Đổi tên
                    if (rowsGoc.Length == 0) return true;
                    else
                    {
                        DataRow rowGoc = rowsGoc[0];
                        if (Convert.ToDecimal(rowMoi["SoLuong"]) != Convert.ToDecimal(rowGoc["SoLuong"]) ||
                            Convert.ToDecimal(rowMoi["DonGia"]) != Convert.ToDecimal(rowGoc["DonGia"]) || // DonGia (bán) vẫn được so sánh vì nó thể hiện giá lúc bán
                            (rowMoi["KhuyenMai"]?.ToString() ?? "") != (rowGoc["KhuyenMai"]?.ToString() ?? ""))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void btnHuyBoDong_Click(object sender, EventArgs e)
        {
            ResetChiTietSanPhamFields();
            dgvChiTietHDB.ClearSelection(); // Kích hoạt SelectionChanged để reset nút thêm/sửa/xóa dòng
            //btnThemDong.Enabled = true; // Đảm bảo nút Thêm dòng được bật lại
            //btnSuaDong.Enabled = false;
            //btnXoaDong.Enabled = false;
            //btnHuyBoDong.Enabled = false;
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
