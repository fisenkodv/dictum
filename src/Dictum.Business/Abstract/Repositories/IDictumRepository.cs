using System.Threading.Tasks;

namespace Dictum.Business.Abstract.Repositories
{
    public interface IDictumRepository
    {
        Task<Models.Dictum> GetRandom(string lang);
        Task<Models.Dictum> GetDictum(string uuid);
    }
}
