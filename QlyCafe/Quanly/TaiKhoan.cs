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

namespace QlyCafe.Quanly
{
    public partial class TaiKhoan : Form
    {
        public TaiKhoan()
        {
            InitializeComponent();
            txtPassword.UseSystemPasswordChar = true;
            dgvTaiKhoan.CellClick += dgvTaiKhoan_CellClick;
            LoadTaiKhoan();
            dgvTaiKhoan.Columns["MatKhau"].Visible = false;

        }

        private bool isAdding = false;
        private bool isEditing = false;





        private void LoadTaiKhoan()
        {
            string sql = "SELECT * FROM TaiKhoan";
            dgvTaiKhoan.DataSource = Function.GetDataToTable(sql);

            string sqlNV = "SELECT MaNV, TenNV FROM NhanVien";
            Function.FillCombo(cboMaLienKet, "TenNV", "MaNV", sqlNV);
        }

        private void ClearInput()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            cboMaLienKet.SelectedIndex = -1;
            txtPassword.Enabled = false;
            isAdding = isEditing = false;
        }





        private void dgvTaiKhoan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;
            if (row >= 0)
            {
                txtUsername.Text = dgvTaiKhoan.Rows[row].Cells["TenDangNhap"].Value.ToString();
                cboMaLienKet.SelectedValue = dgvTaiKhoan.Rows[row].Cells["MaLienKet"].Value.ToString();
                txtPassword.Clear();
                txtPassword.Enabled = false;
            }
        }

        private void btnSua_Click_1(object sender, EventArgs e)
        {
            if (txtUsername.Text == "") return;

            isEditing = true;
            txtUsername.Enabled = false;
            txtPassword.Enabled = true;
        }

        private void btnThem_Click_1(object sender, EventArgs e)
        {
            ClearInput();
            isAdding = true;
            txtUsername.Enabled = true;
            txtPassword.Enabled = true;
        }

        private void btnLuu_Click_1(object sender, EventArgs e)
        {
            string user = txtUsername.Text.Trim();
            string pass = txtPassword.Text.Trim();
            string maNV = cboMaLienKet.SelectedValue?.ToString() ?? "";
            string role = "NhanVien";

            if (user == "" || maNV == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.");
                return;
            }

            if (isAdding)
            {
                string hash = BCrypt.Net.BCrypt.HashPassword(pass);
                string sql = "INSERT INTO TaiKhoan (TenDangNhap, MatKhau, VaiTro, MaLienKet) VALUES (@User, @Pass, @Role, @MaNV)";
                SqlParameter[] prms =
                {
                    new SqlParameter("@User", user),
                    new SqlParameter("@Pass", hash),
                    new SqlParameter("@Role", role),
                    new SqlParameter("@MaNV", maNV),
                };
                Function.ExecuteNonQuery(sql, prms);
            }
            else if (isEditing)
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@User", user),
                    new SqlParameter("@MaNV", maNV)
                };

                string sql = "UPDATE TaiKhoan SET MaLienKet = @MaNV";

                if (!string.IsNullOrEmpty(pass))
                {
                    string hash = BCrypt.Net.BCrypt.HashPassword(pass);
                    sql += ", MatKhau = @Pass";
                    parameters.Add(new SqlParameter("@Pass", hash));
                }

                sql += " WHERE TenDangNhap = @User";
                Function.ExecuteNonQuery(sql, parameters.ToArray());
            }

            LoadTaiKhoan();
            ClearInput();
        }

        private void btnBoqua_Click_1(object sender, EventArgs e)
        {
            ClearInput();
        }

        private void btnXoa_Click_1(object sender, EventArgs e)
        {
            if (txtUsername.Text == "") return;

            DialogResult result = MessageBox.Show("Xoá tài khoản này?", "Xác nhận", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                string sql = "DELETE FROM TaiKhoan WHERE TenDangNhap = @User";
                Function.ExecuteNonQuery(sql, new SqlParameter("@User", txtUsername.Text.Trim()));
                LoadTaiKhoan();
                ClearInput();
            }
        }

        private void btnDong_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
