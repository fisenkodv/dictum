using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Dictum.Business.Abstract.Repositories;
using Dictum.Data.Models;
using Microsoft.Extensions.Configuration;
using Author = Dictum.Business.Models.Author;
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

        public async Task<Author> Get(string name)
        {
            using (var connection = ConfigurationExtensions.GetConnection(_configuration))
            {
                var sql = $@"
                     SELECT     {AuthorSchema.Table}.{AuthorSchema.Columns.Uuid} AS Uuid,
                                {AuthorNameSchema.Table}.{AuthorNameSchema.Columns.Name} AS Name
                     FROM       {AuthorSchema.Table} AS {AuthorSchema.Table}
                     INNER JOIN {AuthorNameSchema.Table} AS {AuthorNameSchema.Table}
                     ON         {AuthorNameSchema.Table}.{AuthorNameSchema.Columns.AuthorId} = {AuthorSchema.Table}.{AuthorSchema.Columns.Id}
                     WHERE      {AuthorNameSchema.Table}.{AuthorNameSchema.Columns.Name} = @{nameof(name)}
                     LIMIT      1";

                return await connection.QueryFirstOrDefaultAsync<Author>(sql, new {name});
            }
        }

        public async Task<IEnumerable<Author>> SearchByName(string name, int page, int count)
        {
            using (var connection = ConfigurationExtensions.GetConnection(_configuration))
            {
                var offset = page * count;
                var sql = $@"
                     SELECT     {AuthorSchema.Table}.{AuthorSchema.Columns.Uuid} AS Uuid,
                                {AuthorNameSchema.Table}.{AuthorNameSchema.Columns.Name} AS Name
                     FROM       {AuthorSchema.Table} AS {AuthorSchema.Table}
                     INNER JOIN {AuthorNameSchema.Table} AS {AuthorNameSchema.Table}
                     ON         {AuthorNameSchema.Table}.{AuthorNameSchema.Columns.AuthorId} = {AuthorSchema.Table}.{AuthorSchema.Columns.Id}
                     WHERE      {AuthorNameSchema.Table}.{AuthorNameSchema.Columns.Name} LIKE CONCAT('%',@{nameof(name)},'%')
                     ORDER BY   {AuthorNameSchema.Table}.{AuthorNameSchema.Columns.Name} ASC
                     LIMIT      @{nameof(count)} OFFSET @{nameof(offset)}";

                return await connection.QueryAsync<Author>(sql, new {query = name, count, offset});
            }
        }

        public async Task<Author> Create(string name, Business.Models.Language language)
        {
            using (var connection = ConfigurationExtensions.GetConnection(_configuration))
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    var authorUuid = IdGenerator.Instance.Next();
                    var insertAuthorSql = $@"
                        INSERT INTO {AuthorSchema.Table} ({AuthorSchema.Columns.Uuid})
                        VALUES      (@{nameof(authorUuid)})";

                    await connection.ExecuteAsync(insertAuthorSql, new {authorUuid}, transaction);

                    var languageCode = language.Code;
                    var insertAuthorNameSql = $@"
                        SELECT @author_id;
                        SELECT {AuthorSchema.Table}.{AuthorSchema.Columns.Id} INTO @author_id 
                        FROM   {AuthorSchema.Table} AS {AuthorSchema.Table}
                        WHERE  {AuthorSchema.Table}.{AuthorSchema.Columns.Uuid} = @{nameof(authorUuid)};

                        SELECT @language_id;
                        SELECT {LanguageSchema.Table}.{LanguageSchema.Columns.Id} INTO @language_id 
                        FROM   {LanguageSchema.Table} AS {LanguageSchema.Table}
                        WHERE  {LanguageSchema.Table}.{LanguageSchema.Columns.Code} = @{nameof(languageCode)};

                        INSERT INTO {AuthorNameSchema.Table} 
                        (
                            {AuthorNameSchema.Columns.Name},
                            {AuthorNameSchema.Columns.AuthorId},
                            {AuthorNameSchema.Columns.LanguageId}
                        )
                        VALUES (@{nameof(name)}, @author_id, @language_id)";

                    await connection.ExecuteAsync(
                        insertAuthorNameSql,
                        new {name, authorUuid, languageCode},
                        transaction);

                    transaction.Commit();

                    return new Author {Uuid = authorUuid, Name = name};
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return null;
                }
            }
        }
    }
}