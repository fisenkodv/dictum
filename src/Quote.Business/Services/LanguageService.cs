using System.Collections.Generic;
using System.Threading.Tasks;
using Dictum.Business.Abstract.Repositories;
using Dictum.Business.Models;

namespace Dictum.Business.Services
{
    public class LanguageService
    {
        public const string DefaultLanguageCode = "EN";

        private readonly ILanguageRepository _languageRepository;

        public LanguageService(ILanguageRepository languageRepository)
        {
            _languageRepository = languageRepository;
        }

        public Task<IEnumerable<Language>> GetLanguages()
        {
            return _languageRepository.GetLanguages();
        }

        public Task<Language> GetLanguage(string text)
        {
            var language = new Language
                {Code = DefaultLanguageCode, Description = "English"}; //TODO: implement logic to get correct language

            return Task.FromResult<Language>(language);
        }
    }
}