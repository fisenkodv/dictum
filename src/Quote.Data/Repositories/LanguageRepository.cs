using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Dictum.Business.Abstract.Repositories;
using Dictum.Data.Models;
using Microsoft.Extensions.Configuration;
using ConfigurationExtensions = Dictum.Data.Extensions.ConfigurationExtensions;
using Language = Dictum.Business.Models.Language;

namespace Dictum.Data.Repositories
{
    public class LanguageRepository : ILanguageRepository
    {
        private readonly IConfiguration _configuration;

        public LanguageRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<Language>> GetAll()
        {
            using (var connection = ConfigurationExtensions.GetConnection(_configuration))
            {
                var sql = $@"
                     SELECT     {LanguageSchema.Table}.{LanguageSchema.Columns.Code} AS Code,
                                {LanguageSchema.Table}.{LanguageSchema.Columns.Name} AS Description
                     FROM       {LanguageSchema.Table} AS {LanguageSchema.Table}
                     GROUP BY   {LanguageSchema.Table}.{LanguageSchema.Columns.Id}";

                return await connection.QueryAsync<Language>(sql);
            }
        }
    }
}