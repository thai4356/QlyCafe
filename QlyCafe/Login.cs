using QlyCafe.Quanly;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

            string sql = $"SELECT * FROM TaiKhoan WHERE TenDangNhap = N'{user}' AND MatKhau = N'{pass}'";
            DataTable dt = Function.GetDataToTable(sql);

            if (dt.Rows.Count > 0)
            {
                string role = dt.Rows[0]["VaiTro"].ToString();
                string maLienKet = dt.Rows[0]["MaLienKet"].ToString();

                UserSession.TenDangNhap = user;
                UserSession.VaiTro = role;
                UserSession.MaNguoiDung = maLienKet;

                switch (role)
                {
                    case "NguoiDung":
                        new FormNguoiDung().Show();
                        break;
                    case "NguoiBan":
                        new FormNguoiBan().Show();
                        break;
                    case "QuanLy":
                        new FormQuanLy().Show();
                        break;
                }

                this.Hide();
            }
            else
            {
                MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu.", "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}
