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
using DocumentFormat.OpenXml.Wordprocessing;

namespace QlyCafe.Quanly
{
    public partial class SanPham : Form
    {
        public DataTable dtSanPham;
        public SanPham()
        {
            InitializeComponent();
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

            dgvSanPham.AllowUserToAddRows = false;
            dgvSanPham.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void LoadDataGridView()
        {
            string sql = @"Select MaSP, TenSP, TenLoai, SoLuong, GiaNhap ,GiaBan from SanPham
                                join Loai on Loai.MaLoai = SanPham.MaLoai";
            dtSanPham = Function.GetDataToTable(sql);
            dgvSanPham.DataSource = dtSanPham;

            CustomizeDataGridViewColumn(dgvSanPham);
        }

        private void ResetValues()
        {
            txtMaSP.Text = "";
            txtTenSP.Text = "";
            cboLoaiSP.Text = "";
            cboCongDung.Text = "";
            numGiaNhap.Value = 0;
            numGiaBan.Value = 0;
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
                // 1. Lấy MaSP từ dòng được chọn (quan trọng cho việc lấy các trường khác từ DB)
                string maSPHienTai = GetCellValueAsString(row.Cells["MaSP"]);
                txtMaSP.Text = maSPHienTai;

                // 2. Hiển thị các thông tin có sẵn trên Grid
                txtTenSP.Text = GetCellValueAsString(row.Cells["TenSP"]);
                // Cột "Loại" trong Grid chỉ để hiển thị Tên Loại, không dùng để set SelectedValue trực tiếp
                // Giá trị cho các NumericUpDown vẫn lấy từ Grid nếu có
                SetNumericUpDownValue(numGiaNhap, row.Cells["GiaNhap"].Value); // Giả sử cột GiaNhap có trong Grid
                SetNumericUpDownValue(numGiaBan, row.Cells["GiaBan"].Value);   // Giả sử cột GiaBan có trong Grid
                SetNumericUpDownValue(numSoLuong, row.Cells["SoLuong"].Value); // Giả sử cột SoLuong có trong Grid


                // Reset ComboBoxes và PictureBox trước khi lấy giá trị mới
                cboLoaiSP.SelectedIndex = -1;
                cboCongDung.SelectedIndex = -1;
                txtDuongDan.Text = "";
                picHinhAnhSP.Image = null;

                if (!string.IsNullOrWhiteSpace(maSPHienTai))
                {
                    SqlParameter paramMaSP = new SqlParameter("@MaSP", SqlDbType.VarChar, 10) { Value = maSPHienTai };

                    // 3. Lấy MaLoai từ CSDL bằng GetFieldValue và hiển thị lên ComboBox
                    string sqlGetMaLoai = "SELECT MaLoai FROM dbo.SanPham WHERE MaSP = @MaSP";
                    string maLoaiFromDb = Function.GetFieldValue(sqlGetMaLoai, paramMaSP);
                    if (!string.IsNullOrWhiteSpace(maLoaiFromDb))
                    {
                        SetComboBoxValue(cboLoaiSP, maLoaiFromDb);
                    }

                    // 4. Lấy MaCongDung từ CSDL bằng GetFieldValue và hiển thị lên ComboBox
                    // (Tạo lại SqlParameter vì nó đã được dùng ở trên, hoặc clear Parameters của command trong GetFieldValue nếu command được tái sử dụng)
                    // Để đơn giản, ta tạo mới hoặc bạn có thể điều chỉnh GetFieldValue để nhận MaSP trực tiếp nếu tiện.
                    // Hoặc, nếu GetFieldValue không giữ lại parameters giữa các lần gọi thì không sao.
                    // An toàn nhất là tạo SqlParameter mới nếu không chắc chắn.
                    SqlParameter paramMaSPForCongDung = new SqlParameter("@MaSP", SqlDbType.VarChar, 10) { Value = maSPHienTai };
                    string sqlGetMaCongDung = "SELECT MaCongDung FROM dbo.SanPham WHERE MaSP = @MaSP";
                    string maCongDungFromDb = Function.GetFieldValue(sqlGetMaCongDung, paramMaSPForCongDung);
                    if (!string.IsNullOrWhiteSpace(maCongDungFromDb))
                    {
                        SetComboBoxValue(cboCongDung, maCongDungFromDb);
                    }

                    // 5. Lấy đường dẫn ảnh từ CSDL bằng GetFieldValue và hiển thị
                    SqlParameter paramMaSPForHinhAnh = new SqlParameter("@MaSP", SqlDbType.VarChar, 10) { Value = maSPHienTai };
                    string sqlGetHinhAnh = "SELECT HinhAnh FROM dbo.SanPham WHERE MaSP = @MaSP";
                    string hinhAnhDbValue = Function.GetFieldValue(sqlGetHinhAnh, paramMaSPForHinhAnh);
                    txtDuongDan.Text = hinhAnhDbValue;

                    if (!string.IsNullOrWhiteSpace(hinhAnhDbValue))
                    {
                        string basePath = Application.StartupPath;
                        string imageRootFolder = Path.Combine(basePath, "Images"); // THAY THẾ bằng thư mục Images của bạn
                        string fullPathToImage = Path.Combine(imageRootFolder, hinhAnhDbValue);

                        try
                        {
                            if (File.Exists(fullPathToImage))
                            {
                                picHinhAnhSP.Image = Image.FromFile(fullPathToImage);
                            }
                            else
                            {
                                Console.WriteLine($"File ảnh không tồn tại tại đường dẫn đã ghép: {fullPathToImage}");
                            }
                        }
                        catch (Exception exImg)
                        {
                            Console.WriteLine($"Lỗi khi tải ảnh '{fullPathToImage}': {exImg.Message}");
                        }
                    }
                }

                // 6. Cập nhật trạng thái các nút
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

        private void SetNumericUpDownValue(NumericUpDown num, object value)
        {
            if (value != null && value != DBNull.Value)
            {
                try
                {
                    decimal decValue = Convert.ToDecimal(value);
                    if (decValue < num.Minimum) decValue = num.Minimum;
                    if (decValue > num.Maximum) decValue = num.Maximum;
                    num.Value = decValue;
                }
                catch
                {
                    num.Value = num.Minimum;
                }
            }
            else
            {
                num.Value = num.Minimum;
            }
        }
    }
}
