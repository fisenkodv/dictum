using System.Collections.Generic;
using System.Threading.Tasks;
using Dictum.Business.Models;
using Dictum.Business.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dictum.Api.Controllers
{
    [ApiController]
    [Route("quotes")]
    public class QuotesController : ControllerBase
    {
        private readonly QuoteService _quoteService;

        public QuotesController(QuoteService quoteService)
        {
            _quoteService = quoteService;
        }

        [HttpGet]
        public Task<ActionResult<Quote>> GetRandom([FromQuery(Name = "l")] string languageCode)
        {
            return WrapToActionResult(() => _quoteService.GetRandomQuote(languageCode));
        }

        [HttpGet("{uuid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public Task<ActionResult<Quote>> Get(string uuid)
        {
            return WrapToActionResult(() => _quoteService.GetQuoteById(uuid));
        }

        [HttpGet("authors/{uuid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public Task<ActionResult<IEnumerable<Quote>>> GetAuthorQuotes(
            string uuid,
            [FromQuery(Name = "p")] int? page,
            [FromQuery(Name = "c")] int? count
        )
        {
            return WrapToActionResult(() => _quoteService.GetAuthorQuotes(uuid, page, count));
        }
    }
}