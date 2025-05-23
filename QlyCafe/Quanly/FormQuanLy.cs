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
            // Bạn có thể thêm Timer ở đây để refresh KPIs định kỳ nếu muốn
            Timer kpiTimer = new Timer();
            kpiTimer.Interval = 60000; // Ví dụ: cập nhật mỗi 60 giây
            kpiTimer.Tick += KpiTimer_Tick;
            kpiTimer.Start();
        }

        private void KpiTimer_Tick(object sender, EventArgs e)
        {
            LoadKPIs();
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

        }

        private void mnuHangTonKho_Click(object sender, EventArgs e)
        {

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

        }

        private void btnBCTK_Click(object sender, EventArgs e)
        {

        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            // Nên có xác nhận trước khi thoát
            if (MessageBox.Show("Bạn có chắc chắn muốn đăng xuất và thoát ứng dụng?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit(); // Hoặc quay về form Login nếu có
            }
        }
    }
}
