using System.Data.Common;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace Dictum.Data.Extensions
{
    internal static class ConfigurationExtensions
    {
        public static DbConnection GetConnection(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Quote");
            return GetConnection(connectionString);
        }

        private static DbConnection GetConnection(string connectionString)
        {
            var connectionStringBuilder = new MySqlConnectionStringBuilder(connectionString)
            {
                AllowZeroDateTime = false,
                ConvertZeroDateTime = false,
                Pooling = true,
                AllowUserVariables = true,
                ConnectionTimeout = 60 * 5
            };

            var connection = new MySqlConnection(connectionStringBuilder.ConnectionString);
            connection.Open();

            return connection;
        }
    }
}