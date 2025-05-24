using QlyCafe.Quanly;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QlyCafe
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string user = txtUsername.Text.Trim();
            string pass = txtPassword.Text.Trim();

            if (user == "" || pass == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin đăng nhập.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 1. Truy vấn lấy mật khẩu đã mã hóa
            string sql = "SELECT MatKhau, VaiTro, MaLienKet FROM TaiKhoan WHERE TenDangNhap = @User";
            SqlParameter param = new SqlParameter("@User", user);
            DataTable dt = Function.GetDataToTable(sql, param);

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu.", "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string hashedPassword = dt.Rows[0]["MatKhau"].ToString();

            // 2. Dùng Bcrypt để so sánh mật khẩu
            if (!BCrypt.Net.BCrypt.Verify(pass, hashedPassword))
            {
                MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu.", "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 3. Đăng nhập thành công
            string role = dt.Rows[0]["VaiTro"].ToString();
            string maLienKet = dt.Rows[0]["MaLienKet"].ToString();

            UserSession.TenDangNhap = user;
            UserSession.VaiTro = role;
            UserSession.MaNguoiDung = maLienKet;

            switch (role)
            {
                case "NhanVien":
                    new DatBan().Show();
                    break;
                case "Admin":
                    new FormQuanLy().Show();
                    break;
            }

            this.Hide();
        }


        private void Login_Load(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = true;
        }
    }
}
