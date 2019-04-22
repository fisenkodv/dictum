using System.Threading.Tasks;
using Dictum.Business.Models;

namespace Dictum.Business.Anstract.Repositories
{
    public interface IDictumRepository
    {
        Task<Models.Dictum> GetRandom();
        Task<Models.Dictum> GetDictum(string uuid);
    }
}
