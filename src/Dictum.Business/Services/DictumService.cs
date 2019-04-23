using System.Threading.Tasks;
using Dictum.Business.Abstract.Repositories;

namespace Dictum.Business.Services
{
    public class DictumService
    {
        private readonly IDictumRepository _dictumRepository;

        public DictumService(IDictumRepository dictumRepository)
        {
            _dictumRepository = dictumRepository;
        }

        public Task<Models.Dictum> GetRandom(string lang)
        {
            return _dictumRepository.GetRandom(lang);
        }

        public Task<Models.Dictum> GetDictum(string uuid)
        {
            return _dictumRepository.GetDictum(uuid);
        }
    }
}
