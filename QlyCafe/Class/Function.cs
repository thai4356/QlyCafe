using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QlyCafe
{
    internal class Function
    {
        private static string connString = "Data Source=LAPTOP-FRNPC1AU;Initial Catalog=qlyCafe;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
        private static SqlConnection conn;

        public static void OpenConnection()
        {
            if (conn == null)
                conn = new SqlConnection(connString);

            if (conn.State != ConnectionState.Open)
                conn.Open();
        }

        public static void CloseConnection()
        {
            if (conn != null && conn.State != ConnectionState.Closed)
                conn.Close();
        }


        public static DataTable GetDataToTable(string sql)
        {
            OpenConnection();

            SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            CloseConnection();
            return dt;
        }

        public static void ExecuteNonQuery(string sql)
        {
            OpenConnection();
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
            CloseConnection();
        }

        public static bool CheckExists(string sql)
        {
            OpenConnection();
            SqlCommand cmd = new SqlCommand(sql, conn);
            object result = cmd.ExecuteScalar();
            CloseConnection();
            return result != null;
        }

        // Do tôi dùng sql prepared statement nên tôi sẽ tạo các hàm overload cho các form crud của tôi

        public static DataTable GetDataToTable(string sql, params SqlParameter[] parameters)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(connString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    if (parameters != null && parameters.Length > 0) // Kiểm tra parameters.Length > 0 để an toàn
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    try
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        adapter.Fill(dataTable);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("DataService - Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            return dataTable;
        }

        public static void FillCombo(ComboBox cbo, string displayMember, string valueMember, string sql, params SqlParameter[] parameters)
        {
            DataTable comboDataTable = GetDataToTable(sql, parameters);
            cbo.DataSource = comboDataTable;
            cbo.DisplayMember = displayMember;
            cbo.ValueMember = valueMember;
            cbo.SelectedIndex = -1; // Reset selection
        }

        public static string GetFieldValue(string sql, params SqlParameter[] parameters) // Đổi tên GetFieldValues thành GetFieldValue cho rõ ràng hơn vì nó trả về một giá trị
        {
            string value = "";
            using (SqlConnection connection = new SqlConnection(connString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    if (parameters != null && parameters.Length > 0)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            value = result.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("DataService - Lỗi khi lấy giá trị: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            return value;
        }

        public static void RunSql(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(connString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    if (parameters != null && parameters.Length > 0)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("DataService - Lỗi SQL: " + ex.Message + "\nSố lỗi: " + ex.Number, "Lỗi Thực Thi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("DataService - Lỗi không xác định: " + ex.Message, "Lỗi Thực Thi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        public static int CountRecords(string sql, params SqlParameter[] parameters)
        {
            int count = 0;
            using (SqlConnection connection = new SqlConnection(connString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    if (parameters != null && parameters.Length > 0)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out count))
                        {
                            return count;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("DataService - Lỗi khi tìm kiếm số bản ghi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            return count;
        }

        public static bool CheckKey(string sql, params SqlParameter[] parameters)
        {
            int recordCount = CountRecords(sql,parameters);
            return recordCount > 0;
            
        }

        public static void Logout(Form currentForm)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                // Reset session nếu cần
                UserSession.TenDangNhap = null;
                UserSession.VaiTro = null;
                UserSession.MaNguoiDung = null;

                // Mở lại form Login
                Login loginForm = new Login();
                loginForm.Show();

                // Đóng form hiện tại
                currentForm.Close();
            }
        }

    }
}
