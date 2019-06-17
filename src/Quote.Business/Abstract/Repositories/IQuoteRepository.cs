using System.Collections.Generic;
using System.Threading.Tasks;
using Dictum.Business.Models;

namespace Dictum.Business.Abstract.Repositories
{
    public interface IQuoteRepository
    {
        Task<Quote> GetRandomQuote(string languageCode);
        Task<Quote> GetQuoteById(string uuid);
        Task<IEnumerable<Quote>> GetAuthorQuotes(string authorUuid, int page, int count);
        Task<Quote> CreateQuote(Quote quote, Author author, Language language);
    }
}