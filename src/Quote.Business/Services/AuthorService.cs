using System.Collections.Generic;
using System.Threading.Tasks;
using Dictum.Business.Abstract.Repositories;
using Dictum.Business.Models;

namespace Dictum.Business.Services
{
    public class AuthorService
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorService(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public Task<IEnumerable<Author>> GetAuthors(string query, int? page, int? count)
        {
            var paging = Paging.Create(page, count);

            return _authorRepository.GetAuthors(query, paging.Page, paging.Count);
        }
    }
}