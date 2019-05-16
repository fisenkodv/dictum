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

        public async Task<IEnumerable<Author>> GetAuthors(string query, int page, int count)
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
                     WHERE      {AuthorNameSchema.Table}.{AuthorNameSchema.Columns.Name} LIKE '%@{nameof(query)}%'
                     ORDER BY   {AuthorNameSchema.Table}.{AuthorNameSchema.Columns.Name} ASC
                     LIMIT @{nameof(count)} OFFSET @{nameof(offset)}";

                return await connection.QueryAsync<Author>(sql, new {query, count, offset});
            }
        }
    }
}