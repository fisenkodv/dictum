using System.Collections.Generic;
using System.Threading.Tasks;
using Dictum.Business.Models.Domain;

namespace Dictum.Business.Abstract.Repositories
{
    public interface IQuoteRepository
    {
        Task<Quote> Create(Quote quote, Author author, Language language);
        Task<Quote> GetById(string uuid);
        Task<Quote> GetRandom(string languageCode);
        Task<IEnumerable<Quote>> GetByAuthor(string languageCode, string authorUuid, int page, int count);
        Task<QuotesStatistics> GetStatistics();
    }
}