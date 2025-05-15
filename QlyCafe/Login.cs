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

                switch (role)
                {
                    case "NguoiDung":
                        FormNguoiDung f1 = new FormNguoiDung();
                        f1.Show();
                        break;
                    case "NguoiBan":
                        FormNguoiBan f2 = new FormNguoiBan();
                        f2.Show();
                        break;
                    case "QuanLy":
                        FormQuanLy f3 = new FormQuanLy();
                        f3.Show();
                        break;
                }

                this.Hide(); // Ẩn FormLogin sau khi đăng nhập thành công
            }
            else
            {
                MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu.", "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
