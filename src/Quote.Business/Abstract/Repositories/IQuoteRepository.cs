using System.Collections.Generic;
using System.Threading.Tasks;
using Dictum.Business.Models;

namespace Dictum.Business.Abstract.Repositories
{
    public interface IQuoteRepository
    {
        Task<Quote> GetRandom(string languageCode);
        Task<Quote> GetDictum(string uuid);
        Task<IEnumerable<Quote>> GetAuthorQuotes(string authorUuid, int page, int count);
    }
}