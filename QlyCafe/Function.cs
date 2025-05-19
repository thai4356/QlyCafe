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
        public static string connString = "Data Source=DESKTOP-6P76JI8\\SQLEXPRESS;Initial Catalog=qlyCafe;Integrated Security=True;TrustServerCertificate=True";
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

        public static bool CheckKey(string sql, params SqlParameter[] parameters)
        {
            int recordCount = CountRecords(sql,parameters);
            return recordCount > 0;
            
        }



    }
}
