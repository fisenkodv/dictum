using System.Threading.Tasks;
using Dictum.Business.Abstract.Repositories;

namespace Dictum.Business.Services
{
    public class DictumService
    {
        private const string DefaultLanguageCode = "EN";

        private readonly IDictumRepository _dictumRepository;

        public DictumService(IDictumRepository dictumRepository)
        {
            _dictumRepository = dictumRepository;
        }

        public Task<Models.Quote> GetRandom(string lang)
        {
            return _dictumRepository.GetRandom(string.IsNullOrWhiteSpace(lang) ? DefaultLanguageCode : lang);
        }

        public Task<Models.Quote> GetDictum(string uuid)
        {
            return _dictumRepository.GetDictum(uuid);
        }
    }
}