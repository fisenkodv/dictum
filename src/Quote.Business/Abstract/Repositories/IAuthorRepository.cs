using System.Collections.Generic;
using System.Threading.Tasks;
using Dictum.Business.Models;

namespace Dictum.Business.Abstract.Repositories
{
    public interface IAuthorRepository
    {
        Task<Author> Create(string name, Language language);
        Task<Author> Get(string name);
        Task<IEnumerable<Author>> SearchByName(string name, int page, int count);
    }
}