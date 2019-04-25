using System.Threading.Tasks;
using Dapper;
using Dictum.Business.Abstract.Repositories;
using Dictum.Data.Models;
using Microsoft.Extensions.Configuration;
using ConfigurationExtensions = Dictum.Data.Extensions.ConfigurationExtensions;

namespace Dictum.Data.Repositories
{
    public class DictumRepository : IDictumRepository
    {
        private readonly IConfiguration _configuration;

        public DictumRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Business.Models.Quote> GetRandom(string lang)
        {
            using (var connection = ConfigurationExtensions.GetConnection(_configuration))
            {
                var sql = $@"
                     SELECT     {QuoteSchema.Table}.{QuoteSchema.Columns.Uuid},
                                {QuoteSchema.Table}.{QuoteSchema.Columns.Text},
                                {QuoteSchema.Table}.{QuoteSchema.Columns.Author}
                     FROM       {QuoteSchema.Table} AS {QuoteSchema.Table}
                     INNER JOIN {LanguageSchema.Table} AS {LanguageSchema.Table}
                     ON         {QuoteSchema.Table}.{QuoteSchema.Columns.LanguageId} = {LanguageSchema.Table}.{LanguageSchema.Columns.Id}
                     WHERE      {LanguageSchema.Columns.Code} = @{nameof(lang)}
                     ORDER BY RAND() LIMIT 1";

                return await connection.QueryFirstOrDefaultAsync<Business.Models.Quote>(sql, new {lang});
            }
        }

        public async Task<Business.Models.Quote> GetDictum(string uuid)
        {
            using (var connection = ConfigurationExtensions.GetConnection(_configuration))
            {
                var sql = $@"
                     SELECT {QuoteSchema.Columns.Uuid},
                            {QuoteSchema.Columns.Text},
                            {QuoteSchema.Columns.Author}
                     FROM   {QuoteSchema.Table}
                     WHERE  {QuoteSchema.Columns.Uuid} = @{nameof(uuid)}";

                return await connection.QueryFirstOrDefaultAsync<Business.Models.Quote>(sql, new {uuid});
            }
        }
    }
}