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
                                             // Trong HoaDonNhap_Load():
                                             // dtChiTietHDN sẽ được khởi tạo và gán DataSource cho dgvChiTietHDN   
                //txtMaHDN.Text =  GenerateMaHDN();
                if (dtChiTietHDN == null) // Khởi tạo và định nghĩa cột cho dtChiTietHDN nếu chưa có
                {
                    dtChiTietHDN = new DataTable("ChiTietHDN_Table");
                    dtChiTietHDN.Columns.Add("MaSP", typeof(string));
                    dtChiTietHDN.Columns.Add("TenSP", typeof(string));
                    dtChiTietHDN.Columns.Add("SoLuong", typeof(decimal));
                    dtChiTietHDN.Columns.Add("DonGia", typeof(decimal));
                    dtChiTietHDN.Columns.Add("KhuyenMai", typeof(string));
                    dtChiTietHDN.Columns.Add("ThanhTien", typeof(decimal));
                }
                // Gán dtChiTietHDN làm nguồn dữ liệu cho DataGridView.
                // Việc này chỉ cần làm một lần.
                if (dgvChiTietHDN.DataSource == null) // Chỉ gán nếu chưa được gán
                {
                    dgvChiTietHDN.DataSource = dtChiTietHDN;
                }

                CustomizeDgvChiTietHDN();
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

        private void SetInitialButtonStatesAndControls() // Đổi tên và mục đích rõ ràng hơn
        {
            SetControlsEnabledState(false); // Vô hiệu hóa các vùng nhập liệu chính

            btnThemHDN.Enabled = true;    // LUÔN BẬT
            btnLuuHDN.Enabled = false;
            btnHuyHDN.Enabled = false;
            btnInHDN.Enabled = false;
            btnDong.Enabled = true;

            btnThemDong.Enabled = false;
            btnSuaDong.Enabled = false;
            btnXoaDong.Enabled = false;
            btnHuyBoDong.Enabled = false;

            cboMaHDNSearch.Enabled = true;
            if (cboMaHDNSearch.Items.Count > 0) cboMaHDNSearch.SelectedIndex = -1;
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

            if (dtChiTietHDN != null)
            {
                dtChiTietHDN.Rows.Clear();
            }
            // dgvChiTietHDN sẽ tự cập nhật

            maHDNToLoad = null;
            SetInitialButtonStatesAndControls(); // Gọi hàm mới để thiết lập trạng thái nút
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

            btnThemHDN.Enabled = true;    // LUÔN BẬT
            btnLuuHDN.Enabled = true;
            btnHuyHDN.Enabled = true;
            btnInHDN.Enabled = isEditingOldInvoice;

            btnThemDong.Enabled = true;  // BẬT KHI BẮT ĐẦU THAO TÁC HÓA ĐƠN
            btnSuaDong.Enabled = false;  // Sẽ bật khi chọn dòng
            btnXoaDong.Enabled = false;  // Sẽ bật khi chọn dòng
            btnHuyBoDong.Enabled = false; // Sẽ bật khi chọn dòng và bắt đầu sửa

            cboMaHDNSearch.Enabled = !btnLuuHDN.Enabled; // Tắt tìm kiếm khi đang có hóa đơn chưa lưu
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
                string maSPChon = cboMaSP.SelectedValue.ToString();

                // 1. Cập nhật Tên Sản Phẩm (txtTenSP)
                DataRowView drv = cboMaSP.SelectedItem as DataRowView;
                if (drv != null)
                {
                    txtTenSP.Text = drv["TenSP"].ToString();
                }
                else // Trường hợp dự phòng nếu không lấy được từ DataRowView
                {
                    txtTenSP.Text = Function.GetFieldValue("SELECT TenSP FROM dbo.SanPham WHERE MaSP = @MaSP",
                                                         new SqlParameter("@MaSP", maSPChon));
                }

                // 2. LẤY VÀ CẬP NHẬT ĐƠN GIÁ NHẬP (txtDonGia) TỪ BẢNG SANPHAM
                string giaNhapStr = Function.GetFieldValue("SELECT GiaNhap FROM dbo.SanPham WHERE MaSP = @MaSP",
                                                         new SqlParameter("@MaSP", maSPChon));

                decimal giaNhapDecimal;
                if (decimal.TryParse(giaNhapStr, NumberStyles.Any, CultureInfo.InvariantCulture, out giaNhapDecimal))
                {
                    // Hiển thị giá nhập. Người dùng có thể chỉnh sửa giá này nếu cần thiết
                    // cho lần nhập hàng cụ thể này.
                    // Hiển thị số thuần, không định dạng, để dễ sửa.
                    txtDonGia.Text = giaNhapDecimal.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    txtDonGia.Text = "0"; // Nếu không có giá nhập hoặc lỗi, đặt là 0 và cho phép người dùng nhập
                                          // Có thể hiển thị một cảnh báo nhỏ hoặc log lỗi ở đây nếu giá nhập không tìm thấy
                                          // MessageBox.Show($"Không tìm thấy giá nhập cho sản phẩm {maSPChon}. Vui lòng nhập thủ công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // 3. Reset các trường khác để người dùng nhập mới cho sản phẩm này
                txtSoLuong.Text = "1"; // Đặt số lượng mặc định là 1 (hoặc 0 tùy ý bạn)
                txtKhuyenMai.Text = ""; // Xóa khuyến mãi cũ (hoặc đặt mặc định là "0%" )

                // txtThanhTienDong sẽ tự động cập nhật do sự kiện TextChanged của txtDonGia và txtSoLuong
                // đã được gán để gọi TinhThanhTienDong().
                // Nếu muốn chắc chắn nó cập nhật ngay, có thể gọi TinhThanhTienDong() ở đây.
                TinhThanhTienDong(); // Gọi để tính lại thành tiền với đơn giá mới và số lượng mặc định

                txtSoLuong.Focus(); // Đưa focus vào Số lượng để người dùng nhập tiếp
            }
            else // Nếu không có sản phẩm nào được chọn (ví dụ, chọn dòng trống)
            {
                txtTenSP.Text = "";
                txtDonGia.Text = "0";
                txtSoLuong.Text = "0";
                txtKhuyenMai.Text = "";
                txtThanhTienDong.Text = "0"; // Cũng reset thành tiền dòng
            }
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
            if (dgvChiTietHDN.SelectedRows.Count > 0 && dgvChiTietHDN.CurrentRow != null && dgvChiTietHDN.CurrentRow.DataBoundItem != null)
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

        public static string GenerateMaHDN()
        {
            // Tạo mã theo định dạng: HDN_ddMMyyyy_HHmmss
            return "HDN_" + DateTime.Now.ToString("ddMMyyyy_HHmmss");
        }

        private void btnThemHDN_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem có dữ liệu chưa lưu trên form không (nếu txtMaHDN không rỗng và chưa phải là HĐN cũ đang load)
            if (!string.IsNullOrWhiteSpace(txtMaHDN.Text) && string.IsNullOrEmpty(maHDNToLoad) && dtChiTietHDN.Rows.Count > 0)
            {
                DialogResult confirm = MessageBox.Show("Bạn có hóa đơn đang làm dở. Bạn có muốn hủy hóa đơn này và tạo một hóa đơn mới không?",
                                                       "Xác nhận tạo mới",
                                                       MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.No)
                {
                    return; // Người dùng không muốn hủy, không làm gì cả
                }
            }
            // Nếu là HĐN cũ đang được load (maHDNToLoad có giá trị) và người dùng nhấn Thêm mới
            else if (!string.IsNullOrEmpty(maHDNToLoad))
            {
                DialogResult confirm = MessageBox.Show("Bạn đang xem/sửa một hóa đơn cũ. Bạn có muốn bỏ qua và tạo một hóa đơn mới không?",
                                                      "Xác nhận tạo mới",
                                                      MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.No)
                {
                    return;
                }
            }


            ResetValuesToInitialState(); // Reset form về trạng thái ban đầu

            txtMaHDN.Text = GenerateMaHDN(); // Sinh mã mới

            // Thiết lập trạng thái cho việc nhập hóa đơn mới
            // isEditingOldInvoice là false vì đây là hóa đơn mới
            SetActiveProcessingButtonStates(false);

            dpNgayNhap.Focus();
        }

        private void btnThemDong_Click(object sender, EventArgs e)
        {
            // 1. KIỂM TRA ĐIỀU KIỆN ĐẦU VÀO (như code trước)
            if (string.IsNullOrWhiteSpace(txtMaHDN.Text))
            {
                MessageBox.Show("Vui lòng tạo hoặc chọn một hóa đơn nhập trước khi thêm sản phẩm.", "Chưa có Hóa Đơn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnThemHDN.Focus();
                return;
            }
            if (cboMaSP.SelectedValue == null || cboMaSP.SelectedIndex == -1)
            {
                MessageBox.Show("Bạn phải chọn một sản phẩm để thêm vào hóa đơn.", "Chưa chọn sản phẩm", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboMaSP.Focus();
                return;
            }
            decimal soLuong;
            if (!decimal.TryParse(txtSoLuong.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out soLuong) || soLuong <= 0)
            {
                MessageBox.Show("Số lượng phải là một số dương hợp lệ.", "Số lượng không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSoLuong.Focus();
                txtSoLuong.SelectAll();
                return;
            }
            decimal donGia;
            if (!decimal.TryParse(txtDonGia.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out donGia) || donGia < 0)
            {
                MessageBox.Show("Đơn giá phải là một số không âm hợp lệ.", "Đơn giá không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtDonGia.Focus();
                txtDonGia.SelectAll();
                return;
            }
            decimal thanhTienDong;
            if (!decimal.TryParse(txtThanhTienDong.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out thanhTienDong))
            {
                MessageBox.Show("Thành tiền dòng không hợp lệ. Vui lòng kiểm tra lại Số lượng, Đơn giá và Khuyến mãi.", "Thành tiền không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string maSPDaChon = cboMaSP.SelectedValue.ToString();
            string tenSPDaChon = txtTenSP.Text;
            string khuyenMai = txtKhuyenMai.Text.Trim();

            // 2. KIỂM TRA MÃ SẢN PHẨM TRÙNG (như code trước)
            if (dtChiTietHDN != null)
            {
                foreach (DataRow existingRow in dtChiTietHDN.Rows)
                {
                    if (existingRow.RowState != DataRowState.Deleted &&
                        existingRow["MaSP"].ToString().Equals(maSPDaChon, StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show($"Sản phẩm '{tenSPDaChon}' (Mã: {maSPDaChon}) đã tồn tại trong hóa đơn này.\n" +
                                        "Bạn có thể chọn dòng đó và nhấn 'Sửa dòng' để thay đổi số lượng hoặc thông tin khác.",
                                        "Sản phẩm đã tồn tại", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }

            // 3. THÊM DÒNG MỚI VÀO DATATABLE dtChiTietHDN (như code trước)
            if (dtChiTietHDN == null) { Console.WriteLine("dtChiTietHDN is null"); return; }
            try
            {
                DataRow newRow = dtChiTietHDN.NewRow();
                newRow["MaSP"] = maSPDaChon;
                newRow["TenSP"] = tenSPDaChon;
                newRow["SoLuong"] = soLuong;
                newRow["DonGia"] = donGia;
                newRow["KhuyenMai"] = khuyenMai;
                newRow["ThanhTien"] = thanhTienDong;
                dtChiTietHDN.Rows.Add(newRow);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); return; }

            // 4. CẬP NHẬT TỔNG TIỀN CỦA TOÀN BỘ HÓA ĐƠN
            CapNhatTongTienHoaDon();

            // 5. RESET CÁC CONTROLS NHẬP LIỆU CHI TIẾT SẢN PHẨM
            ResetChiTietSanPhamFields();

            // 6. BỎ CHỌN TẤT CẢ CÁC DÒNG TRONG DATAGRIDVIEW
            // Điều này sẽ kích hoạt sự kiện SelectionChanged, và vì không có dòng nào được chọn,
            // nhánh 'else' của SelectionChanged sẽ chạy, đặt btnThemDong.Enabled = true.
            dgvChiTietHDN.ClearSelection();

            // Các dòng sau không còn cần thiết vì SelectionChanged sẽ xử lý:
            // btnSuaDong.Enabled = false;
            // btnXoaDong.Enabled = false;
            // btnHuyBoDong.Enabled = false;
            // Quan trọng: Phải đảm bảo btnThemDong được bật lại bởi SelectionChanged
            // (xem lại hàm dgvChiTietHDN_SelectionChanged)

            // 7. Đặt focus lại
            cboMaSP.Focus();
        }

        private void btnLuuHDN_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(maHDNToLoad))
            {
                MessageBox.Show("Chức năng sửa hóa đơn cũ chưa được triển khai trong ví dụ này.\n" +
                                "Hàm này hiện tại tập trung vào việc lưu hóa đơn mới.",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // TODO: Triển khai logic SỬA hóa đơn nếu cần.
                // Ví dụ:
                // CapNhatHoaDonCu();
                return;
            }

            // --- BẮT ĐẦU LƯU HÓA ĐƠN MỚI ---

            // 1. KIỂM TRA DỮ LIỆU ĐẦU VÀO CHO THÔNG TIN CHUNG
            string maHDN = txtMaHDN.Text.Trim();
            if (string.IsNullOrWhiteSpace(maHDN))
            {
                MessageBox.Show("Mã hóa đơn nhập không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Nút Thêm HĐN nên đã tạo mã này, trường hợp này ít xảy ra nếu logic đúng.
                btnThemHDN.Focus();
                return;
            }

            if (cboMaNV.SelectedValue == null || cboMaNV.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn nhân viên lập hóa đơn.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cboMaNV.Focus();
                return;
            }
            string maNV = cboMaNV.SelectedValue.ToString();

            if (cboMaNCC.SelectedValue == null || cboMaNCC.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn nhà cung cấp.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cboMaNCC.Focus();
                return;
            }
            string maNCC = cboMaNCC.SelectedValue.ToString();

            DateTime ngayNhap = dpNgayNhap.Value;

            // Kiểm tra xem có ít nhất một dòng chi tiết sản phẩm không
            if (dtChiTietHDN == null || dtChiTietHDN.Rows.Count == 0)
            {
                MessageBox.Show("Hóa đơn nhập phải có ít nhất một chi tiết sản phẩm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnThemDong.Focus(); // Hướng người dùng đến việc thêm dòng chi tiết
                return;
            }

            // Tính lại tổng tiền từ dtChiTietHDN để đảm bảo chính xác trước khi lưu
            CapNhatTongTienHoaDon(); // Hàm này sẽ cập nhật txtTongTien.Text
            decimal tongTienDecimal;
            if (!decimal.TryParse(txtTongTien.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out tongTienDecimal))
            {
                MessageBox.Show("Tổng tiền hóa đơn không hợp lệ. Vui lòng kiểm tra lại chi tiết hóa đơn.", "Lỗi Tổng Tiền", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Không lưu nếu tổng tiền không hợp lệ
            }

            try
            {
                Function.ExecuteTransaction((conn, trans) => // conn và trans được cung cấp bởi ExecuteTransaction
                {
                    // a. Lưu thông tin chung vào bảng HoaDonNhap
                    string sqlInsertHDN = @"INSERT INTO dbo.HoaDonNhap (MaHDN, NgayNhap, MaNV, MaNCC, TongTien, IsDeleted)
                               VALUES (@MaHDN, @NgayNhap, @MaNV, @MaNCC, @TongTien, 0)";
                    SqlParameter[] paramsHDN =
                    {
                        new SqlParameter("@MaHDN", maHDN),
                        new SqlParameter("@NgayNhap", ngayNhap.Date),
                        new SqlParameter("@MaNV", maNV),
                        new SqlParameter("@MaNCC", maNCC),
                        new SqlParameter("@TongTien", tongTienDecimal)
                    };
                    // Gọi hàm ExecuteNonQuery mới, truyền conn và trans vào
                    Function.ExecuteNonQuery(sqlInsertHDN, conn, trans, paramsHDN);


                    // b. Lưu các dòng chi tiết từ dtChiTietHDN vào bảng ChiTietHDN
                    string sqlInsertChiTiet = @"INSERT INTO dbo.ChiTietHDN (MaHDN, MaSP, SoLuong, DonGia, KhuyenMai, ThanhTien)
                                    VALUES (@MaHDN_CT, @MaSP_CT, @SoLuong_CT, @DonGia_CT, @KhuyenMai_CT, @ThanhTien_CT)";
                    foreach (DataRow drChiTiet in dtChiTietHDN.Rows)
                    {
                        if (drChiTiet.RowState == DataRowState.Deleted) continue;

                        SqlParameter[] paramsChiTiet =
                        {
                new SqlParameter("@MaHDN_CT", maHDN),
                new SqlParameter("@MaSP_CT", drChiTiet["MaSP"].ToString()),
                new SqlParameter("@SoLuong_CT", Convert.ToDecimal(drChiTiet["SoLuong"])),
                new SqlParameter("@DonGia_CT", Convert.ToDecimal(drChiTiet["DonGia"])),
                new SqlParameter("@KhuyenMai_CT", drChiTiet["KhuyenMai"] == DBNull.Value ? (object)DBNull.Value : drChiTiet["KhuyenMai"].ToString()),
                new SqlParameter("@ThanhTien_CT", Convert.ToDecimal(drChiTiet["ThanhTien"]))
            };
                        // Gọi hàm ExecuteNonQuery mới
                        Function.ExecuteNonQuery(sqlInsertChiTiet, conn, trans, paramsChiTiet);

                        // c. Cập nhật số lượng tồn kho trong bảng SanPham
                        string sqlUpdateKho = @"UPDATE dbo.SanPham
                                    SET SoLuong = SoLuong + @SoLuongNhap
                                    WHERE MaSP = @MaSPKho";
                        SqlParameter[] paramsKho =
                        {
                new SqlParameter("@SoLuongNhap", Convert.ToDecimal(drChiTiet["SoLuong"])),
                new SqlParameter("@MaSPKho", drChiTiet["MaSP"].ToString())
            };
                        // Gọi hàm ExecuteNonQuery mới
                        Function.ExecuteNonQuery(sqlUpdateKho, conn, trans, paramsKho);
                    }
                }); // Kết thúc Function.ExecuteTransaction

                MessageBox.Show("Lưu hóa đơn nhập thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetValuesToInitialState();
                Function.FillCombo(cboMaHDNSearch, "MaHDN", "MaHDN", "SELECT MaHDN FROM dbo.HoaDonNhap WHERE IsDeleted = 0 ORDER BY NgayNhap DESC, MaHDN DESC");
                cboMaHDNSearch.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi trong quá trình lưu hóa đơn nhập:\n" + ex.Message,
                                "Lỗi Lưu Hóa Đơn", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
