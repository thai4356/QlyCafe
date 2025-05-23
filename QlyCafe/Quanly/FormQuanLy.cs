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

namespace QlyCafe.Quanly
{
    public partial class FormQuanLy : Form
    {
        public FormQuanLy()
        {
            InitializeComponent();

            UpdateDateTimeLabels();

            // Nếu bạn muốn thời gian cập nhật liên tục, hãy sử dụng một Timer
            Timer timer = new Timer();
            timer.Interval = 1000; // Cập nhật mỗi giây
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void FormQuanLy_Load(object sender, EventArgs e)
        {
            LoadKPIs();
            LoadAllCharts();
            LoadQuickViewData();
            // Bạn có thể thêm Timer ở đây để refresh KPIs định kỳ nếu muốn
            Timer kpiTimer = new Timer();
            kpiTimer.Interval = 60000; // Ví dụ: cập nhật mỗi 60 giây
            kpiTimer.Tick += KpiTimer_Tick;
            kpiTimer.Start();
        }

        private void KpiTimer_Tick(object sender, EventArgs e)
        {
            LoadKPIs();
            LoadAllCharts();
            LoadQuickViewData();
        }

        private void LoadKPIs()
        {
            try
            {
                // 1. Doanh Thu Hôm Nay
                string sqlDoanhThu = "SELECT SUM(TongTien) FROM HoaDonBan WHERE IsDeleted = 0 AND CONVERT(date, NgayBan) = CONVERT(date, GETDATE())";
                object resultDoanhThu = Function.GetFieldValue(sqlDoanhThu); // Sử dụng GetFieldValue
                if (resultDoanhThu != null && resultDoanhThu != DBNull.Value)
                {
                    lblDoanhThuHomNay.Text = Convert.ToDecimal(resultDoanhThu).ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + " VNĐ";
                }
                else
                {
                    lblDoanhThuHomNay.Text = "0 VNĐ";
                }

                // 2. Tổng Số Hóa Đơn Bán Hôm Nay
                string sqlSoHDB = "SELECT COUNT(MaHDB) FROM HoaDonBan WHERE IsDeleted = 0 AND CONVERT(date, NgayBan) = CONVERT(date, GETDATE())";
                object resultSoHDB = Function.GetFieldValue(sqlSoHDB);
                if (resultSoHDB != null && resultSoHDB != DBNull.Value)
                {
                    lblSoHDBanHomNay.Text = Convert.ToInt32(resultSoHDB).ToString() + " Hóa đơn";
                }
                else
                {
                    lblSoHDBanHomNay.Text = "0 Hóa đơn";
                }

                // 3. Số Bàn Đang Phục Vụ
                string sqlSoBanPhucVu = "SELECT COUNT(id) FROM Ban WHERE status = N'Đang phục vụ'";
                object resultSoBanPhucVu = Function.GetFieldValue(sqlSoBanPhucVu);
                if (resultSoBanPhucVu != null && resultSoBanPhucVu != DBNull.Value)
                {
                    lblSoBanPhucVu.Text = Convert.ToInt32(resultSoBanPhucVu).ToString() + " Bàn";
                }
                else
                {
                    lblSoBanPhucVu.Text = "0 Bàn";
                }

                // 4. Sản Phẩm Bán Chạy Nhất Hôm Nay (Theo Số Lượng)
                string sqlSPBanChay = @"SELECT TOP 1 sp.TenSP 
                                       FROM ChiTietHDB ct
                                       JOIN SanPham sp ON ct.MaSP = sp.MaSP
                                       JOIN HoaDonBan hdb ON ct.MaHDB = hdb.MaHDB
                                       WHERE hdb.IsDeleted = 0 AND CONVERT(date, hdb.NgayBan) = CONVERT(date, GETDATE())
                                       GROUP BY sp.MaSP, sp.TenSP
                                       ORDER BY SUM(ct.SoLuong) DESC";
                object resultSPBanChay = Function.GetFieldValue(sqlSPBanChay);
                if (resultSPBanChay != null && resultSPBanChay != DBNull.Value)
                {
                    lblSPBanChay.Text = resultSPBanChay.ToString();
                }
                else
                {
                    lblSPBanChay.Text = "Chưa có";
                }

                // 5. Số Mặt Hàng Sắp Hết Hàng (ví dụ: số lượng < 10)
                int nguongSapHetHang = 10; // Bạn có thể đặt ngưỡng này ở đâu đó cấu hình được
                string sqlSPSapHet = "SELECT COUNT(MaSP) FROM SanPham WHERE SoLuong < @Nguong";
                object resultSPSapHet = Function.GetFieldValue(sqlSPSapHet, new System.Data.SqlClient.SqlParameter("@Nguong", nguongSapHetHang));
                if (resultSPSapHet != null && resultSPSapHet != DBNull.Value)
                {
                    lblSoMatHangHetHang.Text = Convert.ToInt32(resultSPSapHet).ToString() + " Mặt hàng";
                }
                else
                {
                    lblSoMatHangHetHang.Text = "0 Mặt hàng";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu KPI: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Đặt giá trị mặc định cho các label nếu có lỗi
                lblDoanhThuHomNay.Text = "Lỗi tải";
                lblSoHDBanHomNay.Text = "Lỗi tải";
                lblSoBanPhucVu.Text = "Lỗi tải";
                lblSPBanChay.Text = "Lỗi tải";
                lblSoMatHangHetHang.Text = "Lỗi tải";
            }
        }

        private void LoadAllCharts()
        {
            LoadRevenueOverTimeChart();
            LoadTopProductsChart();
            LoadEmployeePerformanceChart();
            LoadCategoryRevenueChart();
        }

        // --- BIỂU ĐỒ 1: DOANH THU THEO THỜI GIAN (LINE CHART) ---
        private void LoadRevenueOverTimeChart()
        {
            // Tham chiếu trực tiếp đến control chart đã đặt tên trong Designer
            LiveCharts.WinForms.CartesianChart currentChart = this.chartRevenueOverTime;
            if (currentChart != null)
            {
                try
                {
                    string sql = @"
                        SELECT CONVERT(date, NgayBan) AS Ngay, SUM(TongTien) AS TongDoanhThu
                        FROM HoaDonBan
                        WHERE IsDeleted = 0 AND NgayBan >= DATEADD(day, -6, CONVERT(date, GETDATE())) 
                              AND NgayBan < DATEADD(day, 1, CONVERT(date, GETDATE()))
                        GROUP BY CONVERT(date, NgayBan)
                        ORDER BY Ngay ASC"; //
                    DataTable dt = Function.GetDataToTable(sql);

                    var dayLabels = new List<string>();
                    var revenueValues = new ChartValues<decimal>();

                    // Tạo dữ liệu cho 7 ngày, kể cả ngày không có doanh thu
                    Dictionary<DateTime, decimal> dailyRevenue = new Dictionary<DateTime, decimal>();
                    for (int i = 0; i <= 6; i++)
                    {
                        dailyRevenue[DateTime.Today.AddDays(-i)] = 0;
                    }

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            dailyRevenue[Convert.ToDateTime(row["Ngay"])] = Convert.ToDecimal(row["TongDoanhThu"]);
                        }
                    }

                    foreach (var item in dailyRevenue.OrderBy(kvp => kvp.Key))
                    {
                        dayLabels.Add(item.Key.ToString("dd/MM"));
                        revenueValues.Add(item.Value);
                    }

                    currentChart.Series = new SeriesCollection
                    {
                        new LineSeries
                        {
                            Title = "Doanh thu",
                            Values = revenueValues,
                            DataLabels = true,
                            PointGeometrySize = 10
                        }
                    };
                    currentChart.AxisX.Clear();
                    currentChart.AxisX.Add(new Axis { Title = "Ngày", Labels = dayLabels, Separator = new Separator { Step = 1, IsEnabled = false } });
                    currentChart.AxisY.Clear();
                    currentChart.AxisY.Add(new Axis { Title = "Doanh thu (VNĐ)", LabelFormatter = value => value.ToString("N0") });
                }
                catch (Exception ex) { Console.WriteLine("Error LoadRevenueOverTimeChart: " + ex.Message); currentChart.Series.Clear(); }
            }
        }

        // --- BIỂU ĐỒ 2: TOP 5 SẢN PHẨM BÁN CHẠY (HORIZONTAL BAR CHART) ---
        private void LoadTopProductsChart()
        {
            LiveCharts.WinForms.CartesianChart currentChart = this.chartTopProducts;
            if (currentChart != null)
            {
                try
                {
                    string sql = @"
                        SELECT TOP 5 sp.TenSP, SUM(ct.ThanhTien) AS TongDoanhThuSP
                        FROM ChiTietHDB ct
                        JOIN SanPham sp ON ct.MaSP = sp.MaSP
                        JOIN HoaDonBan hdb ON ct.MaHDB = hdb.MaHDB
                        WHERE hdb.IsDeleted = 0 AND hdb.NgayBan >= DATEADD(month, -1, CONVERT(date, GETDATE())) -- Doanh thu trong vòng 1 tháng gần nhất
                        GROUP BY sp.MaSP, sp.TenSP
                        HAVING SUM(ct.ThanhTien) > 0
                        ORDER BY TongDoanhThuSP DESC"; //
                    DataTable dt = Function.GetDataToTable(sql);

                    var productLabels = new List<string>();
                    var productValues = new ChartValues<decimal>();

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            productLabels.Add(row["TenSP"].ToString());
                            productValues.Add(Convert.ToDecimal(row["TongDoanhThuSP"]));
                        }
                    }

                    currentChart.Series = new SeriesCollection
                    {
                        new RowSeries
                        {
                            Title = "Doanh thu",
                            Values = productValues,
                            DataLabels = true
                        }
                    };
                    currentChart.AxisY.Clear();
                    currentChart.AxisY.Add(new Axis { Labels = productLabels });
                    currentChart.AxisX.Clear();
                    currentChart.AxisX.Add(new Axis { Title = "Doanh thu (VNĐ)", LabelFormatter = value => value.ToString("N0"), MinValue = 0 });
                }
                catch (Exception ex) { Console.WriteLine("Error LoadTopProductsChart: " + ex.Message); currentChart.Series.Clear(); }
            }
        }

        // --- BIỂU ĐỒ 3: HIỆU SUẤT NHÂN VIÊN (COLUMN CHART) ---
        private void LoadEmployeePerformanceChart()
        {
            LiveCharts.WinForms.CartesianChart currentChart = this.chartEmployeePerformance;
            if (currentChart != null)
            {
                try
                {
                    string sql = @"
                        SELECT nv.TenNV, SUM(hdb.TongTien) AS TongDoanhThuNV
                        FROM HoaDonBan hdb
                        JOIN NhanVien nv ON hdb.MaNV = nv.MaNV
                        WHERE hdb.IsDeleted = 0 AND hdb.NgayBan >= DATEADD(month, -1, CONVERT(date, GETDATE())) -- Doanh thu trong vòng 1 tháng gần nhất
                        GROUP BY nv.MaNV, nv.TenNV
                        HAVING SUM(hdb.TongTien) > 0
                        ORDER BY TongDoanhThuNV DESC"; //
                    DataTable dt = Function.GetDataToTable(sql);

                    var employeeLabels = new List<string>();
                    var employeeSalesValues = new ChartValues<decimal>();

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            employeeLabels.Add(row["TenNV"].ToString());
                            employeeSalesValues.Add(Convert.ToDecimal(row["TongDoanhThuNV"]));
                        }
                    }

                    currentChart.Series = new SeriesCollection
                    {
                        new ColumnSeries
                        {
                            Title = "Doanh thu",
                            Values = employeeSalesValues,
                            DataLabels = true
                        }
                    };
                    currentChart.AxisX.Clear();
                    currentChart.AxisX.Add(new Axis { Title = "Nhân viên", Labels = employeeLabels, LabelsRotation = 10, Separator = new Separator { Step = 1, IsEnabled = false } });
                    currentChart.AxisY.Clear();
                    currentChart.AxisY.Add(new Axis { Title = "Doanh thu (VNĐ)", LabelFormatter = value => value.ToString("N0"), MinValue = 0 });
                }
                catch (Exception ex) { Console.WriteLine("Error LoadEmployeePerformanceChart: " + ex.Message); currentChart.Series.Clear(); }
            }
        }

        // --- BIỂU ĐỒ 4: TỶ LỆ DOANH THU THEO LOẠI SẢN PHẨM (PIE CHART) ---
        private void LoadCategoryRevenueChart()
        {
            LiveCharts.WinForms.PieChart currentChart = this.chartCategoryRevenue;
            if (currentChart != null)
            {
                try
                {
                    string sql = @"
                        SELECT l.TenLoai, SUM(ct.ThanhTien) AS DoanhThuLoai
                        FROM ChiTietHDB ct
                        JOIN SanPham sp ON ct.MaSP = sp.MaSP
                        JOIN Loai l ON sp.MaLoai = l.MaLoai
                        JOIN HoaDonBan hdb ON ct.MaHDB = hdb.MaHDB
                        WHERE hdb.IsDeleted = 0 AND hdb.NgayBan >= DATEADD(month, -1, CONVERT(date, GETDATE())) -- Doanh thu trong vòng 1 tháng gần nhất
                        GROUP BY l.MaLoai, l.TenLoai
                        HAVING SUM(ct.ThanhTien) > 0
                        ORDER BY DoanhThuLoai DESC"; //
                    DataTable dt = Function.GetDataToTable(sql);

                    SeriesCollection pieSeriesCollection = new SeriesCollection();

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            pieSeriesCollection.Add(new PieSeries
                            {
                                Title = row["TenLoai"].ToString(),
                                Values = new ChartValues<decimal> { Convert.ToDecimal(row["DoanhThuLoai"]) },
                                DataLabels = true,
                                //LabelPoint = chartPoint => string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation) // Hiển thị giá trị và %
                            });
                        }
                    }
                    currentChart.Series = pieSeriesCollection;
                    currentChart.LegendLocation = LegendLocation.Right;
                }
                catch (Exception ex) { Console.WriteLine("Error LoadCategoryRevenueChart: " + ex.Message); currentChart.Series.Clear(); }
            }
        }

        private void LoadQuickViewData()
        {
            LoadRecentSales();
            LoadLowStockProducts();
        }

        // --- TẢI HÓA ĐƠN BÁN GẦN ĐÂY ---
        private void LoadRecentSales()
        {
            try
            {
                // Lấy 5 hóa đơn bán gần nhất
                string sql = @"
                    SELECT TOP 5 hdb.MaHDB, hdb.NgayBan, 
                                 ISNULL(kh.TenKH, N'Khách vãng lai') AS TenKhachHang, 
                                 hdb.TongTien
                    FROM HoaDonBan hdb
                    LEFT JOIN KhachHang kh ON hdb.MaKH = kh.MaKH
                    WHERE hdb.IsDeleted = 0
                    ORDER BY hdb.NgayBan DESC, hdb.MaHDB DESC"; //
                DataTable dtRecentSales = Function.GetDataToTable(sql);
                dgvHDBanGanDay.DataSource = dtRecentSales; //

                // Tùy chỉnh cột cho dgvHDBanGanDay
                if (dgvHDBanGanDay.Columns["MaHDB"] != null)
                {
                    dgvHDBanGanDay.Columns["MaHDB"].HeaderText = "Mã HĐ";
                    dgvHDBanGanDay.Columns["MaHDB"].Width = 150;
                }
                if (dgvHDBanGanDay.Columns["NgayBan"] != null)
                {
                    dgvHDBanGanDay.Columns["NgayBan"].HeaderText = "Ngày Bán";
                    dgvHDBanGanDay.Columns["NgayBan"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm"; // Hiển thị cả giờ nếu có
                    dgvHDBanGanDay.Columns["NgayBan"].Width = 130;
                }
                if (dgvHDBanGanDay.Columns["TenKhachHang"] != null)
                {
                    dgvHDBanGanDay.Columns["TenKhachHang"].HeaderText = "Khách Hàng";
                    dgvHDBanGanDay.Columns["TenKhachHang"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                if (dgvHDBanGanDay.Columns["TongTien"] != null)
                {
                    dgvHDBanGanDay.Columns["TongTien"].HeaderText = "Tổng Tiền";
                    dgvHDBanGanDay.Columns["TongTien"].DefaultCellStyle.Format = "N0";
                    dgvHDBanGanDay.Columns["TongTien"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvHDBanGanDay.Columns["TongTien"].Width = 120;
                }
                dgvHDBanGanDay.AllowUserToAddRows = false;
                dgvHDBanGanDay.EditMode = DataGridViewEditMode.EditProgrammatically;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error LoadRecentSales: " + ex.Message);
                dgvHDBanGanDay.DataSource = null;
            }
        }

        // --- TẢI SẢN PHẨM SẮP HẾT HÀNG ---, sẽ load khi có sản phẩm có số lượng < 10
        private void LoadLowStockProducts()
        {
            try
            {
                int nguongSapHetHang = 10; // Ngưỡng bạn đã dùng cho KPI
                string sql = @"
                    SELECT sp.MaSP, sp.TenSP, sp.SoLuong, l.TenLoai
                    FROM SanPham sp
                    LEFT JOIN Loai l ON sp.MaLoai = l.MaLoai
                    WHERE sp.SoLuong < @Nguong
                    ORDER BY sp.SoLuong ASC, sp.TenSP ASC"; //

                DataTable dtLowStock = Function.GetDataToTable(sql, new System.Data.SqlClient.SqlParameter("@Nguong", nguongSapHetHang));
                this.dvgSPSapHetHang.DataSource = dtLowStock; // Đảm bảo là dvgSPSapHetHang

                // Tùy chỉnh cột cho dvgSPSapHetHang
                // Tùy chỉnh cột cho dvgSPSapHetHang
                if (this.dvgSPSapHetHang.Columns["MaSP"] != null)
                {
                    this.dvgSPSapHetHang.Columns["MaSP"].HeaderText = "Mã SP";
                    this.dvgSPSapHetHang.Columns["MaSP"].Width = 80;
                }
                if (this.dvgSPSapHetHang.Columns["TenSP"] != null)
                {
                    this.dvgSPSapHetHang.Columns["TenSP"].HeaderText = "Tên Sản Phẩm";
                    this.dvgSPSapHetHang.Columns["TenSP"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                if (this.dvgSPSapHetHang.Columns["TenLoai"] != null)
                {
                    this.dvgSPSapHetHang.Columns["TenLoai"].HeaderText = "Loại SP";
                    this.dvgSPSapHetHang.Columns["TenLoai"].Width = 150;
                }
                if (this.dvgSPSapHetHang.Columns["SoLuong"] != null)
                {
                    this.dvgSPSapHetHang.Columns["SoLuong"].HeaderText = "SL Tồn";
                    this.dvgSPSapHetHang.Columns["SoLuong"].Width = 70;
                    this.dvgSPSapHetHang.Columns["SoLuong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    // Highlight nếu số lượng đặc biệt thấp (ví dụ: <= 5)
                    this.dvgSPSapHetHang.Columns["SoLuong"].DefaultCellStyle.ForeColor = Color.Red;
                    // Dòng này cần sửa:
                    this.dvgSPSapHetHang.Columns["SoLuong"].DefaultCellStyle.Font = new Font(this.dvgSPSapHetHang.Font, FontStyle.Bold);
                }
                this.dvgSPSapHetHang.AllowUserToAddRows = false;
                this.dvgSPSapHetHang.EditMode = DataGridViewEditMode.EditProgrammatically;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error LoadLowStockProducts: " + ex.Message);
                dvgSPSapHetHang.DataSource = null;
            }
        }


        private void UpdateDateTimeLabels()
        {
            // Lấy ngày giờ hiện tại
            DateTime now = DateTime.Now;

            // --- Cập nhật Label thứ 2 (ngày tháng năm) ---
            // Định dạng ngày tháng tiếng Việt
            CultureInfo viVn = new CultureInfo("vi-VN");
            // Lấy tên thứ trong tuần (ví dụ: "Thứ hai")
            string dayOfWeek = viVn.DateTimeFormat.GetDayName(now.DayOfWeek);
            // Định dạng chuỗi ngày tháng
            string dateString = $"{dayOfWeek}, {now:dd/MM/yyyy}";
            // Giả sử Label thứ 2 của bạn có tên là label2
            lblNgay.Text = dateString;

            // --- Cập nhật Label thứ 3 (giờ phút giây) ---
            // Định dạng chuỗi thời gian
            string timeString = now.ToString("HH:mm:ss"); // Sử dụng "HH" cho định dạng 24 giờ
                                                          // Giả sử Label thứ 3 của bạn có tên là label3
            lblThoiGian.Text = timeString;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Cập nhật các label mỗi khi Timer tick
            UpdateDateTimeLabels();
        }

        private void mnuDangXuat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void mnuHDBan_Click(object sender, EventArgs e)
        {
            HoaDonBan form = new HoaDonBan();
            form.Show();
        }

        private void mnuHDNhap_Click(object sender, EventArgs e)
        {
            HoaDonNhap form = new HoaDonNhap();
            form.Show();
        }

        private void mnuSanPham_Click(object sender, EventArgs e)
        {
            SanPham form = new SanPham();
            form.Show();
        }

        private void mnuKhuyenMai_Click(object sender, EventArgs e)
        {
            KhuyenMai form = new KhuyenMai();
            form.Show();
        }

        private void mnuNhaCungCap_Click(object sender, EventArgs e)
        {
            NhaCungCap form = new NhaCungCap();
            form.Show();
        }

        private void mnuTaiKhoan_Click(object sender, EventArgs e)
        {
            TaiKhoan form = new TaiKhoan();
            form.Show();
        }

        private void mnuTKHDBan_Click(object sender, EventArgs e)
        {
            TimHDBan form = new TimHDBan();
            form.Show();
        }

        private void mnuTKHDNhap_Click(object sender, EventArgs e)
        {
            TimHDNhap form = new TimHDNhap();
            form.Show();
        }

        private void mnuDoanhThu_Click(object sender, EventArgs e)
        {
            BaoCaoDoanhThu form = new BaoCaoDoanhThu();
            form.Show();
        }

        private void mnuHangTonKho_Click(object sender, EventArgs e)
        {
            BaoCaoTonKho form = new BaoCaoTonKho();
            form.Show();
        }

        private void btnSP_Click(object sender, EventArgs e)
        {
            SanPham form = new SanPham();
            form.Show();
        }

        private void btnKM_Click(object sender, EventArgs e)
        {
            KhuyenMai form = new KhuyenMai();
            form.Show();
        }

        private void btnNCC_Click(object sender, EventArgs e)
        {
            NhaCungCap form = new NhaCungCap();
            form.Show();
        }

        private void btnTK_Click(object sender, EventArgs e)
        {
            TaiKhoan form = new TaiKhoan();
            form.Show();
        }

        private void btnTKHDB_Click(object sender, EventArgs e)
        {
            TimHDBan form = new TimHDBan();
            form.Show();
        }

        private void btnTKHDN_Click(object sender, EventArgs e)
        {
            TimHDNhap form = new TimHDNhap();
            form.Show();
        }

        private void btnBCDT_Click(object sender, EventArgs e)
        {
            BaoCaoDoanhThu form = new BaoCaoDoanhThu();
            form.Show();
        }

        private void btnBCTK_Click(object sender, EventArgs e)
        {
            BaoCaoTonKho form = new BaoCaoTonKho();
            form.Show();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            // Nên có xác nhận trước khi thoát
            if (MessageBox.Show("Bạn có chắc chắn muốn đăng xuất và thoát ứng dụng?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit(); // Hoặc quay về form Login nếu có
            }
        }

        private void panelWrapperTopProducts_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
