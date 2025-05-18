using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClosedXML.Excel;
using System.IO;

namespace QlyCafe.Quanly
{
    public partial class NhaCungCap : Form
    {
        DataTable dtNCC;
        private int maNCCDem = 1;
        public NhaCungCap()
        {
            InitializeComponent();
        }

        private void NhaCungCap_Load(object sender, EventArgs e)
        {
            txtMaNCC.Enabled = false;
            btnLuu.Enabled = false;
            btnBoqua.Enabled = false;
            LoadDataGridView();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtTenNhaCC.Text))
            {
                MessageBox.Show("Tên nhà cung cấp không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenNhaCC.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDiaChi.Text))
            {
                MessageBox.Show("Địa chỉ không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDiaChi.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtSoDienThoai.Text))
            {
                MessageBox.Show("Số điện thoại không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoDienThoai.Focus();
                return false;
            }

            // Kiểm tra định dạng số điện thoại
            // Nếu Muốn chuyển định dạng điện thoại sang "(999) 000-0000"
            // thay Chuỗi regex thành: @"\(\d{3}\) \d{3}-\d{4}"
            if (!Regex.IsMatch(txtSoDienThoai.Text, @"^0\d{9}$"))
            {
                MessageBox.Show("Số điện thoại không đúng định dạng! Số điện thoại phải ở định dạng như 0987111222", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoDienThoai.Focus();
                return false;
            }

            return true;
        }

        private void CustomizeDataGridViewColumn(DataGridView dgv)
        {
            dgv.Columns["MaNCC"].HeaderText = "Mã Nhà Cung Cấp";
            dgv.Columns["TenNCC"].HeaderText = "Tên Nhà Cung Cấp";
            dgv.Columns["Diachi"].HeaderText = "Địa chỉ";
            dgv.Columns["SDT"].HeaderText = "Số Điện Thoại";

            dgv.Columns["MaNCC"].HeaderText = "Mã Nhà Cung Cấp";
            dgv.Columns["MaNCC"].Width = 90; // Độ rộng cố định cho Mã NCC

            dgv.Columns["TenNCC"].HeaderText = "Tên Nhà Cung Cấp";
            dgv.Columns["TenNCC"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["TenNCC"].FillWeight = 35; // Phân bổ độ rộng (tỷ lệ 35%)

            dgv.Columns["DiaChi"].HeaderText = "Địa Chỉ";
            dgv.Columns["DiaChi"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["DiaChi"].FillWeight = 45; // Phân bổ độ rộng (tỷ lệ 45%, rộng hơn TenNCC)

            dgv.Columns["SDT"].HeaderText = "Số Điện Thoại";
            dgv.Columns["SDT"].Width = 110; // Độ rộng cố định cho Số Điện Thoại

            dgv.AllowUserToAddRows = false;
            dgv.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void LoadDataGridView()
        {
            string sql = "SELECT MaNCC,TenNCC,Diachi,SDT FROM NhaCungCap";
            dtNCC = Function.GetDataToTable(sql);
            //Lưu số bản ghi vào biến
            maNCCDem = Function.CountRecords("Select Count(MaNCC) From NhaCungCap") + 1;
            Console.WriteLine($"SO Ban Ghi Hien Tai: {maNCCDem}");
            DataGridViewNCC.DataSource = dtNCC;
            CustomizeDataGridViewColumn(DataGridViewNCC);
        }

        private void LoadDataGridView(string searchFilter)
        {
            string sqlSearch = @"SELECT MaNCC, TenNCC, DiaChi, SDT
                       FROM dbo.NhaCungCap
                       WHERE MaNCC LIKE @SearchPattern
                          OR TenNCC LIKE @SearchPattern
                          OR DiaChi LIKE @SearchPattern
                          OR SDT LIKE @SearchPattern";

            // Sử dụng NVarChar vì TenNCC và DiaChi là NVARCHAR, và nó cũng hoạt động tốt khi so sánh với VARCHAR(MaNCC, SDT)
            SqlParameter param = new SqlParameter("@SearchPattern", SqlDbType.NVarChar)
            {
                Value = "%" + searchFilter + "%" // Thêm ký tự đại diện % để tìm kiếm gần đúng
            };

            DataTable dtNhaCungCap = Function.GetDataToTable(sqlSearch, param);
            int maNCCSearch = dtNhaCungCap.Rows.Count + 1;
            Console.WriteLine($"SO Ban Ghi tat ca: {maNCCDem}");
            Console.WriteLine($"SO Ban Ghi Hien Tai: {maNCCSearch}");
            DataGridViewNCC.DataSource = dtNhaCungCap;

            CustomizeDataGridViewColumn(DataGridViewNCC);
        }

        private void DataGridViewNCC_Click(object sender, EventArgs e)
        {
            if (btnThem.Enabled == false)
            {
                MessageBox.Show("Đang ở chế độ thêm mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (DataGridViewNCC.CurrentRow == null || DataGridViewNCC.CurrentRow.Index == -1 || dtNCC == null || dtNCC.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu hoặc chưa chọn dòng nào.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int rowIndex = DataGridViewNCC.CurrentRow.Index;
            txtMaNCC.Text = DataGridViewNCC.Rows[rowIndex].Cells["MaNCC"].Value?.ToString();
            txtTenNhaCC.Text = DataGridViewNCC.Rows[rowIndex].Cells["TenNCC"].Value?.ToString();

            txtDiaChi.Text = DataGridViewNCC.Rows[rowIndex].Cells["DiaChi"].Value?.ToString();
            txtSoDienThoai.Text = DataGridViewNCC.Rows[rowIndex].Cells["SDT"].Value?.ToString();


            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnBoqua.Enabled = true;
            btnLuu.Enabled = false;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnBoqua.Enabled = true;
            btnLuu.Enabled = true;
            btnThem.Enabled = false;
            ResetValues();
            txtTenNhaCC.Focus();

        }

        private void ResetValues()
        {
            txtMaNCC.Text = string.Empty;
            txtTenNhaCC.Text = string.Empty;
            txtDiaChi.Text = string.Empty;
            txtSoDienThoai.Text = string.Empty;
            txtSearch.Text = string.Empty;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {

            if (!ValidateInput())
            {
                return;
            }

            // Do mã Nhà cụng cấp sẽ tự động sinh nên việc kiểm tra mã trùng là không cần thiết cho trường hợp này


            if (MessageBox.Show("Bạn chắc chắn muốn thêm mới nhà cung cấp này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string maNCC = "NCC" + maNCCDem.ToString("D3");
                string tenNCC = txtTenNhaCC.Text.Trim();
                string diaChi = txtDiaChi.Text.Trim();
                string soDienThoai = txtSoDienThoai.Text.Trim();

                string sqlInsert = "INSERT INTO dbo.NhaCungCap (MaNCC, TenNCC, DiaChi, SDT) VALUES (@MaNCC, @TenNCC, @DiaChi, @SDT)";

                Function.RunSql(sqlInsert, new SqlParameter[] {
                    // MaNCC là VARCHAR(10)
                    new SqlParameter("@MaNCC", SqlDbType.VarChar, 10) { Value = maNCC },
                    // TenNCC là NVARCHAR(50) - Quan trọng cho tiếng Việt
                    new SqlParameter("@TenNCC", SqlDbType.NVarChar, 50) { Value = tenNCC },
                    // DiaChi là NVARCHAR(100) - Quan trọng cho tiếng Việt
                    new SqlParameter("@DiaChi", SqlDbType.NVarChar, 100) { Value = diaChi },
                    // SDT là VARCHAR(15)
                    new SqlParameter("@SDT", SqlDbType.VarChar, 15) { Value = soDienThoai }
                });

                MessageBox.Show("Thêm mới nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDataGridView();
                ResetValues();

                btnSua.Enabled = true;
                btnXoa.Enabled = true;
                btnBoqua.Enabled = true;
                btnLuu.Enabled = false;
            }

            else
            {
                MessageBox.Show("Thêm mới nhà cung cấp thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

        }

        private void btnBoqua_Click(object sender, EventArgs e)
        {
            ResetValues();
            btnXoa.Enabled = true;
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnBoqua.Enabled = false;
            btnLuu.Enabled = false;
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaNCC.Text))
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!ValidateInput())
            {
                return;
            }
            if (MessageBox.Show("Bạn chắc chắn muốn sửa nhà cung cấp này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string maNCC = txtMaNCC.Text;
                string tenNCC = txtTenNhaCC.Text;
                string diaChi = txtDiaChi.Text;
                string soDienThoai = txtSoDienThoai.Text;

                string sqlUpdate = "UPDATE NhaCungCap SET TenNCC = @TenNCC, DiaChi = @DiaChi, SDT = @SDT WHERE MaNCC = @MaNCC";
                Function.RunSql(sqlUpdate, new SqlParameter[] {
                    // MaNCC là VARCHAR(10)
                    new SqlParameter("@MaNCC", SqlDbType.VarChar, 10) { Value = maNCC },
                    // TenNCC là NVARCHAR(50) - Quan trọng cho tiếng Việt
                    new SqlParameter("@TenNCC", SqlDbType.NVarChar, 50) { Value = tenNCC },
                    // DiaChi là NVARCHAR(100) - Quan trọng cho tiếng Việt
                    new SqlParameter("@DiaChi", SqlDbType.NVarChar, 100) { Value = diaChi },
                    // SDT là VARCHAR(15)
                    new SqlParameter("@SDT", SqlDbType.VarChar, 15) { Value = soDienThoai }
                });

                MessageBox.Show("Sửa nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDataGridView();
                ResetValues();
            }
            else
            {
                MessageBox.Show("Sửa nhà cung cấp thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaNCC.Text))
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa bản ghi này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string sql = "DELETE FROM NhaCungCap WHERE MaNCC = @MaNCC";
                // Gọi RunSql từ DataService
                Function.RunSql(sql, new SqlParameter("@MaNCC", txtMaNCC.Text.Trim()));
                MessageBox.Show("Xóa Nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadDataGridView();
                ResetValues();
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                btnBoqua.Enabled = false;
                btnThem.Enabled = true;
            }
        }

        /// <summary>
        /// Tìm kiếm Nhà Cung Cấp trên nhiều cột và tải vào DataGridView.
        /// </summary>
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
            // Lấy DataTable từ DataSource của DataGridView
            // DataTable này chứa dữ liệu hiện đang hiển thị trên lưới (có thể đã được lọc)
            DataTable dt = DataGridViewNCC.DataSource as DataTable;

            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất ra Excel.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Tạo SaveFileDialog để người dùng chọn nơi lưu file
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel Workbook (*.xlsx)|*.xlsx"; // Chỉ cho phép lưu file .xlsx
            saveFileDialog.Title = "Lưu danh sách nhà cung cấp";
            saveFileDialog.FileName = "DanhSachNhaCungCap_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx"; // Tên file gợi ý

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                try
                {
                    using (var workbook = new XLWorkbook()) // Tạo một Excel workbook mới
                    {
                        var worksheet = workbook.Worksheets.Add("Nhà Cung Cấp"); // Thêm một sheet mới tên là "Nhà Cung Cấp"

                        // Ghi tên các cột (Headers) - Lấy từ HeaderText của DataGridView
                        for (int i = 0; i < DataGridViewNCC.Columns.Count; i++)
                        {
                            // Ô trong Excel bắt đầu từ 1 (không phải 0)
                            worksheet.Cell(1, i + 1).Value = DataGridViewNCC.Columns[i].HeaderText;
                            // Có thể thêm style cho header ở đây nếu muốn, ví dụ:
                            // worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                            // worksheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                        }

                        // Ghi dữ liệu từ DataTable vào các hàng tiếp theo
                        for (int i = 0; i < dt.Rows.Count; i++) // Duyệt qua các hàng của DataTable
                        {
                            for (int j = 0; j < dt.Columns.Count; j++) // Duyệt qua các cột của DataTable
                            {
                                // Hàng trong worksheet bắt đầu từ 2 (vì hàng 1 là header)
                                // Cột trong worksheet bắt đầu từ 1
                                // XLCellValue.FromObject cố gắng giữ đúng kiểu dữ liệu (số ra số, text ra text)
                                worksheet.Cell(i + 2, j + 1).Value = XLCellValue.FromObject(dt.Rows[i][j]);
                            }
                        }

                        // Tự động điều chỉnh độ rộng của các cột cho vừa với nội dung
                        worksheet.Columns().AdjustToContents();

                        // Lưu workbook vào đường dẫn đã chọn
                        workbook.SaveAs(filePath);
                    } // Workbook sẽ được dispose ở đây

                    MessageBox.Show("Xuất dữ liệu ra Excel thành công!\nĐường dẫn: " + filePath, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi xảy ra khi xuất dữ liệu ra Excel: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnNhapTuExcel_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Workbook (*.xlsx)|*.xlsx";
            openFileDialog.Title = "Chọn file Excel để nhập dữ liệu Nhà Cung Cấp";
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                Console.WriteLine($"--- BẮT ĐẦU QUÁ TRÌNH NHẬP TỪ FILE EXCEL ---");
                Console.WriteLine($"Đường dẫn file: {filePath}");

                int importedCount = 0;
                int skippedCount = 0;
                int errorCount = 0;
                
                string sqlInsert = "INSERT INTO dbo.NhaCungCap (MaNCC, TenNCC, DiaChi, SDT) VALUES (@MaNCC, @TenNCC, @DiaChi, @SDT)";
                string sqlCheckKey = "SELECT COUNT(*) FROM dbo.NhaCungCap WHERE MaNCC = @MaNCC";

                try
                {
                    using (var workbook = new XLWorkbook(filePath))
                    {
                        var worksheet = workbook.Worksheet(1);
                        if (worksheet == null)
                        {
                            string noSheetMsg = "Không tìm thấy sheet nào trong file Excel.";
                            Console.WriteLine($"LỖI FILE: {noSheetMsg}");
                            MessageBox.Show(noSheetMsg, "Lỗi File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        Console.WriteLine($"Đang làm việc với Sheet: {worksheet.Name}");
                        var rows = worksheet.RowsUsed().Skip(1); // Bỏ qua dòng header

                        Console.WriteLine("Bắt đầu duyệt các dòng dữ liệu...");
                        foreach (var row in rows)
                        {
                            Console.WriteLine($"Đang xử lý dòng Excel số: {row.RowNumber()}");
                            try
                            {
                                string maNCC = row.Cell(1).GetFormattedString().Trim();
                                string tenNCC = row.Cell(2).GetFormattedString().Trim();
                                string diaChi = row.Cell(3).GetFormattedString().Trim();
                                string sdt = row.Cell(4).GetFormattedString().Trim();

                                Console.WriteLine($"  Dữ liệu thô: MaNCC='{maNCC}', TenNCC='{tenNCC}', DiaChi='{diaChi}', SDT='{sdt}'");

                                if (string.IsNullOrWhiteSpace(maNCC))
                                {
                                    errorCount++;
                                    Console.WriteLine($"  LỖI DÒNG {row.RowNumber()}: Mã NCC rỗng. Bỏ qua dòng này.");
                                    continue;
                                }

                                SqlParameter pCheckMaNCC = new SqlParameter("@MaNCC", SqlDbType.VarChar, 10) { Value = maNCC };
                                bool keyExists = Function.CheckKey(sqlCheckKey, pCheckMaNCC);

                                if (keyExists)
                                {
                                    skippedCount++;
                                    Console.WriteLine($"  BỎ QUA DÒNG {row.RowNumber()}: Mã NCC '{maNCC}' đã tồn tại.");
                                }
                                else
                                {
                                    SqlParameter[] insertParams = new SqlParameter[] {
                                        new SqlParameter("@MaNCC", SqlDbType.VarChar, 10) { Value = maNCC },
                                        new SqlParameter("@TenNCC", SqlDbType.NVarChar, 50) { Value = tenNCC },
                                        new SqlParameter("@DiaChi", SqlDbType.NVarChar, 100) { Value = diaChi },
                                        new SqlParameter("@SDT", SqlDbType.VarChar, 15) { Value = sdt }
                                    };
                                    Function.RunSql(sqlInsert, insertParams);
                                    importedCount++;
                                    Console.WriteLine($"  NHẬP THÀNH CÔNG DÒNG {row.RowNumber()}: Mã NCC '{maNCC}'.");
                                }
                            }
                            catch (Exception exRow)
                            {
                                errorCount++;
                                Console.WriteLine($"  LỖI DÒNG {row.RowNumber()}: Xảy ra ngoại lệ - {exRow.Message}");
                                // Bạn có thể log chi tiết hơn về exRow.StackTrace nếu cần gỡ lỗi sâu
                                // Console.WriteLine($"    Stack Trace: {exRow.StackTrace}");
                            }
                        } // Kết thúc vòng lặp duyệt row
                        Console.WriteLine("Kết thúc duyệt các dòng dữ liệu.");
                    } // Workbook được dispose

                    // Hiển thị thông báo kết quả (vẫn giữ MessageBox cho người dùng cuối)
                    StringBuilder summaryForMessageBox = new StringBuilder();
                    summaryForMessageBox.AppendLine($"Hoàn tất nhập dữ liệu từ file: {Path.GetFileName(filePath)}");
                    summaryForMessageBox.AppendLine($"Số bản ghi được nhập thành công: {importedCount}");
                    summaryForMessageBox.AppendLine($"Số bản ghi bị bỏ qua (do trùng Mã NCC): {skippedCount}");
                    summaryForMessageBox.AppendLine($"Số dòng gặp lỗi khi xử lý: {errorCount}");
                    summaryForMessageBox.AppendLine("\n(Chi tiết đã được ghi vào Console Output của Visual Studio khi chạy Debug)");

                    MessageBox.Show(summaryForMessageBox.ToString(), "Kết Quả Nhập Liệu", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Log tóm tắt ra Console
                    Console.WriteLine("--- TÓM TẮT KẾT QUẢ NHẬP LIỆU ---");
                    Console.WriteLine($"File đã xử lý: {Path.GetFileName(filePath)}");
                    Console.WriteLine($"Số bản ghi được nhập thành công: {importedCount}");
                    Console.WriteLine($"Số bản ghi bị bỏ qua (do trùng Mã NCC): {skippedCount}");
                    Console.WriteLine($"Số dòng gặp lỗi khi xử lý: {errorCount}");
                    Console.WriteLine("--- KẾT THÚC QUÁ TRÌNH NHẬP ---");


                    // Tải lại dữ liệu trên DataGridView
                    LoadDataGridView();
                }
                catch (Exception exFile)
                {
                    string fileErrorMsg = $"Có lỗi nghiêm trọng xảy ra khi đọc hoặc xử lý file Excel: {exFile.Message}";
                    Console.WriteLine($"LỖI FILE EXCEL NGHIÊM TRỌNG: {fileErrorMsg}");
                    // Console.WriteLine($"  Stack Trace: {exFile.StackTrace}");
                    MessageBox.Show(fileErrorMsg, "Lỗi File Excel", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}


