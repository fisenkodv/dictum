using System.Threading.Tasks;
using Dapper;
using Dictum.Business.Abstract.Repositories;
using Dictum.Data.Models;
using Microsoft.Extensions.Configuration;
using ConfigurationExtensions = Dictum.Data.Extensions.ConfigurationExtensions;
using Quote = Dictum.Business.Models.Quote;

namespace Dictum.Data.Repositories
{
    public class QuoteRepository : IQuoteRepository
    {
        private readonly IConfiguration _configuration;

        public QuoteRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Quote> GetRandom(string languageCode)
        {
            using (var connection = ConfigurationExtensions.GetConnection(_configuration))
            {
                var sql = $@"
                     SELECT     {QuoteSchema.Table}.{QuoteSchema.Columns.Uuid} AS Uuid,
                                {QuoteSchema.Table}.{QuoteSchema.Columns.Text} AS Text,
                                {AuthorNameSchema.Table}.{AuthorNameSchema.Columns.Name} AS Author
                     FROM       {QuoteSchema.Table} AS {QuoteSchema.Table}
                     INNER JOIN {LanguageSchema.Table} AS {LanguageSchema.Table}
                     ON         {QuoteSchema.Table}.{QuoteSchema.Columns.LanguageId} = {LanguageSchema.Table}.{LanguageSchema.Columns.Id}
                     INNER JOIN {AuthorSchema.Table} AS {AuthorSchema.Table}
                     ON         {QuoteSchema.Table}.{QuoteSchema.Columns.AuthorId} = {AuthorSchema.Table}.{AuthorSchema.Columns.Id}
                     INNER JOIN {AuthorNameSchema.Table} AS {AuthorNameSchema.Table}
                     ON         {AuthorSchema.Table}.{AuthorSchema.Columns.Id} = {AuthorNameSchema.Table}.{AuthorNameSchema.Columns.AuthorId}
                     AND        {QuoteSchema.Table}.{QuoteSchema.Columns.LanguageId} = {AuthorNameSchema.Table}.{AuthorNameSchema.Columns.LanguageId}
                     WHERE      {LanguageSchema.Columns.Code} = @{nameof(languageCode)}
                     ORDER BY RAND() LIMIT 1";

                return await connection.QueryFirstOrDefaultAsync<Quote>(sql, new {languageCode});
            }
        }

        public async Task<Quote> GetDictum(string uuid)
        {
            using (var connection = ConfigurationExtensions.GetConnection(_configuration))
            {
                var sql = $@"
                     SELECT     {QuoteSchema.Table}.{QuoteSchema.Columns.Uuid} AS Uuid,
                                {QuoteSchema.Table}.{QuoteSchema.Columns.Text} AS Text,
                                {AuthorNameSchema.Table}.{AuthorNameSchema.Columns.Name} AS Author
                     FROM       {QuoteSchema.Table} AS {QuoteSchema.Table}
                     INNER JOIN {LanguageSchema.Table} AS {LanguageSchema.Table}
                     ON         {QuoteSchema.Table}.{QuoteSchema.Columns.LanguageId} = {LanguageSchema.Table}.{LanguageSchema.Columns.Id}
                     INNER JOIN {AuthorSchema.Table} AS {AuthorSchema.Table}
                     ON         {QuoteSchema.Table}.{QuoteSchema.Columns.AuthorId} = {AuthorSchema.Table}.{AuthorSchema.Columns.Id}
                     INNER JOIN {AuthorNameSchema.Table} AS {AuthorNameSchema.Table}
                     ON         {AuthorSchema.Table}.{AuthorSchema.Columns.Id} = {AuthorNameSchema.Table}.{AuthorNameSchema.Columns.AuthorId}
                     AND        {QuoteSchema.Table}.{QuoteSchema.Columns.LanguageId} = {AuthorNameSchema.Table}.{AuthorNameSchema.Columns.LanguageId}
                     WHERE      {QuoteSchema.Table}.{QuoteSchema.Columns.Uuid} = @{nameof(uuid)}";

                return await connection.QueryFirstOrDefaultAsync<Quote>(sql, new {uuid});
            }
        }
    }
}