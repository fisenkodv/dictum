using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dictum.Business.Abstract.Repositories;
using Dictum.Business.Models.Domain;

namespace Dictum.Tests
{
    public class FakeLanguageRepository : ILanguageRepository
    {
        public Task<Language> GetLanguage(string code)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Language>> GetLanguages()
        {
            throw new NotImplementedException();
        }
    }
}
