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
            Application.Exit();
        }
    }
}
