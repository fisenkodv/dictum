using System.Threading.Tasks;
using Dictum.Business.Models;
using Dictum.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dictum.Api.Controllers
{
    [ApiController]
    [Route("quotes/languages")]
    public class LanguagesController : ControllerBase
    {
        private readonly LanguageService _languageService;

        public LanguagesController(LanguageService languageService)
        {
            _languageService = languageService;
        }

        [HttpGet]
        public async Task<ActionResult<Language>> GetLanguages()
        {
            return Ok(await _languageService.GetAll());
        }
    }
}