using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using System.Data.Common;

namespace Dictum.Data
{
    internal static class ConfigurationExtensions
    {
        public static DbConnection GetConnection(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Dictum");
            return GetConnection(connectionString);
        }

        private static DbConnection GetConnection(string connectionString)
        {
            var connectionStringBuilder = new MySqlConnectionStringBuilder(connectionString)
            {
                AllowZeroDateTime = false,
                ConvertZeroDateTime = false,
                Pooling = true
            };

            var connection = new MySqlConnection(connectionStringBuilder.ConnectionString);
            connection.Open();

            return connection;
        }
    }
}
