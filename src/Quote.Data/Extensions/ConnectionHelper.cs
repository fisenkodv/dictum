using System.Data.Common;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace Dictum.Data.Extensions
{
    internal static class ConfigurationExtensions
    {
        public static DbConnection CreateOpenConnection(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Quote");
            return CreateOpenConnection(connectionString);
        }

        private static DbConnection CreateOpenConnection(string connectionString)
        {
            var connectionStringBuilder = new MySqlConnectionStringBuilder(connectionString)
            {
                AllowZeroDateTime = false,
                ConvertZeroDateTime = false,
                Pooling = true,
                AllowUserVariables = true,
                ConnectionTimeout = 10 * 5
            };

            var connection = new MySqlConnection(connectionStringBuilder.ConnectionString);
            connection.Open();

            return connection;
        }
    }
}