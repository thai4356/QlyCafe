using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;

namespace QlyCafe.Quanly
{
    public partial class SanPham : Form
    {
        public DataTable dtSanPham;
        private int maSPDem = 1;
        private string _hinhAnhGocKhiSua = null;
        public SanPham()
        {
            InitializeComponent();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtTenSP.Text))
            {
                MessageBox.Show("Tên sản phẩm không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenSP.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(cboLoaiSP.Text))
            {
                MessageBox.Show("Vui lòng chọn loại sản phẩm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboLoaiSP.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(cboCongDung.Text))
            {
                MessageBox.Show("Vui lòng chọn công dụng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboCongDung.Focus();
                return false;
            }
            // Kiểm tra giá nhập
            if (string.IsNullOrWhiteSpace(txtGiaNhap.Text) || !decimal.TryParse(txtGiaNhap.Text, out decimal giaNhap))
            {
                MessageBox.Show("Giá nhập phải ở dạng hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGiaNhap.Focus();
                return false;
            }

            // Kiểm tra giá bán
            if (string.IsNullOrWhiteSpace(txtGiaBan.Text) || !decimal.TryParse(txtGiaBan.Text, out decimal giaBan))
            {
                MessageBox.Show("Giá bán phải ở dạng hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGiaBan.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtSoLuong.Text) || !int.TryParse(txtSoLuong.Text, out int soLuong))
            {
                MessageBox.Show("Số lượng phải ở dạng hợp lệ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuong.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Copy file ảnh từ đường dẫn gốc vào thư mục Images của ứng dụng, 
        /// trả về tên file để lưu vào CSDL hoặc null nếu không có ảnh.
        /// </summary>
        private string ProcessImage(string sourceImagePath)
        {
            if (string.IsNullOrWhiteSpace(sourceImagePath))
                return null;

            if (!File.Exists(sourceImagePath))
            {
                MessageBox.Show(
                    $"File ảnh tại '{sourceImagePath}' không tồn tại. Sản phẩm sẽ được lưu không có ảnh.",
                    "Cảnh Báo Ảnh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            string fileName = Path.GetFileName(sourceImagePath);
            string destinationFolder = Path.Combine(Application.StartupPath, "Images");
            if (!Directory.Exists(destinationFolder))
                Directory.CreateDirectory(destinationFolder);

            string destinationPath = Path.Combine(destinationFolder, fileName);
            File.Copy(sourceImagePath, destinationPath, true);
            Console.WriteLine($"Đã copy ảnh: {sourceImagePath} đến {destinationPath}");

            return fileName;
        }

        /// <summary>
        /// Tạo mảng SqlParameter cho bảng SanPham.
        /// keyParamName là tên tham số khóa (ví dụ "@MaSP" hoặc "@MaSPOld"),
        /// keyValue là giá trị tương ứng.
        /// </summary>
        private SqlParameter[] BuildProductParameters(
            string keyParamName, object keyValue,
            string tenSP, string maLoai, decimal giaNhap, decimal giaBan,
            int soLuong, string maCongDung, string hinhAnhForDb)
        {
            return new[]
            {
                new SqlParameter(keyParamName, SqlDbType.VarChar, 10) { Value = keyValue },
                new SqlParameter("@TenSP",     SqlDbType.NVarChar, 100) { Value = tenSP },
                new SqlParameter("@MaLoai",    SqlDbType.VarChar, 10)   { Value = maLoai },
                new SqlParameter("@GiaNhap",   SqlDbType.Decimal)       { Precision = 10, Scale = 2, Value = giaNhap },
                new SqlParameter("@GiaBan",    SqlDbType.Decimal)       { Precision = 10, Scale = 2, Value = giaBan },
                new SqlParameter("@SoLuong",   SqlDbType.Int)           { Value = soLuong },
                new SqlParameter("@MaCongDung",SqlDbType.VarChar, 10)   { Value = maCongDung },
                new SqlParameter("@HinhAnh",   SqlDbType.NVarChar, 255) { Value = (object)hinhAnhForDb ?? DBNull.Value }
            };
        }
        private void SanPham_Load(object sender, EventArgs e)
        {
            txtMaSP.Enabled = false;
            btnLuu.Enabled = false;
            btnBoQua.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;

            LoadDataGridView();

            // Gọi FillCombo từ DataService
            Function.FillCombo(cboLoaiSP, "TenLoai", "MaLoai", "SELECT MaLoai, TenLoai FROM Loai");
            Function.FillCombo(cboCongDung, "TenCongDung", "MaCongDung", "SELECT MaCongDung, TenCongDung FROM CongDung");

            ResetValues();
        }

        private void CustomizeDataGridViewColumn(DataGridView dgvSanPham)
        {
            dgvSanPham.Columns[0].HeaderText = "Mã Sản Phẩm";
            dgvSanPham.Columns[1].HeaderText = "Tên Sản Phẩm";
            dgvSanPham.Columns[2].HeaderText = "Loại";
            dgvSanPham.Columns[3].HeaderText = "Số Lượng";
            dgvSanPham.Columns[4].HeaderText = "Đơn Giá Nhập";
            dgvSanPham.Columns[5].HeaderText = "Đơn Giá Bán";

            dgvSanPham.Columns[0].Width = 85;
            dgvSanPham.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; // Cho phép cột này lấp đầy không gian còn trống
            dgvSanPham.Columns[1].FillWeight = 40; // Ưu tiên độ rộng cho cột này (giá trị tương đối)
            dgvSanPham.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvSanPham.Columns[2].FillWeight = 25; // Độ rộng tương đối, ít hơn Tên SP
            dgvSanPham.Columns[3].Width = 60;    // Độ rộng cố định, nhỏ hơn hiện tại
            dgvSanPham.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; // Căn phải cho số
            dgvSanPham.Columns[4].Width = 100;
            dgvSanPham.Columns[4].DefaultCellStyle.Format = "N0"; // Định dạng số kiểu tiền tệ (vd: 10,000)
            dgvSanPham.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; // Căn phải
            dgvSanPham.Columns[5].Width = 100;
            dgvSanPham.Columns[5].DefaultCellStyle.Format = "N0"; // Định dạng số kiểu tiền tệ
            dgvSanPham.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; // Căn phải

            if (dgvSanPham.Columns["MaLoai"] != null) dgvSanPham.Columns["MaLoai"].Visible = false;
            if (dgvSanPham.Columns["MaCongDung"] != null) dgvSanPham.Columns["MaCongDung"].Visible = false;
            if (dgvSanPham.Columns["HinhAnh"] != null) dgvSanPham.Columns["HinhAnh"].Visible = false;
            if (dgvSanPham.Columns["TenCongDung"] != null) dgvSanPham.Columns["TenCongDung"].Visible = false; // Ví dụ: chỉ tìm kiếm theo TenCongDung, không hiển thị

            dgvSanPham.AllowUserToAddRows = false;
            dgvSanPham.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void LoadDataGridView()
        {
            string sql = @"Select MaSP, TenSP, TenLoai, SoLuong, GiaNhap ,GiaBan from SanPham
                                join Loai on Loai.MaLoai = SanPham.MaLoai";
            dtSanPham = Function.GetDataToTable(sql);
            //Lưu số bản ghi vào biến
            maSPDem = Function.CountRecords("Select Count(MaSP) From SanPham") + 1;
            Console.WriteLine($"SO Ban Ghi Hien Tai: {maSPDem}");
            dgvSanPham.DataSource = dtSanPham;

            CustomizeDataGridViewColumn(dgvSanPham);
        }

        private void LoadDataGridView(string searchFilter)
        {
            string sqlSearch = @"
            SELECT sp.MaSP, sp.TenSP, l.TenLoai, sp.GiaNhap, sp.GiaBan, sp.SoLuong, cd.TenCongDung, sp.HinhAnh
            FROM SanPham sp
            INNER JOIN Loai l ON sp.MaLoai = l.MaLoai
            INNER JOIN CongDung cd ON sp.MaCongDung = cd.MaCongDung
            WHERE sp.MaSP LIKE @SearchPattern
               OR sp.TenSP LIKE @SearchPattern
               OR l.TenLoai LIKE @SearchPattern
               OR cd.TenCongDung LIKE @SearchPattern
               OR sp.GiaNhap LIKE @SearchPattern
               OR sp.GiaBan LIKE @SearchPattern
               OR sp.SoLuong LIKE @SearchPattern";

            SqlParameter param = new SqlParameter("@SearchPattern", SqlDbType.NVarChar)
            {
                Value = "%" + searchFilter + "%" // Thêm ký tự đại diện % để tìm kiếm gần đúng
            };

            DataTable dtSP = Function.GetDataToTable(sqlSearch, param);
            int maSPSearch = dtSP.Rows.Count + 1;
            Console.WriteLine($"SO Ban Ghi tat ca: {maSPDem}");
            Console.WriteLine($"SO Ban Ghi Hien Tai: {maSPSearch}");
            dgvSanPham.DataSource = dtSP;

            CustomizeDataGridViewColumn(dgvSanPham);
        }

        private void ResetValues()
        {
            txtMaSP.Text = "";
            txtTenSP.Text = "";
            cboLoaiSP.Text = "";
            cboCongDung.Text = "";
            txtGiaNhap.Text = "0";
            txtGiaBan.Text = "0";
            txtSoLuong.Text = "0";
            txtDuongDan.Text = "";
            picHinhAnhSP.Image = null;
        }

        private void dgvSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) // Click vào header, không xử lý
                return;

            if (btnThem.Enabled == false) // Giả sử btnThem bị vô hiệu hóa khi đang thêm mới
            {
                MessageBox.Show("Đang ở chế độ thêm mới! Vui lòng hoàn tất hoặc hủy bỏ thao tác.", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (dgvSanPham.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để hiển thị!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataGridViewRow row = dgvSanPham.Rows[e.RowIndex];

            try
            {
                string maSPHienTai = GetCellValueAsString(row.Cells["MaSP"]);
                txtMaSP.Text = maSPHienTai;
                txtTenSP.Text = GetCellValueAsString(row.Cells["TenSP"]);

                // ----- THAY ĐỔI CHO GIÁ NHẬP, GIÁ BÁN, SỐ LƯỢNG -----
                // Giá Nhập (hiển thị vào TextBox txtGiaNhap)
                object giaNhapValue = row.Cells["GiaNhap"].Value;
                if (giaNhapValue != null && giaNhapValue != DBNull.Value)
                {
                    // Định dạng có dấu phẩy ngăn cách hàng nghìn, và 2 số lẻ (nếu có)
                    // Hoặc "N0" nếu bạn muốn hiển thị số nguyên (ví dụ: 10,000)
                    txtGiaNhap.Text = Convert.ToDecimal(giaNhapValue).ToString("N0");
                }
                else
                {
                    txtGiaNhap.Text = "0"; // Hoặc string.Empty
                }

                // Giá Bán (hiển thị vào TextBox txtGiaBan)
                object giaBanValue = row.Cells["GiaBan"].Value;
                if (giaBanValue != null && giaBanValue != DBNull.Value)
                {
                    txtGiaBan.Text = Convert.ToDecimal(giaBanValue).ToString("N0");
                }
                else
                {
                    txtGiaBan.Text = "0"; // Hoặc string.Empty
                }

                // Số Lượng (hiển thị vào TextBox txtSoLuong)
                object soLuongValue = row.Cells["SoLuong"].Value;
                if (soLuongValue != null && soLuongValue != DBNull.Value)
                {
                    txtSoLuong.Text = Convert.ToInt32(soLuongValue).ToString(); // Số nguyên thường không cần format đặc biệt
                }
                else
                {
                    txtSoLuong.Text = "0"; // Hoặc string.Empty
                }
                // ----- KẾT THÚC THAY ĐỔI -----

                // Reset và lấy MaLoai, MaCongDung, HinhAnh từ DB (giữ nguyên logic này)
                cboLoaiSP.SelectedIndex = -1;
                cboCongDung.SelectedIndex = -1;
                txtDuongDan.Text = "";
                picHinhAnhSP.Image = null;

                if (!string.IsNullOrWhiteSpace(maSPHienTai))
                {
                    SqlParameter paramMaSP = new SqlParameter("@MaSP", SqlDbType.VarChar, 10) { Value = maSPHienTai };

                    string sqlGetMaLoai = "SELECT MaLoai FROM dbo.SanPham WHERE MaSP = @MaSP";
                    string maLoaiFromDb = Function.GetFieldValue(sqlGetMaLoai, paramMaSP);
                    if (!string.IsNullOrWhiteSpace(maLoaiFromDb)) SetComboBoxValue(cboLoaiSP, maLoaiFromDb);

                    // Tạo SqlParameter mới hoặc đảm bảo GetFieldValue không giữ state của parameter
                    SqlParameter paramMaSPCongDung = new SqlParameter("@MaSP", SqlDbType.VarChar, 10) { Value = maSPHienTai };
                    string sqlGetMaCongDung = "SELECT MaCongDung FROM dbo.SanPham WHERE MaSP = @MaSP";
                    string maCongDungFromDb = Function.GetFieldValue(sqlGetMaCongDung, paramMaSPCongDung);
                    if (!string.IsNullOrWhiteSpace(maCongDungFromDb)) SetComboBoxValue(cboCongDung, maCongDungFromDb);

                    SqlParameter paramMaSPHinhAnh = new SqlParameter("@MaSP", SqlDbType.VarChar, 10) { Value = maSPHienTai };
                    string sqlGetHinhAnh = "SELECT HinhAnh FROM dbo.SanPham WHERE MaSP = @MaSP";
                    string hinhAnhDbValue = Function.GetFieldValue(sqlGetHinhAnh, paramMaSPHinhAnh);
                    _hinhAnhGocKhiSua = hinhAnhDbValue; // **LƯU LẠI GIÁ TRỊ ẢNH GỐC TỪ DB**
                    txtDuongDan.Text = _hinhAnhGocKhiSua;



                    if (!string.IsNullOrWhiteSpace(_hinhAnhGocKhiSua))
                    {
                        string basePath = Application.StartupPath;
                        string imageRootFolder = Path.Combine(basePath, "Images");
                        string fullPathToImage = Path.Combine(imageRootFolder, _hinhAnhGocKhiSua);
                        try
                        {
                            if (File.Exists(fullPathToImage))
                            {
                                using (FileStream stream = new FileStream(fullPathToImage, FileMode.Open, FileAccess.Read))
                                {
                                    picHinhAnhSP.Image = Image.FromStream(stream);
                                }
                            }
                            else { Console.WriteLine($"File ảnh không tồn tại: {fullPathToImage}"); }
                        }
                        catch (Exception exImg) { Console.WriteLine($"Lỗi tải ảnh '{fullPathToImage}': {exImg.Message}"); }
                    }
                }

                btnSua.Enabled = true;
                btnXoa.Enabled = true;
                btnBoQua.Enabled = true;
                btnThem.Enabled = true;
                btnLuu.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi hiển thị dữ liệu chi tiết sản phẩm: " + ex.Message, "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"Lỗi trong sự kiện CellClick của dgvSanPham: {ex.ToString()}");
            }

        }

        private string GetCellValueAsString(DataGridViewCell cell)
        {
            return cell.Value?.ToString() ?? string.Empty;
        }

        private void SetComboBoxValue(ComboBox cbo, object value)
        {
            if (value != null && value != DBNull.Value)
            {
                try
                {
                    cbo.SelectedValue = value;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi khi set SelectedValue cho ComboBox '{cbo.Name}' với giá trị '{value}': {ex.Message}. Đặt SelectedIndex = -1.");
                    cbo.SelectedIndex = -1;
                }
            }
            else
            {
                cbo.SelectedIndex = -1;
            }
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnBoQua.Enabled = true;
            btnLuu.Enabled = true;
            btnThem.Enabled = false;
            ResetValues();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            if (MessageBox.Show(
                    "Bạn chắc chắn muốn thêm mới sản phẩm này không?",
                    "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                != DialogResult.Yes) return;

            // Chuẩn bị dữ liệu
            string maSP = "SP" + maSPDem.ToString("D3");
            string tenSP = txtTenSP.Text.Trim();
            string maLoai = cboLoaiSP.SelectedValue.ToString();
            string maCongDung = cboCongDung.SelectedValue.ToString();
            decimal giaNhap = decimal.Parse(txtGiaNhap.Text.Replace(",", ""));
            decimal giaBan = decimal.Parse(txtGiaBan.Text.Replace(",", ""));
            int soLuong = int.Parse(txtSoLuong.Text);

            // Xử lý ảnh
            string hinhAnhForDb = ProcessImage(txtDuongDan.Text.Trim());

            // SQL và tham số
            string sql = @"
                        INSERT INTO dbo.SanPham
                            (MaSP, TenSP, MaLoai, GiaNhap, GiaBan, SoLuong, MaCongDung, HinhAnh)
                        VALUES
                            (@MaSP, @TenSP, @MaLoai, @GiaNhap, @GiaBan, @SoLuong, @MaCongDung, @HinhAnh)";
            var parameters = BuildProductParameters(
                "@MaSP", maSP,
                tenSP, maLoai, giaNhap, giaBan, soLuong, maCongDung, hinhAnhForDb);

            // Thực thi
            Function.RunSql(sql, parameters);

            MessageBox.Show("Thêm mới sản phẩm thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

            LoadDataGridView();
            ResetValues();
            btnXoa.Enabled = btnSua.Enabled = btnThem.Enabled = true;
            btnBoQua.Enabled = btnLuu.Enabled = false;
        }

        private void btnChonAnh_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Thiết lập các thuộc tính cho OpenFileDialog
            openFileDialog.Title = "Chọn hình ảnh sản phẩm";
            // Lọc các loại file ảnh phổ biến
            openFileDialog.Filter = "Image Files(*.BMP;*.JPG;*.JPEG;*.GIF;*.PNG)|*.BMP;*.JPG;*.JPEG;*.GIF;*.PNG|All files (*.*)|*.*";
            // Không cho phép chọn nhiều file
            openFileDialog.Multiselect = false;

            // Hiển thị hộp thoại OpenFileDialog và kiểm tra xem người dùng có chọn file không
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Lấy đường dẫn đầy đủ của file ảnh đã chọn
                    string selectedImagePath = openFileDialog.FileName;

                    // Hiển thị đường dẫn này lên TextBox (txtDuongDanAnh)
                    txtDuongDan.Text = selectedImagePath;

                    // Hiển thị ảnh lên PictureBox (picHinhAnhSP)
                    // Để tránh lỗi file đang được sử dụng, nên đọc file vào MemoryStream
                    // rồi mới tạo Image từ MemoryStream đó.
                    // Cách 1: Đơn giản (có thể khóa file nếu không dispose Image đúng cách)
                    // picHinhAnhSP.Image = Image.FromFile(selectedImagePath);

                    // Cách 2: Tốt hơn, không khóa file gốc (khuyến nghị)
                    // Xóa ảnh cũ trong PictureBox trước khi tải ảnh mới (nếu có)
                    if (picHinhAnhSP.Image != null)
                    {
                        picHinhAnhSP.Image.Dispose();
                        picHinhAnhSP.Image = null;
                    }

                    using (FileStream fs = new FileStream(selectedImagePath, FileMode.Open, FileAccess.Read))
                    {
                        picHinhAnhSP.Image = Image.FromStream(fs);
                    }
                }
                catch (System.Security.SecurityException exSec)
                {
                    MessageBox.Show("Lỗi bảo mật.\n\nKhông có quyền đọc file.\nChi tiết: " + exSec.Message,
                                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (OutOfMemoryException exMem) // Thường xảy ra với file ảnh lớn hoặc không hợp lệ
                {
                    MessageBox.Show("Không đủ bộ nhớ để tải ảnh hoặc file không phải là ảnh hợp lệ.\nChi tiết: " + exMem.Message,
                                    "Lỗi Tải Ảnh", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể mở file ảnh đã chọn.\nChi tiết: " + ex.Message,
                                    "Lỗi Mở File Ảnh", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSP.Text))
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào để sửa.", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!ValidateInput()) return;

            if (MessageBox.Show(
                    "Bạn chắc chắn muốn sửa sản phẩm này không?",
                    "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                != DialogResult.Yes) return;

            // Chuẩn bị dữ liệu
            string maSP = txtMaSP.Text.Trim();
            string tenSP = txtTenSP.Text.Trim();
            string maLoai = cboLoaiSP.SelectedValue.ToString();
            string maCongDung = cboCongDung.SelectedValue.ToString();
            decimal giaNhap = decimal.Parse(txtGiaNhap.Text.Replace(",", ""));
            decimal giaBan = decimal.Parse(txtGiaBan.Text.Replace(",", ""));
            int soLuong = int.Parse(txtSoLuong.Text);

            string hinhAnhToSaveInDb = _hinhAnhGocKhiSua; // Mặc định là giữ lại ảnh cũ
            string currentPathInTextBox = txtDuongDan.Text.Trim();

            // Kiểm tra xem người dùng có chọn/thay đổi ảnh không
            // Dấu hiệu là txtDuongDanAnh.Text chứa đường dẫn tuyệt đối (do OpenFileDialog gán vào)
            if (Path.IsPathRooted(currentPathInTextBox))
            {
                // Người dùng đã chọn một file mới từ máy của họ
                // Gọi ProcessImage để xử lý file mới này.
                // ProcessImage sẽ kiểm tra File.Exists, copy file và trả về tên file hoặc null nếu lỗi.
                string newFileName = ProcessImage(currentPathInTextBox);

                if (newFileName != null) // ProcessImage thành công (ảnh mới đã được copy, có tên file mới)
                {
                    hinhAnhToSaveInDb = newFileName;
                    _hinhAnhGocKhiSua = newFileName; // Cập nhật lại ảnh gốc để giao diện nhất quán nếu không tải lại form
                }
                // Nếu newFileName là null (ProcessImage thất bại), thì hinhAnhToSaveInDb vẫn là _hinhAnhGocKhiSua (giữ ảnh cũ).
                // MessageBox lỗi đã được hiển thị bên trong ProcessImage.
            }
            else if (string.IsNullOrWhiteSpace(currentPathInTextBox) && !string.IsNullOrWhiteSpace(_hinhAnhGocKhiSua))
            {
                // Người dùng đã xóa trắng đường dẫn trong TextBox -> muốn xóa ảnh
                hinhAnhToSaveInDb = null;
                _hinhAnhGocKhiSua = null; // Cập nhật ảnh gốc
            }
            // Nếu currentPathInTextBox không phải là đường dẫn tuyệt đối và không rỗng,
            // có nghĩa là nó đang chứa tên file/đường dẫn tương đối gốc (hoặc người dùng tự gõ vào).
            // Trong trường hợp này, hinhAnhToSaveInDb đã được gán _hinhAnhGocKhiSua, và nếu
            // currentPathInTextBox khác _hinhAnhGocKhiSua (do người dùng tự gõ tên file mới),
            // thì hinhAnhToSaveInDb nên lấy giá trị từ currentPathInTextBox.
            else if (!string.IsNullOrWhiteSpace(currentPathInTextBox) && !Path.IsPathRooted(currentPathInTextBox))
            {
                hinhAnhToSaveInDb = currentPathInTextBox; // Lưu lại đường dẫn tương đối người dùng nhập/giữ lại
                _hinhAnhGocKhiSua = currentPathInTextBox;
            }
            // Nếu không rơi vào các trường hợp trên (ví dụ currentPathInTextBox giống _hinhAnhGocKhiSua và không rỗng, không phải rooted path),
            // thì hinhAnhToSaveInDb vẫn giữ nguyên giá trị _hinhAnhGocKhiSua.

            // SQL và tham số
            string sql = @"
            UPDATE dbo.SanPham
                SET TenSP     = @TenSP,
                    MaLoai    = @MaLoai,
                    GiaNhap   = @GiaNhap,
                    GiaBan    = @GiaBan,
                    SoLuong   = @SoLuong,
                    MaCongDung= @MaCongDung,
                    HinhAnh   = @HinhAnh
            WHERE MaSP = @MaSPOld";
            var parameters = BuildProductParameters(
                "@MaSPOld", maSP,
                tenSP, maLoai, giaNhap, giaBan, soLuong, maCongDung, hinhAnhToSaveInDb);

            // Thực thi
            Function.RunSql(sql, parameters);

            MessageBox.Show("Sửa sản phẩm thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

            LoadDataGridView();
            ResetValues();

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            
            if (dtSanPham.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu!", "Thông báo", MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                return;
            }
            if (txtMaSP.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào", "Thông báo",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Bạn có muốn xóa không?", "Thông báo",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                // Lấy mã sản phẩm từ TextBox txtMaSP
                string maSP = txtMaSP.Text.Trim();
                // Tạo câu lệnh SQL DELETE để xóa bản ghi sản phẩm
                string sqlDelete = @"DELETE FROM dbo.SanPham WHERE MaSP = @MaSP";
                // Tạo tham số SqlParameter để truyền giá trị mã sản phẩm vào câu lệnh SQL
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MaSP", SqlDbType.VarChar, 10) { Value = maSP }
                };
                // Thực thi câu lệnh SQL DELETE bằng cách gọi hàm Function.RunSql
                Function.RunSql(sqlDelete, parameters);
                // Hiển thị thông báo xóa thành công
                MessageBox.Show("Xóa sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Load lại dữ liệu vào DataGridView
                LoadDataGridView();
                // Reset giá trị của các TextBox
                ResetValues();
            }

        }

        private void btnBoQua_Click(object sender, EventArgs e)
        {
            ResetValues();
            btnBoQua.Enabled = false;
            btnThem.Enabled = true;
            btnXoa.Enabled = true;
            btnSua.Enabled = true;
            btnLuu.Enabled = false;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // Nếu ô tìm kiếm trống, tải tất cả nhà cung cấp
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                LoadDataGridView();
                return;
            }
            LoadDataGridView(txtSearch.Text);
        }

        private void btnXuatRaExcel_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel Workbook (*.xlsx)|*.xlsx";
            saveFileDialog.Title = "Xuất Toàn Bộ Danh Sách Sản Phẩm";
            saveFileDialog.FileName = "ToanBoSanPham_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                Console.WriteLine($"Bắt đầu xuất toàn bộ sản phẩm ra file: {filePath}");

                try
                {
                    // Câu SQL để lấy toàn bộ thông tin sản phẩm, bao gồm Tên Loại và Tên Công Dụng
                    string sql = @"
                SELECT 
                    sp.MaSP, 
                    sp.TenSP, 
                    l.TenLoai, 
                    cd.TenCongDung, 
                    sp.GiaNhap, 
                    sp.GiaBan, 
                    sp.SoLuong, 
                    sp.HinhAnh
                FROM dbo.SanPham sp
                LEFT JOIN dbo.Loai l ON sp.MaLoai = l.MaLoai
                LEFT JOIN dbo.CongDung cd ON sp.MaCongDung = cd.MaCongDung
                ORDER BY sp.MaSP ASC;"; // Sắp xếp theo MaSP hoặc tiêu chí bạn muốn

                    DataTable dtSanPham = Function.GetDataToTable(sql); // Sử dụng hàm GetDataToTable của bạn

                    if (dtSanPham == null || dtSanPham.Rows.Count == 0)
                    {
                        MessageBox.Show("Không có dữ liệu sản phẩm để xuất.", "Thông báo",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("DanhSachSanPham"); // Tên sheet
                        int currentRow = 1;

                        // --- Định nghĩa và Ghi Tiêu Đề Cột (Headers) ---
                        string[] headers = {
                    "Mã Sản Phẩm", "Tên Sản Phẩm", "Loại Sản Phẩm", "Công Dụng",
                    "Giá Nhập", "Giá Bán", "Số Lượng", "Hình Ảnh (Tên File)"
                };

                        for (int i = 0; i < headers.Length; i++)
                        {
                            worksheet.Cell(currentRow, i + 1).Value = headers[i];
                        }

                        // Định dạng Header
                        var headerRange = worksheet.Range(currentRow, 1, currentRow, headers.Length);
                        headerRange.Style.Font.Bold = true;
                        headerRange.Style.Fill.BackgroundColor = XLColor.LightSkyBlue; // Chọn màu bạn thích
                        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        headerRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                        currentRow++;

                        // --- Ghi Dữ Liệu Các Dòng ---
                        // Ánh xạ tên cột trong DataTable với thứ tự header đã định nghĩa
                        // (nếu câu SQL SELECT theo đúng thứ tự headers thì có thể dùng index trực tiếp)
                        string[] dataTableColumnNames = {
                    "MaSP", "TenSP", "TenLoai", "TenCongDung",
                    "GiaNhap", "GiaBan", "SoLuong", "HinhAnh"
                };

                        foreach (DataRow dataRow in dtSanPham.Rows)
                        {
                            for (int i = 0; i < dataTableColumnNames.Length; i++)
                            {
                                string colName = dataTableColumnNames[i];
                                IXLCell currentCell = worksheet.Cell(currentRow, i + 1);

                                if (dtSanPham.Columns.Contains(colName) && dataRow[colName] != DBNull.Value)
                                {
                                    // Sử dụng XLCellValue.FromObject để giữ kiểu dữ liệu nếu có thể
                                    currentCell.Value = XLCellValue.FromObject(dataRow[colName]);

                                    // Định dạng cho các cột số
                                    if (colName == "GiaNhap" || colName == "GiaBan")
                                    {
                                        currentCell.Style.NumberFormat.Format = "#,##0"; // Hoặc "#,##0.00" nếu muốn 2 số lẻ
                                        currentCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                                    }
                                    else if (colName == "SoLuong")
                                    {
                                        currentCell.Style.NumberFormat.Format = "#,##0";
                                        currentCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                                    }
                                }
                                else
                                {
                                    currentCell.Value = string.Empty; // Hoặc giá trị mặc định khác
                                }
                            }
                            currentRow++;
                        }

                        // Tự động điều chỉnh độ rộng tất cả các cột cho vừa nội dung
                        worksheet.Columns().AdjustToContents();

                        // Lưu file Excel
                        workbook.SaveAs(filePath);
                    } // Workbook sẽ được dispose ở đây

                    MessageBox.Show($"Xuất toàn bộ dữ liệu sản phẩm ra Excel thành công!\nĐường dẫn: {filePath}",
                                    "Xuất Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi xảy ra khi xuất dữ liệu sản phẩm ra Excel: " + ex.Message,
                                    "Lỗi Xuất File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Console.WriteLine($"Lỗi xuất toàn bộ sản phẩm: {ex.ToString()}");
                }
            }
        }

        private void btnNhapTuExcel_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Workbook (*.xlsx)|*.xlsx";
            openFileDialog.Title = "Chọn file Excel chứa danh sách sản phẩm";
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                int importedCount = 0;
                int skippedByDuplicateInDbCount = 0;
                int skippedByMissingLookupCount = 0;
                int errorInRowCount = 0;
                List<string> importDetailsLog = new List<string>();

                string sqlCheckMaSP = "SELECT COUNT(*) FROM dbo.SanPham WHERE MaSP = @MaSP";
                string sqlGetMaLoai = "SELECT MaLoai FROM dbo.Loai WHERE TenLoai = @TenLoai";
                string sqlGetMaCongDung = "SELECT MaCongDung FROM dbo.CongDung WHERE TenCongDung = @TenCongDung";
                string sqlInsertSanPham = @"INSERT INTO dbo.SanPham 
                                    (MaSP, TenSP, MaLoai, GiaNhap, GiaBan, SoLuong, MaCongDung, HinhAnh) 
                                    VALUES 
                                    (@MaSP, @TenSP, @MaLoai, @GiaNhap, @GiaBan, @SoLuong, @MaCongDung, @HinhAnh)";

                Console.WriteLine($"Bắt đầu nhập sản phẩm từ file: {filePath}");
                importDetailsLog.Add($"File: {Path.GetFileName(filePath)}");

                try
                {
                    using (var workbook = new XLWorkbook(filePath))
                    {
                        var worksheet = workbook.Worksheet(1); // Làm việc với sheet đầu tiên
                        if (worksheet == null)
                        {
                            MessageBox.Show("Không tìm thấy sheet làm việc trong file Excel.", "Lỗi File Excel", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        var rows = worksheet.RowsUsed().Skip(1); // Bỏ qua dòng tiêu đề (dòng 1)

                        foreach (var row in rows)
                        {
                            int currentRowNumber = row.RowNumber();
                            try
                            {
                                // Đọc dữ liệu từ các ô theo thứ tự cột giả định
                                string maSP = row.Cell(1).GetFormattedString().Trim();
                                string tenSP = row.Cell(2).GetFormattedString().Trim();
                                string tenLoaiExcel = row.Cell(3).GetFormattedString().Trim();
                                string tenCongDungExcel = row.Cell(4).GetFormattedString().Trim();
                                string giaNhapStr = row.Cell(5).GetFormattedString().Trim();
                                string giaBanStr = row.Cell(6).GetFormattedString().Trim();
                                string soLuongStr = row.Cell(7).GetFormattedString().Trim();
                                string hinhAnh = row.Cell(8).GetFormattedString().Trim();

                                // ----- VALIDATION CƠ BẢN -----
                                if (string.IsNullOrWhiteSpace(maSP))
                                {
                                    importDetailsLog.Add($"Dòng {currentRowNumber}: Lỗi - Mã SP không được để trống. Bỏ qua.");
                                    errorInRowCount++;
                                    continue;
                                }
                                if (string.IsNullOrWhiteSpace(tenSP))
                                {
                                    importDetailsLog.Add($"Dòng {currentRowNumber} (MaSP: {maSP}): Lỗi - Tên SP không được để trống. Bỏ qua.");
                                    errorInRowCount++;
                                    continue;
                                }

                                // Kiểm tra trùng MaSP trong CSDL
                                if (Function.CheckKey(sqlCheckMaSP, new SqlParameter("@MaSP", SqlDbType.VarChar, 10) { Value = maSP }))
                                {
                                    importDetailsLog.Add($"Dòng {currentRowNumber} (MaSP: {maSP}): Bỏ qua - Mã SP đã tồn tại trong CSDL.");
                                    skippedByDuplicateInDbCount++;
                                    continue;
                                }

                                // Lấy MaLoai từ TenLoai
                                string maLoai = null;
                                if (!string.IsNullOrWhiteSpace(tenLoaiExcel))
                                {
                                    maLoai = Function.GetFieldValue(sqlGetMaLoai, new SqlParameter("@TenLoai", SqlDbType.NVarChar, 50) { Value = tenLoaiExcel });
                                    if (string.IsNullOrWhiteSpace(maLoai))
                                    {
                                        importDetailsLog.Add($"Dòng {currentRowNumber} (MaSP: {maSP}): Bỏ qua - Không tìm thấy Mã Loại cho Tên Loại '{tenLoaiExcel}'.");
                                        skippedByMissingLookupCount++;
                                        continue;
                                    }
                                }

                                // Lấy MaCongDung từ TenCongDung
                                string maCongDung = null;
                                if (!string.IsNullOrWhiteSpace(tenCongDungExcel))
                                {
                                    maCongDung = Function.GetFieldValue(sqlGetMaCongDung, new SqlParameter("@TenCongDung", SqlDbType.NVarChar, 100) { Value = tenCongDungExcel });
                                    if (string.IsNullOrWhiteSpace(maCongDung))
                                    {
                                        importDetailsLog.Add($"Dòng {currentRowNumber} (MaSP: {maSP}): Bỏ qua - Không tìm thấy Mã Công Dụng cho Tên Công Dụng '{tenCongDungExcel}'.");
                                        skippedByMissingLookupCount++;
                                        continue;
                                    }
                                }

                                // Parse và validate các giá trị số
                                decimal giaNhap, giaBan;
                                int soLuong;
                                if (!decimal.TryParse(giaNhapStr.Replace(",", ""), out giaNhap) || giaNhap < 0)
                                {
                                    importDetailsLog.Add($"Dòng {currentRowNumber} (MaSP: {maSP}): Lỗi - Giá nhập '{giaNhapStr}' không hợp lệ. Bỏ qua.");
                                    errorInRowCount++;
                                    continue;
                                }
                                if (!decimal.TryParse(giaBanStr.Replace(",", ""), out giaBan) || giaBan < 0)
                                {
                                    importDetailsLog.Add($"Dòng {currentRowNumber} (MaSP: {maSP}): Lỗi - Giá bán '{giaBanStr}' không hợp lệ. Bỏ qua.");
                                    errorInRowCount++;
                                    continue;
                                }
                                if (!int.TryParse(soLuongStr, out soLuong) || soLuong < 0)
                                {
                                    importDetailsLog.Add($"Dòng {currentRowNumber} (MaSP: {maSP}): Lỗi - Số lượng '{soLuongStr}' không hợp lệ. Bỏ qua.");
                                    errorInRowCount++;
                                    continue;
                                }

                                // ----- INSERT VÀO CSDL -----
                                SqlParameter[] parameters = new SqlParameter[] {
                                    new SqlParameter("@MaSP", SqlDbType.VarChar, 10) { Value = maSP },
                                    new SqlParameter("@TenSP", SqlDbType.NVarChar, 100) { Value = tenSP },
                                    new SqlParameter("@MaLoai", SqlDbType.VarChar, 10) { Value = (object)maLoai ?? DBNull.Value },
                                    new SqlParameter("@GiaNhap", SqlDbType.Decimal) { Precision = 10, Scale = 2, Value = giaNhap },
                                    new SqlParameter("@GiaBan", SqlDbType.Decimal) { Precision = 10, Scale = 2, Value = giaBan },
                                    new SqlParameter("@SoLuong", SqlDbType.Int) { Value = soLuong },
                                    new SqlParameter("@MaCongDung", SqlDbType.VarChar, 10) { Value = (object)maCongDung ?? DBNull.Value },
                                    new SqlParameter("@HinhAnh", SqlDbType.NVarChar, 255) { Value = string.IsNullOrWhiteSpace(hinhAnh) ? DBNull.Value : (object)hinhAnh }
                                };
                                Function.RunSql(sqlInsertSanPham, parameters);
                                importedCount++;
                                importDetailsLog.Add($"Dòng {currentRowNumber} (MaSP: {maSP}): Nhập thành công.");
                            }
                            catch (Exception exRow)
                            {
                                importDetailsLog.Add($"Dòng {currentRowNumber}: Lỗi nghiêm trọng khi xử lý - {exRow.Message}. Bỏ qua.");
                                errorInRowCount++;
                                Console.WriteLine($"Lỗi chi tiết dòng {currentRowNumber}: {exRow.ToString()}");
                            }
                        }
                    }

                    // Hiển thị thông báo kết quả chi tiết
                    System.Text.StringBuilder summary = new System.Text.StringBuilder();
                    summary.AppendLine("Hoàn tất quá trình nhập sản phẩm từ Excel.");
                    summary.AppendLine("-----------------------------------------");
                    summary.AppendLine($"- Số sản phẩm được nhập thành công: {importedCount}");
                    summary.AppendLine($"- Số sản phẩm bị bỏ qua (do trùng Mã SP trong CSDL): {skippedByDuplicateInDbCount}");
                    summary.AppendLine($"- Số sản phẩm bị bỏ qua (do không tìm thấy Mã Loại/Công Dụng): {skippedByMissingLookupCount}");
                    summary.AppendLine($"- Số dòng trong Excel gặp lỗi dữ liệu hoặc xử lý: {errorInRowCount}");
                    summary.AppendLine("-----------------------------------------");
                    if (importDetailsLog.Count > 1) // Có chi tiết để hiển thị (dòng đầu là tên file)
                    {
                        summary.AppendLine("Chi tiết (một vài dòng đầu và cuối nếu quá dài):");
                        int logLimit = 20; // Giới hạn số dòng log chi tiết hiển thị trong MessageBox
                        if (importDetailsLog.Count <= logLimit + 1)
                        {
                            for (int i = 1; i < importDetailsLog.Count; i++) summary.AppendLine(importDetailsLog[i]);
                        }
                        else
                        {
                            for (int i = 1; i <= logLimit / 2; i++) summary.AppendLine(importDetailsLog[i]);
                            summary.AppendLine("...");
                            for (int i = importDetailsLog.Count - (logLimit / 2); i < importDetailsLog.Count; i++) summary.AppendLine(importDetailsLog[i]);
                        }
                        summary.AppendLine("\n(Xem Console Output để có đầy đủ chi tiết nếu cần)");
                    }

                    MessageBox.Show(summary.ToString(), "Kết Quả Nhập Sản Phẩm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Console.WriteLine("--- Toàn bộ Log Nhập Sản Phẩm ---");
                    foreach (var logEntry in importDetailsLog) Console.WriteLine(logEntry);
                    Console.WriteLine("--- Kết Thúc Log ---");


                    LoadDataGridView(); // Tải lại DataGridView
                }
                catch (Exception exFile)
                {
                    MessageBox.Show($"Có lỗi nghiêm trọng xảy ra khi đọc hoặc xử lý file Excel: {exFile.Message}", "Lỗi File Excel", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Console.WriteLine($"Lỗi đọc file Excel nghiêm trọng: {exFile.ToString()}");
                }
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}


