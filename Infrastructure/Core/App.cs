
using System.Configuration;
using System.Data.SqlClient;

namespace Core
{
    public static class App
    {
        public static string DatabaseName()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);

            return builder.InitialCatalog;
        }
    }
}
