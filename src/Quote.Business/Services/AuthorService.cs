using System.Collections.Generic;
using System.Threading.Tasks;
using Dictum.Business.Abstract.Repositories;
using Dictum.Business.Models;

namespace Dictum.Business.Services
{
    public class AuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly LanguageService _languageService;

        public AuthorService(IAuthorRepository authorRepository, LanguageService languageService)
        {
            _authorRepository = authorRepository;
            _languageService = languageService;
        }

        public Task<IEnumerable<Author>> GetAuthors(string query, int? page, int? count)
        {
            var paging = Paging.Create(page, count);

            return _authorRepository.GetAuthors(query, paging.Page, paging.Count);
        }

        public async Task<Author> CreateAuthor(string name)
        {
            var author = await _authorRepository.GetAuthor(name);
            if (author != null) return author;

            var language = await _languageService.GetLanguage(name);
            author = await _authorRepository.CreateAuthor(name, language);

            return author;
        }
    }
}