using System.Threading.Tasks;
using Dictum.Business.Models;

namespace Dictum.Business.Abstract.Repositories
{
    public interface IQuoteRepository
    {
        Task<Quote> GetRandom(string languageCode);
        Task<Quote> GetDictum(string uuid);
    }
}