using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveCharts.Wpf;
using LiveCharts;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.IO;
using ClosedXML.Excel;

using System.Windows.Media;          // Cần tham chiếu đến PresentationCore.dll
using System.Windows.Media.Imaging;  // Cần tham chiếu đến PresentationCore.dll
namespace QlyCafe.Quanly
{
    public partial class BaoCaoTonKho : Form
    {
        private DataTable dtInventoryReport;
        public BaoCaoTonKho()
        {
            InitializeComponent();
        }

        private void BaoCaoTonKho_Load(object sender, EventArgs e)
        {
            // 1. Thiết lập giá trị ban đầu cho các control lọc
            LoadFilterControls();

            // 2. Tải dữ liệu ban đầu cho DataGridView
            LoadAndDisplayInventoryData();

            // 3. Tải dữ liệu cho các biểu đồ
            LoadAllCharts();
        }

        private void LoadFilterControls()
        {
            // Tải dữ liệu cho ComboBox Loại Sản Phẩm
            string sqlLoaiSP = "SELECT MaLoai, TenLoai FROM Loai ORDER BY TenLoai";
            DataTable dtLoai = Function.GetDataToTable(sqlLoaiSP);

            // Tạo một dòng "Tất cả"
            DataRow dr = dtLoai.NewRow();
            dr["MaLoai"] = DBNull.Value; // Hoặc một giá trị đặc biệt bạn có thể kiểm tra, ví dụ ""
            dr["TenLoai"] = "Tất cả";
            dtLoai.Rows.InsertAt(dr, 0);

            Function.FillCombo(cboLoaiSanPhamFilter, "TenLoai", "MaLoai", dtLoai); // Sử dụng overload của FillCombo
            cboLoaiSanPhamFilter.SelectedIndex = 0; // Chọn "Tất cả" làm mặc định

            // Đặt giá trị mặc định cho ngưỡng sắp hết hàng
            txtNguongSapHet.Text = "10"; // Như trong FormQuanLy.cs
        }

        private void LoadAndDisplayInventoryData(string maLoaiFilter = null, int? soLuongNguongFilter = null)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(@"
            SELECT
                sp.MaSP,
                sp.TenSP,
                l.TenLoai,
                sp.MaLoai, 
                sp.SoLuong,
                sp.GiaNhap,
                (ISNULL(sp.SoLuong, 0) * ISNULL(sp.GiaNhap, 0)) AS TongGiaTriTon
            FROM SanPham sp
            LEFT JOIN Loai l ON sp.MaLoai = l.MaLoai");

            List<SqlParameter> parameters = new List<SqlParameter>();
            List<string> whereClauses = new List<string>();

            if (!string.IsNullOrEmpty(maLoaiFilter))
            {
                whereClauses.Add("sp.MaLoai = @MaLoai");
                parameters.Add(new SqlParameter("@MaLoai", maLoaiFilter));
            }

            if (soLuongNguongFilter.HasValue)
            {
                whereClauses.Add("sp.SoLuong < @SoLuongNguong");
                parameters.Add(new SqlParameter("@SoLuongNguong", soLuongNguongFilter.Value));
            }

            if (whereClauses.Count > 0)
            {
                sqlBuilder.Append(" WHERE ");
                sqlBuilder.Append(string.Join(" AND ", whereClauses));
            }

            sqlBuilder.Append(" ORDER BY sp.TenSP;");

            // In ra câu lệnh SQL và tham số để debug (tùy chọn)
            // Console.WriteLine("Executing SQL for Grid: " + sqlBuilder.ToString());
            // foreach (var p in parameters) { Console.WriteLine($"Param: {p.ParameterName} = {p.Value}"); }

            dtInventoryReport = Function.GetDataToTable(sqlBuilder.ToString(), parameters.ToArray());
            dgvBaoCaoTonKho.DataSource = dtInventoryReport; // Gán dữ liệu mới (đã lọc)

            CustomizeInventoryGridColumns(); // Áp dụng định dạng và tô màu
            UpdateSummaryLabels();         // Cập nhật tổng kết dựa trên dtInventoryReport mới
        }

        private void CustomizeInventoryGridColumns()
        {
            if (dgvBaoCaoTonKho.Columns.Count == 0) return;

            // Kiểm tra sự tồn tại của cột trước khi truy cập
            if (dgvBaoCaoTonKho.Columns["MaSP"] != null)
                dgvBaoCaoTonKho.Columns["MaSP"].HeaderText = "Mã SP";

            if (dgvBaoCaoTonKho.Columns["TenSP"] != null)
            {
                dgvBaoCaoTonKho.Columns["TenSP"].HeaderText = "Tên Sản Phẩm";
                dgvBaoCaoTonKho.Columns["TenSP"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            if (dgvBaoCaoTonKho.Columns["TenLoai"] != null) // SỬ DỤNG "TenLoai"
            {
                dgvBaoCaoTonKho.Columns["TenLoai"].HeaderText = "Loại SP";
                dgvBaoCaoTonKho.Columns["TenLoai"].Width = 200; // SỬA Ở ĐÂY
            }

            if (dgvBaoCaoTonKho.Columns["SoLuong"] != null)
            {
                dgvBaoCaoTonKho.Columns["SoLuong"].HeaderText = "SL Tồn";
                dgvBaoCaoTonKho.Columns["SoLuong"].Width = 80;
                dgvBaoCaoTonKho.Columns["SoLuong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            if (dgvBaoCaoTonKho.Columns["GiaNhap"] != null)
            {
                dgvBaoCaoTonKho.Columns["GiaNhap"].HeaderText = "Giá Nhập";
                dgvBaoCaoTonKho.Columns["GiaNhap"].Width = 120;
                dgvBaoCaoTonKho.Columns["GiaNhap"].DefaultCellStyle.Format = "N0";
                dgvBaoCaoTonKho.Columns["GiaNhap"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            if (dgvBaoCaoTonKho.Columns["TongGiaTriTon"] != null)
            {
                dgvBaoCaoTonKho.Columns["TongGiaTriTon"].HeaderText = "Tổng Giá Trị";
                dgvBaoCaoTonKho.Columns["TongGiaTriTon"].Width = 150;
                dgvBaoCaoTonKho.Columns["TongGiaTriTon"].DefaultCellStyle.Format = "N0";
                dgvBaoCaoTonKho.Columns["TongGiaTriTon"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }


            // Tô màu cho hàng sắp hết (ví dụ)
            int threshold = 10;
            if (int.TryParse(txtNguongSapHet.Text, out int parsedThreshold))
            {
                threshold = parsedThreshold;
            }
            else
            {
                MessageBox.Show("Giá trị ngưỡng sắp hết không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtNguongSapHet.Text = "10";
                txtNguongSapHet.Focus();
                return;
            }

            // Đảm bảo cột "SoLuong" tồn tại trước khi lặp
            if (dgvBaoCaoTonKho.Columns["SoLuong"] != null)
            {
                foreach (DataGridViewRow row in dgvBaoCaoTonKho.Rows) // Lặp qua các dòng đang hiển thị trong grid
                {
                    if (row.IsNewRow) continue;

                    // Reset màu nền về mặc định trước
                    row.DefaultCellStyle.BackColor = dgvBaoCaoTonKho.DefaultCellStyle.BackColor;
                    row.DefaultCellStyle.ForeColor = dgvBaoCaoTonKho.DefaultCellStyle.ForeColor;


                    if (row.Cells["SoLuong"].Value != null && row.Cells["SoLuong"].Value != DBNull.Value)
                    {
                        try
                        {
                            if (Convert.ToInt32(row.Cells["SoLuong"].Value) < threshold)
                            {
                                row.DefaultCellStyle.BackColor = System.Drawing.Color.LightCoral;
                                // row.DefaultCellStyle.ForeColor = Color.White; // Tùy chọn
                            }
                        }
                        catch (FormatException) { /* Bỏ qua nếu không thể convert */ }
                    }
                }
            }


            dgvBaoCaoTonKho.AllowUserToAddRows = false;
            dgvBaoCaoTonKho.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgvBaoCaoTonKho.ReadOnly = true;
        }

        private void UpdateSummaryLabels()
        {
            if (dtInventoryReport == null)
            {
                lblTongSoMatHang.Text = "0";
                lblTongGiaTriTonKho.Text = "0 VNĐ";
                return;
            }

            lblTongSoMatHang.Text = dtInventoryReport.Rows.Count.ToString();

            decimal totalValue = 0;
            foreach (DataRow row in dtInventoryReport.Rows)
            {
                if (row["TongGiaTriTon"] != DBNull.Value)
                {
                    totalValue += Convert.ToDecimal(row["TongGiaTriTon"]);
                }
            }
            lblTongGiaTriTonKho.Text = totalValue.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + " VNĐ";
        }

        private void LoadAllCharts(string maLoaiFilter = null, int? soLuongNguongFilter = null)
        {
            LoadCategoryValueChart(maLoaiFilter, soLuongNguongFilter);
            LoadTopValueProductsChart(maLoaiFilter, soLuongNguongFilter);
        }

        private void LoadCategoryValueChart(string maLoaiFilter = null, int? soLuongNguongFilter = null)
        {
            try
            {
                StringBuilder sqlBuilder = new StringBuilder();
                sqlBuilder.Append(@"
                SELECT
                    l.TenLoai,
                    SUM(ISNULL(sp.SoLuong, 0) * ISNULL(sp.GiaNhap, 0)) AS TongGiaTri
                FROM SanPham sp
                JOIN Loai l ON sp.MaLoai = l.MaLoai");

                List<SqlParameter> parameters = new List<SqlParameter>();
                List<string> whereClauses = new List<string>();

                // Luôn có điều kiện giá trị > 0
                whereClauses.Add("(ISNULL(sp.SoLuong, 0) * ISNULL(sp.GiaNhap, 0)) > 0");

                if (!string.IsNullOrEmpty(maLoaiFilter))
                {
                    whereClauses.Add("sp.MaLoai = @MaLoaiChart");
                    parameters.Add(new SqlParameter("@MaLoaiChart", maLoaiFilter));
                }

                if (soLuongNguongFilter.HasValue)
                {
                    whereClauses.Add("sp.SoLuong < @SoLuongNguongChart");
                    parameters.Add(new SqlParameter("@SoLuongNguongChart", soLuongNguongFilter.Value));
                }

                if (whereClauses.Count > 0)
                {
                    sqlBuilder.Append(" WHERE ");
                    sqlBuilder.Append(string.Join(" AND ", whereClauses));
                }

                sqlBuilder.Append(@"
                GROUP BY l.TenLoai
                HAVING SUM(ISNULL(sp.SoLuong, 0) * ISNULL(sp.GiaNhap, 0)) > 0
                ORDER BY TongGiaTri DESC;");

                // Console.WriteLine("SQL DEBUG (LoadCategoryValueChart): " + sqlBuilder.ToString());

                DataTable dtChart = Function.GetDataToTable(sqlBuilder.ToString(), parameters.ToArray());

                SeriesCollection pieSeriesCollection = new SeriesCollection();
                if (dtChart != null && dtChart.Rows.Count > 0)
                {
                    foreach (DataRow row in dtChart.Rows)
                    {
                        pieSeriesCollection.Add(new PieSeries
                        {
                            Title = row["TenLoai"].ToString(),
                            Values = new ChartValues<decimal> { Convert.ToDecimal(row["TongGiaTri"]) },
                            DataLabels = true,
                            LabelPoint = chartPoint => string.Format("{0} ({1:P})", chartPoint.Y.ToString("N0"), chartPoint.Participation)
                        });
                    }
                }
                pieChartGiaTriTheoLoai.Series = pieSeriesCollection;
                pieChartGiaTriTheoLoai.LegendLocation = LegendLocation.Right;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error LoadCategoryValueChart: " + ex.Message);
                if (pieChartGiaTriTheoLoai.Series != null) pieChartGiaTriTheoLoai.Series.Clear();
            }
        }

        private void LoadTopValueProductsChart(string maLoaiFilter = null, int? soLuongNguongFilter = null)
        {
            try
            {
                StringBuilder sqlBuilder = new StringBuilder();
                // Sử dụng WITH để lọc trước rồi mới TOP 5, đảm bảo TOP 5 là từ tập đã lọc
                sqlBuilder.Append(@"
                ;WITH FilteredProducts AS (
                    SELECT
                        sp.TenSP,
                        (ISNULL(sp.SoLuong, 0) * ISNULL(sp.GiaNhap, 0)) AS GiaTriTonKho
                    FROM SanPham sp
                    LEFT JOIN Loai l ON sp.MaLoai = l.MaLoai"); // Join Loai phòng khi cần dùng trong WHERE

                List<SqlParameter> parameters = new List<SqlParameter>();
                List<string> whereClauses = new List<string>();

                whereClauses.Add("(ISNULL(sp.SoLuong, 0) * ISNULL(sp.GiaNhap, 0)) > 0");

                if (!string.IsNullOrEmpty(maLoaiFilter))
                {
                    whereClauses.Add("sp.MaLoai = @MaLoaiChart");
                    parameters.Add(new SqlParameter("@MaLoaiChart", maLoaiFilter));
                }

                if (soLuongNguongFilter.HasValue)
                {
                    whereClauses.Add("sp.SoLuong < @SoLuongNguongChart");
                    parameters.Add(new SqlParameter("@SoLuongNguongChart", soLuongNguongFilter.Value));
                }

                if (whereClauses.Count > 0)
                {
                    sqlBuilder.Append(" WHERE ");
                    sqlBuilder.Append(string.Join(" AND ", whereClauses));
                }
                sqlBuilder.Append(@"
                )
                SELECT TOP 5 TenSP, GiaTriTonKho 
                FROM FilteredProducts
                ORDER BY GiaTriTonKho DESC;");

                // Console.WriteLine("SQL DEBUG (LoadTopValueProductsChart): " + sqlBuilder.ToString());

                DataTable dtChart = Function.GetDataToTable(sqlBuilder.ToString(), parameters.ToArray());

                var productLabels = new List<string>();
                var productValues = new ChartValues<decimal>();

                if (dtChart != null && dtChart.Rows.Count > 0)
                {
                    for (int i = dtChart.Rows.Count - 1; i >= 0; i--) // Đảo ngược để thanh cao nhất ở trên
                    {
                        DataRow row = dtChart.Rows[i];
                        productLabels.Add(row["TenSP"].ToString());
                        productValues.Add(Convert.ToDecimal(row["GiaTriTonKho"]));
                    }
                }

                chartTopSPGiaTriTon.Series = new SeriesCollection
                {
                    new RowSeries
                    {
                        Title = "Giá trị tồn",
                        Values = productValues,
                        DataLabels = true,
                        LabelPoint = chartPoint => chartPoint.X.ToString("N0")
                    }
                };
                // Cập nhật trục Y cho CartesianChart
                if (chartTopSPGiaTriTon.AxisY.Count > 0) chartTopSPGiaTriTon.AxisY[0].Labels = productLabels;
                else chartTopSPGiaTriTon.AxisY.Add(new Axis { Labels = productLabels });

                if (chartTopSPGiaTriTon.AxisY.FirstOrDefault() != null) chartTopSPGiaTriTon.AxisY.FirstOrDefault().Title = "Sản phẩm";

                // Cấu hình trục X
                if (chartTopSPGiaTriTon.AxisX.Count == 0)
                {
                    chartTopSPGiaTriTon.AxisX.Add(new Axis { Title = "Giá trị tồn kho (VNĐ)", LabelFormatter = value => value.ToString("N0"), MinValue = 0 });
                }
                else
                {
                    chartTopSPGiaTriTon.AxisX[0].Title = "Giá trị tồn kho (VNĐ)";
                    chartTopSPGiaTriTon.AxisX[0].LabelFormatter = value => value.ToString("N0");
                    chartTopSPGiaTriTon.AxisX[0].MinValue = 0;
                }
                chartTopSPGiaTriTon.LegendLocation = LegendLocation.None;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error LoadTopValueProductsChart: " + ex.Message);
                if (chartTopSPGiaTriTon.Series != null) chartTopSPGiaTriTon.Series.Clear();
            }
        }

        private void btnApDungLoc_Click(object sender, EventArgs e)
        {
            string maLoaiSelected = null;
            if (cboLoaiSanPhamFilter.SelectedValue != null && cboLoaiSanPhamFilter.SelectedValue != DBNull.Value && !string.IsNullOrEmpty(cboLoaiSanPhamFilter.SelectedValue.ToString()))
            {
                maLoaiSelected = cboLoaiSanPhamFilter.SelectedValue.ToString();
            }

            int? nguongSoLuongSelected = null;
            if (chkHienThiHangSapHet.Checked)
            {
                if (int.TryParse(txtNguongSapHet.Text, out int nguong) && nguong >= 0)
                {
                    nguongSoLuongSelected = nguong;
                }
                else
                {
                    MessageBox.Show("Ngưỡng sắp hết không hợp lệ. Vui lòng nhập một số nguyên không âm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtNguongSapHet.Focus();
                    // Có thể cập nhật txtNguongSapHet.Text = "10";
                    return; // Dừng nếu ngưỡng không hợp lệ khi checkbox được chọn
                }
            }

            LoadAndDisplayInventoryData(maLoaiSelected, nguongSoLuongSelected);
            LoadAllCharts(maLoaiSelected, nguongSoLuongSelected); // << Gọi với tham số lọc
        }

        private void btnLamMoiDuLieu_Click(object sender, EventArgs e)
        {
            // 1. Đặt lại các điều khiển lọc về trạng thái mặc định
            if (cboLoaiSanPhamFilter.Items.Count > 0)
            {
                cboLoaiSanPhamFilter.SelectedIndex = 0; // Chọn "Tất cả", giả sử nó ở vị trí đầu tiên
            }
            chkHienThiHangSapHet.Checked = false;
            txtNguongSapHet.Text = "10"; // Đặt lại ngưỡng mặc định

            // 2. Tải lại dữ liệu không lọc cho DataGridView và cập nhật các thành phần liên quan
            LoadAndDisplayInventoryData(); // Gọi hàm này mà không có tham số để tải tất cả dữ liệu

            // 3. Tải lại dữ liệu tổng quan (không lọc) cho các biểu đồ
            LoadAllCharts(); // Gọi hàm này mà không có tham số
        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            if (dtInventoryReport == null || dtInventoryReport.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel Workbook (*.xlsx)|*.xlsx";
            saveFileDialog.Title = "Lưu Báo cáo Tồn kho";
            saveFileDialog.FileName = "BaoCaoTonKho_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                try
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("BaoCaoTonKho");
                        int currentRow = 1;

                        // 1. Tiêu đề báo cáo và thông tin chung
                        worksheet.Cell(currentRow, 1).Value = "BÁO CÁO TỒN KHO";
                        worksheet.Range(currentRow, 1, currentRow, 6).Merge().Style.Font.Bold = true;
                        worksheet.Range(currentRow, 1, currentRow, 6).Style.Font.FontSize = 16;
                        worksheet.Range(currentRow, 1, currentRow, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        currentRow += 2;

                        worksheet.Cell(currentRow, 1).Value = "Ngày xuất báo cáo:";
                        worksheet.Cell(currentRow, 2).Value = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                        currentRow++;

                        // 2. Thông tin bộ lọc đã áp dụng
                        worksheet.Cell(currentRow, 1).Value = "Bộ lọc áp dụng:";
                        worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                        currentRow++;
                        worksheet.Cell(currentRow, 2).Value = "Loại sản phẩm:"; // Tên mô tả cho cột
                        worksheet.Cell(currentRow, 3).Value = cboLoaiSanPhamFilter.Text; // Giá trị bộ lọc
                        currentRow++;
                        worksheet.Cell(currentRow, 2).Value = "Chỉ hiển thị hàng sắp hết:"; // Tên mô tả cho cột
                        worksheet.Cell(currentRow, 3).Value = chkHienThiHangSapHet.Checked ? "Có" : "Không"; // Giá trị bộ lọc
                        if (chkHienThiHangSapHet.Checked)
                        {
                            worksheet.Cell(currentRow, 4).Value = "(Ngưỡng: " + txtNguongSapHet.Text + ")";
                        }
                        currentRow += 2;

                        // 3. Thông tin tóm tắt (Đã sửa lại thứ tự)
                        worksheet.Cell(currentRow, 1).Value = "Thông tin tóm tắt:";
                        worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                        currentRow++;

                        worksheet.Cell(currentRow, 2).Value = label3.Text; // Mô tả: "Tổng số mặt hàng:"
                        worksheet.Cell(currentRow, 3).Value = lblTongSoMatHang.Text; // Giá trị
                        currentRow++;

                        worksheet.Cell(currentRow, 2).Value = label4.Text; // Mô tả: "Tổng giá trị tồn kho:"
                        worksheet.Cell(currentRow, 3).Value = lblTongGiaTriTonKho.Text; // Giá trị
                        currentRow += 2;

                        // 4. Header cho bảng dữ liệu
                        string[] headers = { "Mã SP", "Tên Sản Phẩm", "Loại SP", "SL Tồn", "Giá Nhập (VNĐ)", "Tổng Giá Trị (VNĐ)" };
                        for (int i = 0; i < headers.Length; i++)
                        {
                            worksheet.Cell(currentRow, i + 1).Value = headers[i];
                            worksheet.Cell(currentRow, i + 1).Style.Font.Bold = true;
                            worksheet.Cell(currentRow, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                            worksheet.Cell(currentRow, i + 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        }
                        currentRow++;

                        // 5. Dữ liệu chi tiết (từ dtInventoryReport đã được lọc)
                        string[] columnNamesFromDataTable = { "MaSP", "TenSP", "TenLoai", "SoLuong", "GiaNhap", "TongGiaTriTon" };

                        foreach (DataRow dataRow in dtInventoryReport.Rows)
                        {
                            for (int i = 0; i < columnNamesFromDataTable.Length; i++)
                            {
                                IXLCell cell = worksheet.Cell(currentRow, i + 1);
                                string colName = columnNamesFromDataTable[i];

                                if (dataRow[colName] != DBNull.Value)
                                {
                                    if (colName == "GiaNhap" || colName == "TongGiaTriTon")
                                    {
                                        cell.Value = Convert.ToDecimal(dataRow[colName]);
                                        cell.Style.NumberFormat.Format = "#,##0";
                                    }
                                    else if (colName == "SoLuong")
                                    {
                                        cell.Value = Convert.ToInt32(dataRow[colName]);
                                        cell.Style.NumberFormat.Format = "#,##0";
                                    }
                                    else
                                    {
                                        cell.Value = dataRow[colName].ToString();
                                    }
                                }
                                else
                                {
                                    cell.Value = string.Empty;
                                }
                                cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            }
                            currentRow++;
                        }
                        worksheet.Columns().AdjustToContents(); // Tự động điều chỉnh độ rộng cột

                        // Bỏ qua phần 6. Chèn hình ảnh biểu đồ

                        workbook.SaveAs(filePath);
                    }

                    MessageBox.Show($"Xuất báo cáo tồn kho ra Excel thành công!\nĐường dẫn: {filePath}",
                                    "Xuất Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi xảy ra khi xuất dữ liệu ra Excel: " + ex.Message,
                                    "Lỗi Xuất File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Console.WriteLine($"Lỗi xuất Excel: {ex.ToString()}");
                }
            }
        }
    }
}

