using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Dictum.Business.Abstract.Repositories;
using Dictum.Data.Models;
using Microsoft.Extensions.Configuration;
using Author = Dictum.Business.Models.Author;
using ConfigurationExtensions = Dictum.Data.Extensions.ConfigurationExtensions;
using Language = Dictum.Business.Models.Language;
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

        public async Task<Quote> Create(Quote quote, Author author, Language language)
        {
            using (var connection = ConfigurationExtensions.GetConnection(_configuration))
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    var quoteUuid = IdGenerator.Instance.Next();
                    var authorUuid = author.Uuid;
                    var quoteText = quote.Text;
                    var quoteHash = HashGenerator.SHA256(quote.Text);
                    var languageCode = language.Code;
                    var addedAt = DateTime.UtcNow;

                    var insertQuoteSql = $@"
                        SELECT @author_id;
                        SELECT {AuthorSchema.Table}.{AuthorSchema.Columns.Id} INTO @author_id 
                        FROM   {AuthorSchema.Table} AS {AuthorSchema.Table}
                        WHERE  {AuthorSchema.Table}.{AuthorSchema.Columns.Uuid} = @{nameof(authorUuid)};

                        SELECT @language_id;
                        SELECT {LanguageSchema.Table}.{LanguageSchema.Columns.Id} INTO @language_id 
                        FROM   {LanguageSchema.Table} AS {LanguageSchema.Table}
                        WHERE  {LanguageSchema.Table}.{LanguageSchema.Columns.Code} = @{nameof(languageCode)};

                        INSERT INTO {QuoteSchema.Table} 
                        (
                            {QuoteSchema.Columns.Uuid},
                            {QuoteSchema.Columns.Text},
                            {QuoteSchema.Columns.Hash},
                            {QuoteSchema.Columns.AuthorId},
                            {QuoteSchema.Columns.LanguageId},
                            {QuoteSchema.Columns.AddedAt}
                        )
                        VALUES (@{nameof(quoteUuid)}, @{nameof(quoteText)}, @{nameof(quoteHash)}, @author_id, @language_id, @{nameof(addedAt)})";

                    await connection.ExecuteAsync(insertQuoteSql, new
                    {
                        quoteUuid,
                        quoteText,
                        quoteHash,
                        authorUuid,
                        languageCode,
                        addedAt
                    }, transaction);

                    transaction.Commit();

                    return new Quote { Uuid = quoteUuid, Text = quoteText };
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return null;
                }
            }
        }

        public async Task<Quote> GetById(string uuid)
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

                return await connection.QueryFirstOrDefaultAsync<Quote>(sql, new { uuid });
            }
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

                return await connection.QueryFirstOrDefaultAsync<Quote>(sql, new { languageCode });
            }
        }

        public async Task<IEnumerable<Quote>> GetByAuthor(string authorUuid, int page, int count)
        {
            using (var connection = ConfigurationExtensions.GetConnection(_configuration))
            {
                var offset = page * count;
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
                     WHERE      {AuthorSchema.Table}.{AuthorSchema.Columns.Uuid} = @{nameof(authorUuid)}
                     LIMIT      @{nameof(count)} OFFSET @{nameof(offset)}";

                return await connection.QueryAsync<Quote>(sql, new { authorUuid, count, offset });
            }
        }
    }
}