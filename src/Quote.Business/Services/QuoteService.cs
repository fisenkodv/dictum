using System.Collections.Generic;
using System.Threading.Tasks;
using Dictum.Business.Abstract.Repositories;
using Dictum.Business.Models;

namespace Dictum.Business.Services
{
    public class QuoteService
    {
        private const string DefaultLanguageCode = "EN";

        private readonly IQuoteRepository _quoteRepository;

        public QuoteService(IQuoteRepository quoteRepository)
        {
            _quoteRepository = quoteRepository;
        }

        public Task<Quote> GetRandomQuote(string languageCode)
        {
            return _quoteRepository.GetRandomQuote(string.IsNullOrWhiteSpace(languageCode)
                ? DefaultLanguageCode
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

        public Task<Quote> CreateQuote(Quote quote)
        {
            
        }
    }
}