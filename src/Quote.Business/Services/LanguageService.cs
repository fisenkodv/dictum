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

        public Task<IEnumerable<Language>> GetAll()
        {
            return _languageRepository.GetLanguages();
        }

        public Task<Language> Detect(string text)
        {
            //TODO: implement logic to get correct language
            var language = new Language
            {
                Id = 8,
                Code = DefaultLanguageCode,
                Description = "English"
            };

            return Task.FromResult(language);
        }
    }
}