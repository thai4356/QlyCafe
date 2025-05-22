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
        private static string connString = "Data Source=DESKTOP-6P76JI8\\SQLEXPRESS;Initial Catalog=qlyCafe;Integrated Security=True;TrustServerCertificate=True";
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

        public static void FillCombo(ComboBox cbo, string displayMember, string valueMember, DataTable dataSource)
        {
            cbo.DataSource = dataSource;
            cbo.DisplayMember = displayMember;
            cbo.ValueMember = valueMember;
            if (dataSource != null && dataSource.Rows.Count > 0)
            {
                // Nếu bạn muốn tự động chọn item đầu tiên sau khi binding DataTable
                // cbo.SelectedIndex = 0;
                // Hoặc để trống lựa chọn ban đầu
                cbo.SelectedIndex = -1;
            }
            else
            {
                cbo.SelectedIndex = -1;
                cbo.DataSource = null; // Xóa DataSource nếu không có dữ liệu
            }
        }

        public static void FillListBox(ListBox lst, string displayMember, string valueMember, string sql, params SqlParameter[] parameters)
        {
            DataTable listBoxDataTable = GetDataToTable(sql, parameters);
            lst.DataSource = listBoxDataTable;
            lst.DisplayMember = displayMember;
            lst.ValueMember = valueMember;
            lst.SelectedIndex = -1; // Reset selection
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

        /// <summary>
        /// Lấy một giá trị đơn lẻ từ CSDL sử dụng một SqlConnection và SqlTransaction đã có.
        /// </summary>
        /// <param name="sql">Câu lệnh SQL.</param>
        /// <param name="connection">Đối tượng SqlConnection đang mở và đã bắt đầu transaction.</param>
        /// <param name="transaction">Đối tượng SqlTransaction đang được sử dụng.</param>
        /// <param name="parameters">Mảng các SqlParameter (tùy chọn).</param>
        /// <returns>Giá trị dạng chuỗi, hoặc chuỗi rỗng nếu không tìm thấy hoặc có lỗi.</returns>
        public static string GetFieldValue(string sql, SqlConnection connection, SqlTransaction transaction, params SqlParameter[] parameters)
        {
            string value = "";
            if (connection == null || connection.State != ConnectionState.Open)
            {
                // Hoặc throw new ArgumentException, hoặc xử lý theo cách khác nếu muốn
                MessageBox.Show("Lỗi GetFieldValue: SqlConnection phải hợp lệ và đang mở.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return value; // Trả về rỗng nếu connection không hợp lệ
            }
            // Transaction có thể là null nếu bạn muốn hàm này cũng dùng được cho các truy vấn đơn lẻ không cần transaction
            // nhưng đang được gọi từ một ngữ cảnh có transaction.
            // Tuy nhiên, nếu đã truyền vào, thì nó nên được sử dụng.

            using (SqlCommand command = new SqlCommand(sql, connection, transaction)) // Gán transaction cho command
            {
                if (parameters != null && parameters.Length > 0)
                {
                    command.Parameters.AddRange(parameters);
                }
                try
                {
                    // Connection đã được mở từ bên ngoài (bởi lời gọi Function.ExecuteTransaction)
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        value = result.ToString();
                    }
                }
                catch (Exception ex)
                {
                    // Tránh hiển thị MessageBox trực tiếp từ lớp Function trong môi trường production
                    // Có thể ghi log hoặc throw exception để lớp gọi xử lý
                    Console.WriteLine("Lỗi GetFieldValue (trong transaction): " + ex.Message);
                    // MessageBox.Show("DataService - Lỗi khi lấy giá trị (transaction): " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return value;
        }

        /// <summary>
        /// Thực thi một câu lệnh INSERT/UPDATE/DELETE, không có transaction riêng.
        /// </summary>
        public static void ExecuteNonQuery(string sql, params SqlParameter[] parameters)
        {
            using (var conn = new SqlConnection(connString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                if (parameters?.Any() == true)
                    cmd.Parameters.AddRange(parameters);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // === HÀM MỚI ExecuteNonQuery HỖ TRỢ TRANSACTION ===
        /// <summary>
        /// Thực thi một câu lệnh SQL (INSERT, UPDATE, DELETE) sử dụng một SqlConnection và SqlTransaction đã có.
        /// </summary>
        /// <param name="sql">Câu lệnh SQL.</param>
        /// <param name="connection">Đối tượng SqlConnection đang mở và đã bắt đầu transaction.</param>
        /// <param name="transaction">Đối tượng SqlTransaction đang được sử dụng.</param>
        /// <param name="parameters">Mảng các SqlParameter (tùy chọn).</param>
        public static void ExecuteNonQuery(string sql, SqlConnection connection, SqlTransaction transaction, params SqlParameter[] parameters)
        {
            if (connection == null || connection.State != ConnectionState.Open)
            {
                throw new ArgumentException("SqlConnection phải hợp lệ và đang mở trong ngữ cảnh transaction.");
            }
            if (transaction == null)
            {
                throw new ArgumentException("SqlTransaction không thể null khi thực thi trong ngữ cảnh transaction.");
            }

            // SqlCommand sẽ được gắn với connection và transaction này
            using (SqlCommand command = new SqlCommand(sql, connection, transaction))
            {
                if (parameters != null && parameters.Length > 0)
                {
                    command.Parameters.AddRange(parameters);
                }
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Thực thi nhiều câu lệnh trong cùng một transaction.
        /// </summary>
        public static void ExecuteTransaction(Action<SqlConnection, SqlTransaction> body)
        {
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                using (var tx = conn.BeginTransaction())
                {
                    try
                    {
                        body(conn, tx);
                        tx.Commit();
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }

        public static void RunSql(string sql, params SqlParameter[] parameters)
            => ExecuteNonQuery(sql, parameters);

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

        // Trong Function.cs
        public static int CountRecords(string sql, SqlConnection connection, SqlTransaction transaction, params SqlParameter[] parameters)
        {
            int count = 0;
            // SqlCommand sẽ được gắn với connection và transaction này
            using (SqlCommand command = new SqlCommand(sql, connection, transaction))
            {
                if (parameters != null && parameters.Length > 0)
                {
                    command.Parameters.AddRange(parameters);
                }
                try
                {
                    // Connection đã được mở bởi lời gọi ExecuteTransaction
                    object result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out count))
                    {
                        return count;
                    }
                }
                catch (Exception ex)
                {
                    // Không nên hiển thị MessageBox từ lớp Function, nên throw hoặc trả về giá trị lỗi
                    // MessageBox.Show("DataService - Lỗi khi đếm bản ghi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Console.WriteLine("DataService - Lỗi khi đếm bản ghi: " + ex.Message);
                    // Hoặc throw ex;
                }
            }
            return count; // Hoặc một giá trị chỉ báo lỗi nếu có exception
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
