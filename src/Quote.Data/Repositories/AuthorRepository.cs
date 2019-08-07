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

namespace Dictum.Data.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly IConfiguration _configuration;

        public AuthorRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Author> Create(string name, Language language)
        {
            using (var connection = ConfigurationExtensions.GetConnection(_configuration))
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    var authorUuid = IdGenerator.Instance.Next();
                    var insertAuthorSql = $@"
                        INSERT INTO {AuthorSchema.Table} ({AuthorSchema.Columns.Uuid})
                        VALUES      (@{nameof(authorUuid)});
                        SELECT LAST_INSERT_ID();";

                    var authorId = await connection.QueryFirstAsync<int>(insertAuthorSql, new { authorUuid }, transaction);

                    var languageId = language.Id;
                    var insertAuthorNameSql = $@"
                        INSERT INTO {AuthorNameSchema.Table} 
                        (
                            {AuthorNameSchema.Columns.Name},
                            {AuthorNameSchema.Columns.AuthorId},
                            {AuthorNameSchema.Columns.LanguageId}
                        )
                        VALUES (@{nameof(name)}, @{nameof(authorId)}, @{nameof(languageId)})";

                    await connection.ExecuteAsync(insertAuthorNameSql, new { name, authorId, languageId }, transaction);

                    transaction.Commit();

                    return new Author { Id = authorId, Uuid = authorUuid, Name = name };
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return null;
                }
            }
        }

        public async Task<Author> Get(string name)
        {
            using (var connection = ConfigurationExtensions.GetConnection(_configuration))
            {
                var sql = $@"
                     SELECT     {AuthorSchema.Table}.{AuthorSchema.Columns.Id} AS Id,
                                {AuthorSchema.Table}.{AuthorSchema.Columns.Uuid} AS Uuid,
                                {AuthorNameSchema.Table}.{AuthorNameSchema.Columns.Name} AS Name
                     FROM       {AuthorSchema.Table} AS {AuthorSchema.Table}
                     INNER JOIN {AuthorNameSchema.Table} AS {AuthorNameSchema.Table}
                     ON         {AuthorNameSchema.Table}.{AuthorNameSchema.Columns.AuthorId} = {AuthorSchema.Table}.{AuthorSchema.Columns.Id}
                     WHERE      {AuthorNameSchema.Table}.{AuthorNameSchema.Columns.Name} = @{nameof(name)}
                     LIMIT      1";

                return await connection.QueryFirstOrDefaultAsync<Author>(sql, new { name });
            }
        }

        public async Task<IEnumerable<Author>> SearchByName(string name, int page, int count)
        {
            using (var connection = ConfigurationExtensions.GetConnection(_configuration))
            {
                var offset = page * count;
                var sql = $@"
                     SELECT     {AuthorSchema.Table}.{AuthorSchema.Columns.Id} AS Id,
                                {AuthorSchema.Table}.{AuthorSchema.Columns.Uuid} AS Uuid,
                                {AuthorNameSchema.Table}.{AuthorNameSchema.Columns.Name} AS Name
                     FROM       {AuthorSchema.Table} AS {AuthorSchema.Table}
                     INNER JOIN {AuthorNameSchema.Table} AS {AuthorNameSchema.Table}
                     ON         {AuthorNameSchema.Table}.{AuthorNameSchema.Columns.AuthorId} = {AuthorSchema.Table}.{AuthorSchema.Columns.Id}
                     WHERE      {AuthorNameSchema.Table}.{AuthorNameSchema.Columns.Name} LIKE CONCAT('%',@{nameof(name)},'%')
                     ORDER BY   {AuthorNameSchema.Table}.{AuthorNameSchema.Columns.Name} ASC
                     LIMIT      @{nameof(count)} OFFSET @{nameof(offset)}";

                return await connection.QueryAsync<Author>(sql, new { name, count, offset });
            }
        }
    }
}