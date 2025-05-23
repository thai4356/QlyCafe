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
    public partial class FormNhanVien : Form
    {
        private bool isAdding = false;

        public FormNhanVien()
        {
            InitializeComponent();
            LoadNhanVien();
            LoadCombos(); 
            dtpNgaySinh.Value = DateTime.Now;
            ClearInputs();
            this.dgvNhanVien.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvNhanVien_CellClick);
            dgvNhanVien.AllowUserToAddRows = false;


        }

        private void LoadNhanVien()
        {
            string sql = "SELECT * FROM NhanVien";
            dgvNhanVien.DataSource = Function.GetDataToTable(sql);
        }

        private void LoadCombos()
        {
            cboGioiTinh.Items.Add("Nam");
            cboGioiTinh.Items.Add("Nữ");

            string sql = "SELECT MaQue, TenQue FROM Que"; // Giả định bạn có bảng Que
            Function.FillCombo(cboMaQue, "TenQue", "MaQue", sql);

        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
           
        }


        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;
            txtMaNV.Text = dgvNhanVien.Rows[row].Cells["MaNV"].Value.ToString();
            txtTenNV.Text = dgvNhanVien.Rows[row].Cells["TenNV"].Value.ToString();
            txtDiaChi.Text = dgvNhanVien.Rows[row].Cells["DiaChi"].Value.ToString();
            cboGioiTinh.Text = dgvNhanVien.Rows[row].Cells["GioiTinh"].Value.ToString();
            dtpNgaySinh.Value = Convert.ToDateTime(dgvNhanVien.Rows[row].Cells["NgaySinh"].Value);
            cboMaQue.SelectedValue = dgvNhanVien.Rows[row].Cells["MaQue"].Value.ToString();
            txtSDT.Text = dgvNhanVien.Rows[row].Cells["SDT"].Value.ToString();
        }

        private void ClearInputs()
        {
            txtMaNV.Clear();
            txtTenNV.Clear();
            txtDiaChi.Clear();
            cboGioiTinh.SelectedIndex = -1;
            cboMaQue.SelectedIndex = -1;
            dtpNgaySinh.Value = DateTime.Now;
            txtSDT.Clear();
        }

        private void FormNhanVien_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void btnLuu_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaNV.Text) ||
        string.IsNullOrWhiteSpace(txtTenNV.Text) ||
        cboMaQue.SelectedIndex == -1 ||
        cboGioiTinh.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string sql;
            List<SqlParameter> parameters = new List<SqlParameter>()
    {
        new SqlParameter("@MaNV", txtMaNV.Text),
        new SqlParameter("@TenNV", txtTenNV.Text),
        new SqlParameter("@DiaChi", txtDiaChi.Text),
        new SqlParameter("@GioiTinh", cboGioiTinh.Text),
        new SqlParameter("@NgaySinh", dtpNgaySinh.Value),
        new SqlParameter("@MaQue", cboMaQue.SelectedValue),
        new SqlParameter("@SDT", txtSDT.Text),
    };

            if (isAdding)
            {
                sql = "INSERT INTO NhanVien VALUES (@MaNV, @TenNV, @DiaChi, @GioiTinh, @NgaySinh, @MaQue, @SDT)";

                string tenDangNhap = txtMaNV.Text.Trim();
                string matKhauMacDinh = "nhanviencafe@";

                // Mã hóa mật khẩu
                string matKhauHash = BCrypt.Net.BCrypt.HashPassword(matKhauMacDinh);

                string sqlInsertTK = "INSERT INTO TaiKhoan (TenDangNhap, MatKhau, VaiTro, MaLienKet) " +
                                     "VALUES (@TenDN, @MatKhau, @VaiTro, @MaNV)";

                SqlParameter[] paramTK = new SqlParameter[]
                {
            new SqlParameter("@TenDN", tenDangNhap),
            new SqlParameter("@MatKhau", matKhauHash),
            new SqlParameter("@VaiTro", "NhanVien"),
            new SqlParameter("@MaNV", tenDangNhap) 
                };

                Function.ExecuteNonQuery(sqlInsertTK, paramTK);

            }
            else
            {
                sql = "UPDATE NhanVien SET TenNV=@TenNV, DiaChi=@DiaChi, GioiTinh=@GioiTinh, NgaySinh=@NgaySinh, MaQue=@MaQue, SDT=@SDT WHERE MaNV=@MaNV";
            }

            Function.ExecuteNonQuery(sql, parameters.ToArray());
            LoadNhanVien();
        }

        private void btnThem_Click_1(object sender, EventArgs e)
        {
            ClearInputs();
            txtMaNV.Enabled = false;
            txtMaNV.Text = GenerateNextMaNV(); // Tự sinh mã
            isAdding = true;

        }
        private string GenerateNextMaNV()
{
    string sql = "SELECT TOP 1 MaNV FROM NhanVien ORDER BY MaNV DESC";
    DataTable dt = Function.GetDataToTable(sql);

    if (dt.Rows.Count == 0)
        return "NV001";

    string lastMaNV = dt.Rows[0]["MaNV"].ToString(); 
    int number = int.Parse(lastMaNV.Substring(2));   
    number++;
    return "NV" + number.ToString("D3");             
}

       

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtMaNV.Text == "")
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            txtMaNV.Enabled = false; // Không cho sửa mã
            isAdding = false;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (txtMaNV.Text == "")
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa nhân viên này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                string maNV = txtMaNV.Text.Trim();

                // 1. Xoá tài khoản nếu có
                string sqlXoaTK = "DELETE FROM TaiKhoan WHERE MaLienKet = @MaNV";
                string sqlXoaNV = "DELETE FROM NhanVien WHERE MaNV = @MaNV";

                SqlParameter param1 = new SqlParameter("@MaNV", maNV);
                SqlParameter param2 = new SqlParameter("@MaNV", maNV);


                // Sử dụng transaction để đảm bảo atomic
                Function.ExecuteTransaction((conn, tx) =>
                {
                    Function.ExecuteNonQuery(sqlXoaTK, conn, tx, param1);
                    Function.ExecuteNonQuery(sqlXoaNV, conn, tx, param2);

                });

                MessageBox.Show("Đã xoá nhân viên.", "Thành công");
                LoadNhanVien();
                ClearInputs();
            }
        }
    }
}
