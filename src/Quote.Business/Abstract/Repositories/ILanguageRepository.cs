using System.Collections.Generic;
using System.Threading.Tasks;
using Dictum.Business.Models;
using Dictum.Business.Models.Internal;

namespace Dictum.Business.Abstract.Repositories
{
    public interface ILanguageRepository
    {
        Task<IEnumerable<Language>> GetLanguages();

        Task<Language> GetLanguage(string code);
    }
}