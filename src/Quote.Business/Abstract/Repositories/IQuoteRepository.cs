using System.Threading.Tasks;

namespace Dictum.Business.Abstract.Repositories
{
    public interface IQuoteRepository
    {
        Task<Models.Quote> GetRandom(string languageCode);
        Task<Models.Quote> GetDictum(string uuid);
    }
}