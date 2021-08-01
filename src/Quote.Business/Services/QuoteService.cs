using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dictum.Business.Abstract.Repositories;
using Dictum.Business.Models.Domain;

namespace Dictum.Business.Services
{
    public class QuoteService
    {
        private readonly Random _random = new Random();
        private readonly AuthorService _authorService;
        private readonly LanguageService _languageService;
        private readonly IQuoteRepository _quoteRepository;
        private readonly IDictionary<string, IList<int>> _languageCodeToQuoteIds;

        public QuoteService(
            AuthorService authorService,
            LanguageService languageService,
            IQuoteRepository quoteRepository)
        {
            _authorService = authorService;
            _languageService = languageService;
            _quoteRepository = quoteRepository;
            _languageCodeToQuoteIds = new Dictionary<string, IList<int>>();
        }

        public async Task<Quote> GetRandomQuote(string? languageCode)
        {
            languageCode = string.IsNullOrWhiteSpace(languageCode)
                ? LanguageService.EnLanguage.Code
                : languageCode;

            if (!_languageCodeToQuoteIds.ContainsKey(languageCode))
            {
                IList<int> idsList = new List<int>(await _quoteRepository.GetQuoteIds(languageCode));
                _languageCodeToQuoteIds.Add(languageCode, idsList);
            }

            IList<int> ids = _languageCodeToQuoteIds[languageCode];
            int id = ids[_random.Next(ids.Count)];

            return await _quoteRepository.GetById(id);
        }

        public Task<Quote> GetQuoteById(string uuid)
        {
            return _quoteRepository.GetByUUID(uuid);
        }

        public Task<IEnumerable<Quote>> GetAuthorQuotes(string? languageCode, string authorUuid, int? page, int? count)
        {
            var paging = Paging.Create(page, count);
            languageCode = string.IsNullOrWhiteSpace(languageCode)
                ? LanguageService.EnLanguage.Code
                : languageCode;

            return _quoteRepository.GetByAuthor(languageCode, authorUuid, paging.Page, paging.Count);
        }

        public async Task<Quote?> CreateQuote(Quote quote)
        {
            var author = await _authorService.GetOrCreate(quote.Author);
            var language = await _languageService.Detect(quote.Text);
            return await _quoteRepository.Create(quote, author, language);
        }
    }
}