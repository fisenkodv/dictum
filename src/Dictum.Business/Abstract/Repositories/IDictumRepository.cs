using System.Threading.Tasks;

namespace Dictum.Business.Abstract.Repositories
{
    public interface IDictumRepository
    {
        Task<Models.Quote> GetRandom(string lang);
        Task<Models.Quote> GetDictum(string uuid);
    }
}