using System.Threading.Tasks;
using Dictum.Data.Models;
using Microsoft.Extensions.Configuration;
using Dapper;
using Dictum.Business.Abstract.Repositories;

namespace Dictum.Data.Repositories
{
    public class DictumRepository : IDictumRepository
    {
        private readonly IConfiguration _configuration;

        public DictumRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Business.Models.Dictum> GetRandom(string lang)
        {
            using (var connection = ConfigurationExtensions.GetConnection(_configuration))
            {
                string sql = $@"
                     SELECT     {DictumSchema.Columns.Uuid},
                                {DictumSchema.Columns.Quote},
                                {DictumSchema.Columns.Author}
                     FROM       {DictumSchema.Table}
                     INNER JOIN {LanguageSchema.Table}
                     ON         {DictumSchema.Table}.{DictumSchema.Columns.LanguageId} = {LanguageSchema.Table}.{LanguageSchema.Columns.Id}
                     WHERE      {LanguageSchema.Columns.Code} = @{nameof(lang)}
                     ORDER BY RAND() LIMIT 1";

                return await connection.QueryFirstAsync<Business.Models.Dictum>(sql, new {lang});
            }
        }

        public async Task<Business.Models.Dictum> GetDictum(string uuid)
        {
            using (var connection = ConfigurationExtensions.GetConnection(_configuration))
            {
                string sql = $@"
                     SELECT {DictumSchema.Columns.Uuid},
                            {DictumSchema.Columns.Quote},
                            {DictumSchema.Columns.Author}
                     FROM   {DictumSchema.Table}
                     WHERE  {DictumSchema.Columns.Uuid} = @{nameof(uuid)}";

                return await connection.QueryFirstOrDefaultAsync<Business.Models.Dictum>(sql, new {uuid});
            }
        }
    }
}