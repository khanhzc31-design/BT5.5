using Microsoft.Data.SqlClient;

namespace InventoryManagement.DAL
{
    public class DatabaseHelper
    {
        public static string ConnectionString = @"Server=.;Database=InventoryDB;Trusted_Connection=True;TrustServerCertificate=True;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}