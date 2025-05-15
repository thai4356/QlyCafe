using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
