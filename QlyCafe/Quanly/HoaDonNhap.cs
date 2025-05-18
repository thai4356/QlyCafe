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
            LoadComboBoxes();
            CustomizeDgvChiTietHDN();

            // Thiết lập ReadOnly cho các TextBox không cho phép nhập trực tiếp
            txtTenNV.ReadOnly = true;
            txtTenNCC.ReadOnly = true;
            txtTenSP.ReadOnly = true;
            txtThanhTienDong.ReadOnly = true; // Sẽ tự tính
            txtTongTien.ReadOnly = true;     // Sẽ tự tính từ tổng các thành tiền dòng
            txtMaHDN.ReadOnly = true;        // Mã HĐN sinh tự động hoặc load từ HĐN cũ

            if (!string.IsNullOrEmpty(maHDNToLoad))
            {
                // Chế độ xem/sửa hóa đơn cũ
                txtMaHDN.Text = maHDNToLoad;
                LoadThongTinHoaDon(maHDNToLoad);
                // Trạng thái nút sẽ được điều chỉnh trong LoadThongTinHoaDon
            }
            else
            {
                // Chế độ chuẩn bị thêm hóa đơn mới (form vừa được mở, chưa nhấn nút "Thêm HĐN")
                ResetValuesToInitialState(); // Đưa form về trạng thái trắng, sẵn sàng
            }
        }

        private void LoadComboBoxes()
        {
            // Load Mã Nhân Viên
            Function.FillCombo(cboMaNV, "MaNV", "MaNV", "SELECT MaNV, TenNV FROM dbo.NhanVien ORDER BY TenNV");
            cboMaNV.SelectedIndex = -1;

            // Load Mã Nhà Cung Cấp
            Function.FillCombo(cboMaNCC, "MaNCC", "MaNCC", "SELECT MaNCC, TenNCC FROM dbo.NhaCungCap ORDER BY TenNCC");
            cboMaNCC.SelectedIndex = -1;

            // Load Mã Sản Phẩm cho việc nhập chi tiết
            Function.FillCombo(cboMaSP, "MaSP", "MaSP", "SELECT MaSP, TenSP FROM dbo.SanPham ORDER BY TenSP");
            cboMaSP.SelectedIndex = -1;

            // Load Mã Hóa Đơn Nhập để người dùng có thể chọn xem/sửa hóa đơn cũ trực tiếp từ form này
            Function.FillCombo(cboMaHDNSearch, "MaHDN", "MaHDN", "SELECT MaHDN FROM dbo.HoaDonNhap WHERE IsDeleted = 0 ORDER BY NgayNhap DESC, MaHDN DESC");
            cboMaHDNSearch.SelectedIndex = -1;
        }

        private void CustomizeDgvChiTietHDN()
        {
            dgvChiTietHDN.AutoGenerateColumns = false;
            dgvChiTietHDN.Columns.Clear();

            dgvChiTietHDN.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MaSP",
                HeaderText = "Mã SP",
                Name = "MaSP",
                Width = 90
            });
            dgvChiTietHDN.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TenSP",
                HeaderText = "Tên Sản Phẩm",
                Name = "TenSP",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 30
            });
            dgvChiTietHDN.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "SoLuong",
                HeaderText = "Số Lượng",
                Name = "SoLuong",
                Width = 80,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight }
            });
            dgvChiTietHDN.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "DonGia",
                HeaderText = "Đơn Giá",
                Name = "DonGia",
                Width = 100,
                DefaultCellStyle = { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }
            });
            dgvChiTietHDN.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "KhuyenMai",
                HeaderText = "Khuyến Mãi",
                Name = "KhuyenMai",
                Width = 110
            });
            dgvChiTietHDN.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ThanhTien",
                HeaderText = "Thành Tiền",
                Name = "ThanhTien",
                Width = 120,
                DefaultCellStyle = { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvChiTietHDN.AllowUserToAddRows = false;
            dgvChiTietHDN.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgvChiTietHDN.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvChiTietHDN.MultiSelect = false;
        }

        private void ResetChiTietSanPhamFields()
        {
            cboMaSP.SelectedIndex = -1;
            txtTenSP.Text = "";
            txtSoLuong.Text = "0";
            txtDonGia.Text = "0";
            txtKhuyenMai.Text = "";
            txtThanhTienDong.Text = "0";
            if (cboMaSP.Enabled) // Chỉ focus nếu control enabled
                cboMaSP.Focus();
        }

        /// <summary>
        /// Đặt lại form về trạng thái ban đầu khi mới load hoặc sau khi hoàn tất/hủy một hóa đơn.
        /// </summary>
        private void ResetValuesToInitialState()
        {
            txtMaHDN.Text = "";
            dpNgayNhap.Value = DateTime.Now;
            cboMaNV.SelectedIndex = -1;
            txtTenNV.Text = "";
            cboMaNCC.SelectedIndex = -1;
            txtTenNCC.Text = "";
            txtTongTien.Text = "0";
            UpdateTongTienBangChu();

            ResetChiTietSanPhamFields();

            // Xóa dữ liệu chi tiết hóa đơn
            if (dtChiTietHDN != null)
            {
                dtChiTietHDN.Rows.Clear(); // Sử dụng Rows.Clear() để giữ lại cấu trúc cột nếu có
            }
            dgvChiTietHDN.DataSource = null; // Gán lại DataSource sau nếu dtChiTietHDN là nguồn chính
            if (dtChiTietHDN != null && dtChiTietHDN.Columns.Count > 0) // Chỉ gán lại nếu dtChiTietHDN có cấu trúc
            {
                dgvChiTietHDN.DataSource = dtChiTietHDN;
            }


            maHDNToLoad = null;

            // Trạng thái nút và controls ban đầu
            SetControlsEnabledState(false); // Disable các group box nhập liệu
            btnThemHDN.Enabled = true;
            btnLuuHDN.Enabled = false;
            btnHuyHDN.Enabled = false; // Hủy hóa đơn (xóa khỏi CSDL)
            btnInHDN.Enabled = false;
            btnDong.Enabled = true; // Nút Đóng form luôn enabled

            btnThemDong.Enabled = false;
            btnSuaDong.Enabled = false;
            btnXoaDong.Enabled = false;
            btnHuyBoDong.Enabled = false; // Hủy thao tác trên dòng chi tiết

            cboMaHDNSearch.Enabled = true;
            cboMaHDNSearch.SelectedIndex = -1;
        }

        /// <summary>
        /// Thiết lập trạng thái Enabled/Disabled cho các controls nhập liệu chính.
        /// </summary>
        private void SetControlsEnabledState(bool isEnabled)
        {
            // Thông tin chung
            dpNgayNhap.Enabled = isEnabled;
            cboMaNV.Enabled = isEnabled;
            cboMaNCC.Enabled = isEnabled;
            // txtMaHDN, txtTenNV, txtTenNCC, txtTongTien là ReadOnly, không cần set Enabled ở đây

            // Chi tiết sản phẩm
            cboMaSP.Enabled = isEnabled;
            txtSoLuong.Enabled = isEnabled;
            txtDonGia.Enabled = isEnabled;
            txtKhuyenMai.Enabled = isEnabled;
            // txtTenSP, txtThanhTienDong là ReadOnly
        }

        /// <summary>
        /// Thiết lập trạng thái các nút khi đang trong quá trình tạo mới hoặc sửa hóa đơn.
        /// </summary>
        private void SetActiveProcessingButtonStates(bool isEditingOldInvoice)
        {
            SetControlsEnabledState(true);

            btnThemHDN.Enabled = false; // Đã nhấn "Thêm HĐN" hoặc đang sửa HĐN cũ
            btnLuuHDN.Enabled = true;
            // Nút Hủy HĐN:
            // Nếu là hóa đơn mới (chưa lưu), nút này có thể là "Hủy thao tác" (ResetValuesToInitialState)
            // Nếu là hóa đơn cũ, nút này là "Xóa Hóa Đơn" (khỏi CSDL)
            btnHuyHDN.Enabled = true;
            btnInHDN.Enabled = isEditingOldInvoice; // Chỉ in được hóa đơn đã tồn tại

            btnThemDong.Enabled = true;
            // btnSuaDong, btnXoaDong sẽ được bật khi chọn dòng trong DataGridView
            // btnHuyBoDong sẽ bật khi đang sửa dòng

            cboMaHDNSearch.Enabled = false; // Không cho tìm hóa đơn khác khi đang làm việc
        }

        private void LoadThongTinHoaDon(string maHDN)
        {
            string sql = "SELECT NgayNhap, MaNV, MaNCC, TongTien FROM dbo.HoaDonNhap WHERE MaHDN = @MaHDN AND IsDeleted = 0";
            SqlParameter param = new SqlParameter("@MaHDN", maHDN);
            DataTable dtThongTinChung = Function.GetDataToTable(sql, param);

            if (dtThongTinChung.Rows.Count > 0)
            {
                DataRow row = dtThongTinChung.Rows[0];
                dpNgayNhap.Value = Convert.ToDateTime(row["NgayNhap"]);

                // Cẩn thận khi gán SelectedValue, đảm bảo ComboBox đã load xong và giá trị tồn tại
                if (cboMaNV.DataSource != null) cboMaNV.SelectedValue = row["MaNV"]; else cboMaNV.Text = row["MaNV"].ToString();
                if (cboMaNCC.DataSource != null) cboMaNCC.SelectedValue = row["MaNCC"]; else cboMaNCC.Text = row["MaNCC"].ToString();

                // Cập nhật tên NV và NCC (sự kiện SelectedIndexChanged sẽ làm điều này)
                // Nếu không, gọi trực tiếp:
                UpdateTenNVFromMaNV();
                UpdateTenNCCFromMaNCC();

                txtTongTien.Text = Convert.ToDecimal(row["TongTien"]).ToString("N0", CultureInfo.InvariantCulture);
                UpdateTongTienBangChu();

                LoadChiTietHoaDon(maHDN);
                SetActiveProcessingButtonStates(true); // Trạng thái nút cho sửa hóa đơn cũ
            }
            else
            {
                MessageBox.Show("Không tìm thấy thông tin hóa đơn nhập hoặc hóa đơn đã bị xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ResetValuesToInitialState();
            }
        }

        private void LoadChiTietHoaDon(string maHDN)
        {
            string sql = @"SELECT cthdn.MaSP, sp.TenSP, cthdn.SoLuong, cthdn.DonGia, cthdn.KhuyenMai, cthdn.ThanhTien 
                       FROM dbo.ChiTietHDN cthdn 
                       INNER JOIN dbo.SanPham sp ON cthdn.MaSP = sp.MaSP 
                       WHERE cthdn.MaHDN = @MaHDN";
            SqlParameter param = new SqlParameter("@MaHDN", maHDN);
            dtChiTietHDN = Function.GetDataToTable(sql, param);
            dgvChiTietHDN.DataSource = dtChiTietHDN;
        }

        private void UpdateTenNVFromMaNV()
        {
            if (cboMaNV.SelectedValue != null && cboMaNV.SelectedIndex != -1)
            {
                // Cách 2: Lấy lại từ CSDL (an toàn hơn nếu ComboBox có thể không đồng bộ)
                string tenNV = Function.GetFieldValue("SELECT TenNV FROM dbo.NhanVien WHERE MaNV = @MaNV", new SqlParameter("@MaNV", cboMaNV.SelectedValue.ToString()));
                txtTenNV.Text = tenNV;
            }
            else
            {
                txtTenNV.Text = "";
            }
        }

        private void UpdateTenNCCFromMaNCC()
        {
            if (cboMaNCC.SelectedValue != null && cboMaNCC.SelectedIndex != -1)
            {
                string tenNCC = Function.GetFieldValue("SELECT TenNCC FROM dbo.NhaCungCap WHERE MaNCC = @MaNCC", new SqlParameter("@MaNCC", cboMaNCC.SelectedValue.ToString()));
                txtTenNCC.Text = tenNCC;
            }
            else
            {
                txtTenNCC.Text = "";
            }
        }

        private void cboMaNCC_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTenNCCFromMaNCC();
        }

        private void cboMaNV_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTenNVFromMaNV();
        }

        private void cboMaSP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMaSP.SelectedValue != null && cboMaSP.SelectedIndex != -1)
            {
                DataRowView drv = cboMaSP.SelectedItem as DataRowView;
                if (drv != null)
                {
                    txtTenSP.Text = drv["TenSP"].ToString();
                }
                // Có thể load luôn đơn giá nhập mặc định của sản phẩm vào txtDonGia nếu muốn
                // string donGiaNhap = Function.GetFieldValue("SELECT GiaNhap FROM dbo.SanPham WHERE MaSP = @MaSP", new SqlParameter("@MaSP", cboMaSP.SelectedValue.ToString()));
                // txtDonGia.Text = string.IsNullOrEmpty(donGiaNhap) ? "0" : Convert.ToDecimal(donGiaNhap).ToString("N0");
            }
            else
            {
                txtTenSP.Text = "";
                // txtDonGia.Text = "0";
            }
            // Tự động tính lại thành tiền dòng khi mã sản phẩm thay đổi (nếu số lượng, đơn giá đã có)
             TinhThanhTienDong();
        }

        private void UpdateTongTienBangChu()
        {
            // Giả sử bạn đã có hàm ChuyenSoSangChu trong class Function hoặc ở đâu đó
            // Nếu chưa, bạn cần triển khai nó.
            // Ví dụ tham khảo từ GiaotrinhC#2.docx
            if (string.IsNullOrWhiteSpace(txtTongTien.Text))
            {
                lblTongTienBangChu.Text = "Bằng chữ: Không đồng";
                return;
            }
            decimal tongTienSo;
            if (decimal.TryParse(txtTongTien.Text.Replace(",", ""), NumberStyles.Any, CultureInfo.InvariantCulture, out tongTienSo))
            {
                // lblTongTienBangChu.Text = "Bằng chữ: " + Function.ChuyenSoSangChu(tongTienSo.ToString("F0")); // F0 để không có phần thập phân
                lblTongTienBangChu.Text = "Bằng chữ: " + ChuyenSoSangChuHelper(tongTienSo.ToString("F0")); // Gọi hàm helper nếu cần
            }
            else
            {
                lblTongTienBangChu.Text = "Bằng chữ: (Số không hợp lệ)";
            }
        }

        // Hàm helper để gọi ChuyenSoSangChu (bạn cần có hàm này trong class Function hoặc ở đây)
        private string ChuyenSoSangChuHelper(string number)
        {
            // Đây là nơi bạn sẽ gọi hàm chuyển số sang chữ thực sự
            // Ví dụ: return new SoThanhChu().DocSo(number);
            // Tạm thời trả về chuỗi để không lỗi:
            if (string.IsNullOrEmpty(number) || number == "0") return "Không đồng";

            long num;
            if (!long.TryParse(number, out num)) return "(Không thể chuyển đổi)";

            if (num == 0) return "Không";
            string[] units = { "", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] teens = { "mười", "mười một", "mười hai", "mười ba", "mười bốn", "mười lăm", "mười sáu", "mười bảy", "mười tám", "mười chín" };
            string[] tens = { "", "mười", "hai mươi", "ba mươi", "bốn mươi", "năm mươi", "sáu mươi", "bảy mươi", "tám mươi", "chín mươi" };
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

        private string DocNhomBaChuSo(int num) //Đọc 1 nhóm 3 chữ số
        {
            string[] units = { "", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string tram = units[num / 100];
            string chuc = units[(num % 100) / 10];
            string donvi = units[num % 10];
            string result = "";

            if (num == 0) return "";
            if (!string.IsNullOrEmpty(tram)) result += tram + " trăm ";

            if ((num % 100) > 0) //có hàng chục hoặc đơn vị
            {
                if (string.IsNullOrEmpty(tram) && string.IsNullOrEmpty(chuc) && (num % 100 < 10)) // 00x
                {
                    // không cần "linh" nếu nhóm 3 số đầu tiên là 00x (ví dụ: 1 005)
                }
                else if (string.IsNullOrEmpty(tram) && (num % 100 < 10)) // vd 005, 001 (cho các nhóm sau)
                {
                    if (result != "") result += "không trăm linh "; // Nếu trước đó có tỷ, triệu... và nhóm này là 00x thì thêm "linh"
                    else if (num < 10 && (num % 1000 > 0)) result += "linh "; //vd: 1,000,005 -> một triệu không trăm linh năm
                }
                else if (string.IsNullOrEmpty(chuc) && !string.IsNullOrEmpty(donvi)) // x0y (vd 105)
                {
                    if (!string.IsNullOrEmpty(tram)) result += "linh "; // một trăm linh năm
                                                                        // else result += "linh "; // bỏ trường hợp này để tránh "linh năm"
                }


                if ((num % 100) < 10) // 0 -> 9
                {
                    // đã xử lý ở trên hoặc sẽ là đơn vị
                }
                else if ((num % 100) < 20) // 10 -> 19
                {
                    result += ((num % 100) == 15) ? "mười lăm " : ("mười " + ((donvi == "một") ? "mốt " : donvi + " "));
                }
                else // 20 -> 99
                {
                    result += chuc + " mươi ";
                    if (donvi == "một" && chuc != "mười") result += "mốt ";
                    else if (donvi == "bốn" && chuc != "mười") result += "tư ";
                    else if (donvi == "năm" && chuc != "mười" && !string.IsNullOrEmpty(chuc)) result += "lăm "; // năm mươi lăm
                    else if (!string.IsNullOrEmpty(donvi)) result += donvi + " ";
                }
                if ((num % 100) > 0 && (num % 100) < 10 && string.IsNullOrEmpty(chuc) && string.IsNullOrEmpty(tram))
                {
                    // nếu chỉ có đơn vị và không có trăm, chục (ví dụ 005 trong 1,000,005)
                    // đã thêm "linh" ở trên nếu cần
                }
                else if ((num % 100) > 0 && (num % 100) < 10 && string.IsNullOrEmpty(chuc) && !string.IsNullOrEmpty(tram))
                { // ví dụ 105
                  // đã thêm "linh" ở trên
                    result += donvi + " ";
                }
                else if ((num % 100) > 0 && (num % 100) < 10 && !string.IsNullOrEmpty(chuc))
                { // ví dụ 15 (không rơi vào đây), 25, 21

                }


                if (string.IsNullOrEmpty(donvi) && (num % 100) >= 10 && !string.IsNullOrEmpty(chuc))
                { // ví dụ 20, 30
                  // không thêm gì
                }
                else if ((num % 100) > 0 && (num % 100) < 10 && string.IsNullOrEmpty(chuc))
                { // 00x
                    result += donvi + " ";
                }
            }
            return result.Trim().Replace("  ", " ");
        }

        private void TinhThanhTienDong()
        {
            decimal soLuong = 0;
            decimal donGia = 0;
            decimal khuyenMaiPercent = 0; // Giả sử khuyến mãi là %
            decimal thanhTienDong = 0;

            // Cố gắng parse giá trị từ TextBox, sử dụng CultureInfo.InvariantCulture để xử lý dấu '.' hoặc ',' nhất quán
            // và NumberStyles.Any để cho phép các ký hiệu tiền tệ hoặc dấu phẩy nhóm.
            decimal.TryParse(txtSoLuong.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out soLuong);
            decimal.TryParse(txtDonGia.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out donGia);
            decimal.TryParse(txtKhuyenMai.Text.Replace("%", ""), NumberStyles.Any, CultureInfo.InvariantCulture, out khuyenMaiPercent);

            // Kiểm tra các giá trị âm hoặc không hợp lệ
            if (soLuong < 0) soLuong = 0;
            if (donGia < 0) donGia = 0;
            if (khuyenMaiPercent < 0) khuyenMaiPercent = 0;
            if (khuyenMaiPercent > 100) khuyenMaiPercent = 100; // Giới hạn khuyến mãi tối đa 100%

            // Tính toán thành tiền
            // Công thức: Thành tiền = (Số lượng * Đơn giá) * (1 - Khuyến mãi % / 100)
            thanhTienDong = (soLuong * donGia) * (1 - (khuyenMaiPercent / 100));

            // Hiển thị thành tiền đã tính, định dạng kiểu số (ví dụ: "N0" để có dấu phẩy ngăn cách hàng nghìn, không có số lẻ thập phân)
            txtThanhTienDong.Text = thanhTienDong.ToString("N0", CultureInfo.InvariantCulture);
        }

        // Hàm xử lý sự kiện TextChanged chung cho các TextBox liên quan đến tính toán chi tiết
        private void txtChiTietSP_TextChanged(object sender, EventArgs e)
        {
            TinhThanhTienDong();
        }

        public void CapNhatTongTienHoaDon()
        {
            decimal tongTienHoaDon = 0;

            if (dtChiTietHDN != null)
            {
                foreach (DataRow row in dtChiTietHDN.Rows)
                {
                    // Chỉ tính những dòng không bị đánh dấu xóa (nếu bạn có cơ chế xóa dòng logic)
                    if (row.RowState != DataRowState.Deleted && row["ThanhTien"] != DBNull.Value)
                    {
                        tongTienHoaDon += Convert.ToDecimal(row["ThanhTien"]);
                    }
                }
            }
            txtTongTien.Text = tongTienHoaDon.ToString("N0", CultureInfo.InvariantCulture);
            UpdateTongTienBangChu(); // Gọi hàm cập nhật tổng tiền bằng chữ bạn đã có
        }

        private void dgvChiTietHDN_SelectionChanged(object sender, EventArgs e)
        {
            // Kiểm tra xem có dòng nào được chọn không và DataGridView có dữ liệu không
            if (dgvChiTietHDN.CurrentRow != null && dgvChiTietHDN.CurrentRow.Index != -1 && dgvChiTietHDN.CurrentRow.DataBoundItem != null)
            {
                DataGridViewRow selectedRow = dgvChiTietHDN.CurrentRow;

                // Lấy giá trị từ các ô của dòng được chọn.
                // Sử dụng tên cột bạn đã định nghĩa trong CustomizeDgvChiTietHDN()
                string maSP = selectedRow.Cells["MaSP"].Value?.ToString();
                string tenSP = selectedRow.Cells["TenSP"].Value?.ToString(); // TenSP đã có sẵn
                string soLuongStr = selectedRow.Cells["SoLuong"].Value?.ToString();
                string donGiaStr = selectedRow.Cells["DonGia"].Value?.ToString();
                string khuyenMaiStr = selectedRow.Cells["KhuyenMai"].Value?.ToString();
                // txtThanhTienDong sẽ được tính lại hoặc lấy trực tiếp nếu bạn muốn
                string thanhTienDongStr = selectedRow.Cells["ThanhTien"].Value?.ToString();

                // --- Hiển thị lên các controls ---

                // 1. ComboBox Mã Sản Phẩm (cboMaSP)
                if (!string.IsNullOrEmpty(maSP))
                {
                    // Cố gắng chọn giá trị trong ComboBox.
                    // Điều này yêu cầu cboMaSP đã được load dữ liệu và ValueMember của nó là MaSP.
                    cboMaSP.SelectedValue = maSP;
                    // Nếu SelectedValue không hoạt động như mong muốn (ví dụ do kiểu dữ liệu khác nhau giữa cell và ValueMember),
                    // bạn có thể cần phải duyệt qua Items của ComboBox để tìm và chọn đúng.
                    // Hoặc nếu bạn chỉ muốn hiển thị Text: cboMaSP.Text = maSP; (nhưng SelectedValue tốt hơn)
                }
                else
                {
                    cboMaSP.SelectedIndex = -1;
                }

                // 2. TextBox Tên Sản Phẩm (txtTenSP)
                txtTenSP.Text = tenSP; // Đã có sẵn

                // 3. TextBox Số Lượng (txtSoLuong)
                // Hiển thị số lượng nguyên bản, không cần định dạng đặc biệt khi hiển thị lại để sửa
                txtSoLuong.Text = soLuongStr ?? "0";

                // 4. TextBox Đơn Giá (txtDonGia)
                // Hiển thị đơn giá, có thể giữ nguyên định dạng số từ cell hoặc parse lại
                decimal donGiaDecimal;
                if (decimal.TryParse(donGiaStr, NumberStyles.Any, CultureInfo.InvariantCulture, out donGiaDecimal))
                {
                    txtDonGia.Text = donGiaDecimal.ToString(CultureInfo.InvariantCulture); // Hiển thị số thuần để dễ sửa
                }
                else
                {
                    txtDonGia.Text = "0";
                }

                // 5. TextBox Khuyến Mãi (txtKhuyenMai)
                txtKhuyenMai.Text = khuyenMaiStr ?? ""; // Khuyến mãi có thể là dạng text (ví dụ "10%")

                // 6. TextBox Thành Tiền Dòng (txtThanhTienDong)
                // Thành tiền dòng thường là ReadOnly và được tính tự động.
                // Khi click dòng, bạn có thể hiển thị lại giá trị đã tính, hoặc gọi TinhThanhTienDong()
                // để nó tự cập nhật dựa trên các giá trị vừa được load.
                decimal thanhTienDecimal;
                if (decimal.TryParse(thanhTienDongStr, NumberStyles.Any, CultureInfo.InvariantCulture, out thanhTienDecimal))
                {
                    txtThanhTienDong.Text = thanhTienDecimal.ToString("N0", CultureInfo.InvariantCulture);
                }
                else
                {
                    txtThanhTienDong.Text = "0";
                }
                // Hoặc gọi TinhThanhTienDong() nếu muốn nó tính lại dựa trên các control vừa được set:
                 TinhThanhTienDong();


                // --- Điều chỉnh trạng thái các nút cho việc Sửa/Xóa dòng ---
                // Chỉ cho phép sửa/xóa dòng nếu hóa đơn đang ở trạng thái có thể lưu (tức là đang tạo mới hoặc sửa hóa đơn cũ)
                if (btnLuuHDN.Enabled) // Kiểm tra nút Lưu Hóa Đơn
                {
                    btnThemDong.Enabled = false;   // Tạm tắt nút Thêm Dòng để tập trung vào Sửa/Xoá
                    btnSuaDong.Enabled = true;
                    btnXoaDong.Enabled = true;
                    btnHuyBoDong.Enabled = true; // Cho phép hủy bỏ các thay đổi trên controls
                }
            }
            else
            {
                // Nếu không có dòng nào được chọn (ví dụ: DataGridView trống hoặc vừa clear)
                // ResetChiTietSanPhamFields(); // Tùy chọn: Xóa trắng các control chi tiết
                if (btnLuuHDN.Enabled) // Chỉ thay đổi trạng thái nút nếu HĐN đang được phép chỉnh sửa
                {
                    btnThemDong.Enabled = true; // Bật lại nút Thêm Dòng
                    btnSuaDong.Enabled = false;
                    btnXoaDong.Enabled = false;
                    btnHuyBoDong.Enabled = false;
                }
            }
        }

        private void btnThemHDN_Click(object sender, EventArgs e)
        {

        }
    }
}
