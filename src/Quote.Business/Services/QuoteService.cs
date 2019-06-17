using System.Collections.Generic;
using System.Threading.Tasks;
using Dictum.Business.Abstract.Repositories;
using Dictum.Business.Models;

namespace Dictum.Business.Services
{
    public class QuoteService
    {
        private readonly AuthorService _authorService;
        private readonly LanguageService _languageService;
        private readonly IQuoteRepository _quoteRepository;

        public QuoteService(
            AuthorService authorService,
            LanguageService languageService,
            IQuoteRepository quoteRepository)
        {
            _authorService = authorService;
            _languageService = languageService;
            _quoteRepository = quoteRepository;
        }

        public Task<Quote> GetRandomQuote(string languageCode)
        {
            return _quoteRepository.GetRandomQuote(string.IsNullOrWhiteSpace(languageCode)
                ? LanguageService.DefaultLanguageCode
                : languageCode);
        }

        public Task<Quote> GetQuoteById(string uuid)
        {
            return _quoteRepository.GetQuoteById(uuid);
        }

        public Task<IEnumerable<Quote>> GetAuthorQuotes(string authorUuid, int? page, int? count)
        {
            var paging = Paging.Create(page, count);

            return _quoteRepository.GetAuthorQuotes(authorUuid, paging.Page, paging.Count);
        }

        public async Task<Quote> CreateQuote(Quote quote)
        {
            var author = await _authorService.CreateAuthor(quote.Author);
            var language = await _languageService.GetLanguage(quote.Text);
            return await _quoteRepository.CreateQuote(quote, author, language);
        }
    }
}