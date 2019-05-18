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
        private readonly AuthorService _authorService;

        public AuthorsController(AuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public Task<ActionResult<IEnumerable<Author>>> GetAuthors(
            [FromQuery(Name = "q")] string query,
            [FromQuery(Name = "p")] int? page,
            [FromQuery(Name = "c")] int? count
        )
        {
            return WrapToActionResult(() => _authorService.GetAuthors(query, page, count));
        }

        [HttpGet("{uuid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public Task<ActionResult<IEnumerable<Quote>>> GetAuthorQuotes(
            string authorUuid,
            [FromQuery(Name = "p")] int? page,
            [FromQuery(Name = "c")] int? count
        )
        {
            return WrapToActionResult(() => _authorService.GetAuthorQuotes(authorUuid, page, count));
        }
    }
}