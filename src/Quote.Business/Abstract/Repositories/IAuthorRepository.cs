using System.Collections.Generic;
using System.Threading.Tasks;
using Dictum.Business.Models;

namespace Dictum.Business.Abstract.Repositories
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<Author>> GetAuthors(string query, int page, int count);
    }
}