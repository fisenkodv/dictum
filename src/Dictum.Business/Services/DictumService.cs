using Dictum.Business.Anstract.Repositories;
using System.Threading.Tasks;

namespace Dictum.Business.Services
{
    public class DictumService
    {
        private readonly IDictumRepository _dictumRepository;

        public DictumService(IDictumRepository dictumRepository)
        {
            _dictumRepository = dictumRepository;
        }

        public Task<Models.Dictum> GetRandom()
        {
            return _dictumRepository.GetRandom();
        }

        public Task<Models.Dictum> GetDictum(string uuid)
        {
            return _dictumRepository.GetDictum(uuid);
        }
    }
}
