using System.Collections.Generic;
using System.Threading.Tasks;
using Dictum.Business.Models;

namespace Dictum.Business.Services
{
    public class AuthorsService
    {
        public Task<IEnumerable<Author>> GetAuthors(string query, string languageCode, int page, int count)
        {
            throw new System.NotImplementedException();
        }
    }
}