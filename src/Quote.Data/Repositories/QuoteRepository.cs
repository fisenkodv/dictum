using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Dictum.Business.Abstract.Repositories;
using Dictum.Business.Models.Domain;
using Dictum.Data.Models;
using Microsoft.Extensions.Configuration;
using Author = Dictum.Business.Models.Domain.Author;
using ConfigurationExtensions = Dictum.Data.Extensions.ConfigurationExtensions;
using Language = Dictum.Business.Models.Domain.Language;
using Quote = Dictum.Business.Models.Domain.Quote;

namespace Dictum.Data.Repositories
{
    public class QuoteRepository : IQuoteRepository
    {
        private readonly IConfiguration _configuration;

        public QuoteRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Quote?> Create(Quote quote, Author author, Language language)
        {
            await using var connection = ConfigurationExtensions.CreateOpenConnection(_configuration);
            await using var transaction = connection.BeginTransaction();
            try
            {
                var quoteHash = HashGenerator.Sha256(quote.Text);
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

                var quoteId = await connection.QueryFirstAsync<int>(
                    insertQuoteSql,
                    new { quoteUuid, quoteText, quoteHash, authorId, languageId, addedAt },
                    transaction);

                await transaction.CommitAsync();

                quote.Id = quoteId;
                quote.Uuid = quoteUuid;

                return quote;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await transaction.RollbackAsync();
                return null;
            }
        }

        public async Task<Quote> GetById(string uuid)
        {
            await using var connection = ConfigurationExtensions.CreateOpenConnection(_configuration);
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

        public async Task<Quote> GetRandom(string languageCode)
        {
            await using var connection = ConfigurationExtensions.CreateOpenConnection(_configuration);
            var languageIdSql = $@"
                    SELECT @languageId := {LanguageSchema.Table}.{LanguageSchema.Columns.Id}
                    FROM   {LanguageSchema.Table} AS {LanguageSchema.Table}
                    WHERE  {LanguageSchema.Table}.{LanguageSchema.Columns.Code} = @{nameof(languageCode)};";

            var minMaxIdSql = $@"
                    SELECT @min := MIN({QuoteSchema.Table}.{QuoteSchema.Columns.Id}),
                           @max := MAX({QuoteSchema.Table}.{QuoteSchema.Columns.Id})
                    FROM   {QuoteSchema.Table} AS {QuoteSchema.Table}
                    WHERE  {QuoteSchema.Table}.{QuoteSchema.Columns.LanguageId} = @languageId;";

            var sql = $@"
                     SELECT     {QuoteSchema.Table}.{QuoteSchema.Columns.Id} AS Id,
                                {QuoteSchema.Table}.{QuoteSchema.Columns.Uuid} AS Uuid,
                                {QuoteSchema.Table}.{QuoteSchema.Columns.Text} AS Text,
                                {AuthorNameSchema.Table}.{AuthorNameSchema.Columns.Name} AS Author
                     FROM       {QuoteSchema.Table} AS {QuoteSchema.Table}
                     LEFT JOIN  {AuthorNameSchema.Table} AS {AuthorNameSchema.Table}
                     ON         {QuoteSchema.Table}.{QuoteSchema.Columns.AuthorId} = {AuthorNameSchema.Table}.{AuthorNameSchema.Columns.AuthorId}
                     AND        {QuoteSchema.Table}.{QuoteSchema.Columns.LanguageId} = {AuthorNameSchema.Table}.{AuthorNameSchema.Columns.LanguageId}
                     JOIN (
                        SELECT {QuoteSchema.Columns.Id}
                        FROM (
                             SELECT   {QuoteSchema.Columns.Id}
                             FROM     (SELECT @min + (@max - @min + 1 - 50) * RAND() AS START FROM DUAL) AS INIT
                             JOIN     {QuoteSchema.Table} AS Y
                             WHERE    Y.{QuoteSchema.Columns.Id} > INIT.START
                             ORDER BY Y.{QuoteSchema.Columns.Id}
                             LIMIT 50 -- Inflated to deal with gaps
                        ) AS Z ORDER BY RAND()
                        LIMIT 1 -- number of rows desired (change to 1 if looking for a single row)
                     ) R
                     ON {QuoteSchema.Table}.{QuoteSchema.Columns.Id} = R.{QuoteSchema.Columns.Id}
                     AND {QuoteSchema.Table}.{QuoteSchema.Columns.LanguageId}=@languageId;";

            var languageId = await connection.QueryFirstAsync<int>(languageIdSql, new { languageCode });
            var (min, max) = await connection.QueryFirstAsync<(int min, int max)>(minMaxIdSql, new { languageId });
            return await connection.QueryFirstAsync<Quote>(sql, new { min, max, languageId });
        }

        public async Task<IEnumerable<Quote>> GetByAuthor(string languageCode, string authorUuid, int page, int count)
        {
            await using var connection = ConfigurationExtensions.CreateOpenConnection(_configuration);
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
                     AND        {LanguageSchema.Table}.{LanguageSchema.Columns.Code} = @{nameof(languageCode)}
                     LIMIT      @{nameof(count)} OFFSET @{nameof(offset)}";

            return await connection.QueryAsync<Quote>(sql, new { languageCode, authorUuid, count, offset });
        }

        public async Task<QuotesStatistics> GetStatistics()
        {
            await using var connection = ConfigurationExtensions.CreateOpenConnection(_configuration);
            var sql = $@"
                     SELECT    {LanguageSchema.Table}.{LanguageSchema.Columns.Code} AS code,
                               COUNT({QuoteSchema.Table}.{QuoteSchema.Columns.LanguageId}) AS count
                     FROM      {QuoteSchema.Table} AS {QuoteSchema.Table}
                     LEFT JOIN {LanguageSchema.Table} AS {LanguageSchema.Table}
                     ON        {QuoteSchema.Table}.{QuoteSchema.Columns.LanguageId} = {LanguageSchema.Table}.{LanguageSchema.Columns.Id}
                     GROUP BY  {QuoteSchema.Table}.{QuoteSchema.Columns.LanguageId}
                     ORDER BY  Count DESC";

            var statistics = (await connection.QueryAsync<(string code, int count)>(sql)).ToList();
            var result = new QuotesStatistics
            {
                Languages = statistics.ToDictionary(x => x.code, x => x.count),
                Total = statistics.Sum(x => x.count)
            };
            return result;
        }

        private async Task<Quote?> GetQuoteIdsByHash(string hash)
        {
            await using var connection = ConfigurationExtensions.CreateOpenConnection(_configuration);
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