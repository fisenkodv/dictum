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

        public Task<Quote> GetRandom(string languageCode)
        {
            return _quoteRepository.GetRandom(string.IsNullOrWhiteSpace(languageCode)
                ? DefaultLanguageCode
                : languageCode);
        }

        public Task<Quote> GetDictum(string uuid)
        {
            return _quoteRepository.GetDictum(uuid);
        }
    }
}