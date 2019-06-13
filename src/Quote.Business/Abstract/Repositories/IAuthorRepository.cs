using System.Collections.Generic;
using System.Threading.Tasks;
using Dictum.Business.Models;

namespace Dictum.Business.Abstract.Repositories
{
    public interface IAuthorRepository
    {
        Task<Author> GetAuthor(string name);
        Task<IEnumerable<Author>> GetAuthors(string query, int page, int count);
        Task<string> CreateAuthor(string name, string languageCode);
    }
}