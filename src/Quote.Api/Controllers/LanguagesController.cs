using System.Collections.Generic;
using System.Threading.Tasks;
using Dictum.Business.Models.Dto;
using Dictum.Business.Models.Internal;
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
        public Task<ActionResult<IEnumerable<LanguageDto>>> GetLanguages()
        {
            return WrapToActionResult<Language, LanguageDto>(() => _languageService.GetAll());
        }
    }
}