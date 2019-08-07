using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Dictum.Business.Abstract.Repositories;
using Dictum.Data.Models;
using Microsoft.Extensions.Configuration;
using Author = Dictum.Business.Models.Internal.Author;
using ConfigurationExtensions = Dictum.Data.Extensions.ConfigurationExtensions;
using Language = Dictum.Business.Models.Internal.Language;
using Quote = Dictum.Business.Models.Internal.Quote;

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
                    var quoteHash = HashGenerator.SHA256(quote.Text);
                    var quoteIds = await GetQuoteIdsByHash(quoteHash);
                    if (quoteIds != default)
                    {
                        quote.Id = quoteIds.Id;
                        quote.Uuid = quoteIds.Uuid;
                        return quote;
                    }

                    var quoteUuid = IdGenerator.Instance.Next();
                    var quoteText = quote.Text;
                    var authorId = author.Id;
                    var languageId = language.Id;
                    var addedAt = DateTime.UtcNow;

                    var insertQuoteSql = $@"
                        INSERT INTO {QuoteSchema.Table}
                        (
                            {QuoteSchema.Columns.Uuid},
                            {QuoteSchema.Columns.Text},
                            {QuoteSchema.Columns.Hash},
                            {QuoteSchema.Columns.AuthorId},
                            {QuoteSchema.Columns.LanguageId},
                            {QuoteSchema.Columns.AddedAt}
                        )
                        VALUES
                        (
                            @{nameof(quoteUuid)}, @{nameof(quoteText)}, @{nameof(quoteHash)},
                            @{nameof(authorId)}, @{nameof(languageId)}, @{nameof(addedAt)}
                        );
                        SELECT {QuoteSchema.Columns.Id} FROM {QuoteSchema.Table} WHERE {QuoteSchema.Columns.Uuid} = @{nameof(quoteUuid)};";

                    var quoteId = await connection.QueryFirstAsync<int>(insertQuoteSql, new
                    {
                        quoteUuid,
                        quoteText,
                        quoteHash,
                        authorId,
                        languageId,
                        addedAt
                    }, transaction);

                    transaction.Commit();

                    quote.Id = quoteId;
                    quote.Uuid = quoteUuid;

                    return quote;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
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
                     SELECT     {QuoteSchema.Table}.{QuoteSchema.Columns.Id} AS Id,
                                {QuoteSchema.Table}.{QuoteSchema.Columns.Uuid} AS Uuid,
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
                     SELECT     {QuoteSchema.Table}.{QuoteSchema.Columns.Id} AS Id,
                                {QuoteSchema.Table}.{QuoteSchema.Columns.Uuid} AS Uuid,
                                {QuoteSchema.Table}.{QuoteSchema.Columns.Text} AS Text,
                                {AuthorNameSchema.Table}.{AuthorNameSchema.Columns.Name} AS Author
                     FROM       {QuoteSchema.Table} AS {QuoteSchema.Table}
                     INNER JOIN {LanguageSchema.Table} AS {LanguageSchema.Table}
                     ON         {QuoteSchema.Table}.{QuoteSchema.Columns.LanguageId} = {LanguageSchema.Table}.{LanguageSchema.Columns.Id}
                     LEFT JOIN  {AuthorNameSchema.Table} AS {AuthorNameSchema.Table}
                     ON         {QuoteSchema.Table}.{QuoteSchema.Columns.AuthorId} = {AuthorNameSchema.Table}.{AuthorNameSchema.Columns.AuthorId}
                     AND        {QuoteSchema.Table}.{QuoteSchema.Columns.LanguageId} = {AuthorNameSchema.Table}.{AuthorNameSchema.Columns.LanguageId}
                     WHERE      {LanguageSchema.Table}.{LanguageSchema.Columns.Code} = @{nameof(languageCode)}
                     AND        {QuoteSchema.Table}.{QuoteSchema.Columns.Id} >= 
                        CAST(
                            RAND() * (
                                SELECT MAX({QuoteSchema.Table}_1.{QuoteSchema.Columns.Id}) 
                                FROM   {QuoteSchema.Table} AS {QuoteSchema.Table}_1
                                WHERE  {QuoteSchema.Table}.{QuoteSchema.Columns.LanguageId} = {QuoteSchema.Table}_1.{QuoteSchema.Columns.LanguageId}
                            ) 
                            AS INT)
                     LIMIT 1";

                return await connection.QueryFirstOrDefaultAsync<Quote>(sql, new { languageCode });
            }
        }

        public async Task<IEnumerable<Quote>> GetByAuthor(string authorUuid, int page, int count)
        {
            using (var connection = ConfigurationExtensions.GetConnection(_configuration))
            {
                var offset = page * count;
                var sql = $@"
                     SELECT     {QuoteSchema.Table}.{QuoteSchema.Columns.Id} AS Id,
                                {QuoteSchema.Table}.{QuoteSchema.Columns.Uuid} AS Uuid,
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

        private async Task<Quote> GetQuoteIdsByHash(string hash)
        {
            using (var connection = ConfigurationExtensions.GetConnection(_configuration))
            {
                var sql = $@"
                     SELECT {QuoteSchema.Table}.{QuoteSchema.Columns.Id},
                            {QuoteSchema.Table}.{QuoteSchema.Columns.Uuid}
                     FROM   {QuoteSchema.Table} AS {QuoteSchema.Table}
                     WHERE  {QuoteSchema.Table}.{QuoteSchema.Columns.Hash} = @{nameof(hash)}
                     LIMIT  1";

                return await connection.QueryFirstOrDefaultAsync<Quote>(sql, new { hash });
            }
        }
    }
}