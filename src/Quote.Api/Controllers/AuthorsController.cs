using System.Collections.Generic;
using System.Threading.Tasks;
using Dictum.Business.Models;
using Dictum.Business.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dictum.Api.Controllers
{
    [ApiController]
    [Route("quotes/authors")]
    public class AuthorsController : ControllerBase
    {
        private readonly AuthorsService _authorsService;

        public AuthorsController(AuthorsService authorsService)
        {
            _authorsService = authorsService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public Task<ActionResult<IEnumerable<Author>>> GetAuthors(
            [FromQuery(Name = "q")] string query,
            [FromQuery(Name = "l")] string languageCode,
            [FromQuery(Name = "p")] int page,
            [FromQuery(Name = "c")] int count
        )
        {
            return WrapToActionResult(() => _authorsService.GetAuthors(query, languageCode, page, count));
        }
    }
}