using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Dictum.Business.Abstract.Repositories;
using Dictum.Data.Models;
using Microsoft.Extensions.Configuration;
using ConfigurationExtensions = Dictum.Data.Extensions.ConfigurationExtensions;

namespace Dictum.Data.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly IConfiguration _configuration;

        public AuthorRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<Business.Models.Author>> GetAuthors(string query, int page, int count)
        {
            using (var connection = ConfigurationExtensions.GetConnection(_configuration))
            {
                var sql = $@"
                     SELECT {LanguageSchema.Table}.{LanguageSchema.Columns.Code} AS Code,
                            {LanguageSchema.Table}.{LanguageSchema.Columns.Name} AS Description,
                     FROM   {LanguageSchema.Table} AS {LanguageSchema.Table}";

                return await connection.QueryAsync<Business.Models.Author>(sql);
            }
        }
    }
}