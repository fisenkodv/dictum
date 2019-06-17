using System.Collections.Generic;
using System.Threading.Tasks;
using Dictum.Business.Abstract.Repositories;
using Dictum.Business.Models;

namespace Dictum.Business.Services
{
    public class AuthorService
    {
        private readonly LanguageService _languageService;
        private readonly IAuthorRepository _authorRepository;

        public AuthorService(LanguageService languageService, IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
            _languageService = languageService;
        }

        public Task<IEnumerable<Author>> Search(string query, int? page, int? count)
        {
            var paging = Paging.Create(page, count);

            return _authorRepository.SearchByName(query, paging.Page, paging.Count);
        }

        public async Task<Author> GetOrCreate(string name)
        {
            var author = await _authorRepository.Get(name);
            if (author != null) return author;

            var language = await _languageService.Detect(name);
            author = await _authorRepository.Create(name, language);

            return author;
        }
    }
}