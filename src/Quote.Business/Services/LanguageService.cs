using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dictum.Business.Abstract.Repositories;
using Dictum.Business.Models.Domain;

namespace Dictum.Business.Services
{
    public class LanguageService
    {
        public static readonly Language EnLanguage = new Language
        {
            Id = 8,
            Code = "EN",
            Description = "English",
            Alphabet = new HashSet<char>() { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'V', 'X', 'Y', 'Z' }
        };

        public static readonly Language RuLanguage = new Language
        {
            Id = 27,
            Code = "RU",
            Description = "Русский",
            Alphabet = new HashSet<char>() { 'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ё', 'Ж', 'З', 'И', 'Й', 'К', 'Л', 'М', 'Н', 'О', 'П', 'Р', 'С', 'Т', 'У', 'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ', 'Ъ', 'Ы', 'Ь', 'Э', 'Ю', 'Я' }
        };

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
            if (BelongsTo(text, EnLanguage))
                return Task.FromResult(EnLanguage);
            else if (BelongsTo(text, RuLanguage))
                return Task.FromResult(RuLanguage);

            return Task.FromResult<Language>(null);
        }

        private bool BelongsTo(string text, Language language)
        {
            int languageThreashold = (text.Length * 70) / 100; // if 70% of the text belongs to some language then we have detected the language
            foreach (var ch in text)
            {
                if (Char.IsLetter(ch) && language.Alphabet.Contains(char.ToUpper(ch))) languageThreashold--;
                if (languageThreashold == 0) return true;
            }

            return false;
        }
    }
}