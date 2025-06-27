using System.Data.SqlClient;

namespace LoanManagementSystem.Util
{
    public class DBConnUtil
    {
        private static string connStr = "Server=LAPTOP-MEERAH74\\SQLEXPRESS;Database=LoanDB;Trusted_Connection=True;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connStr);
        }
    }
}
