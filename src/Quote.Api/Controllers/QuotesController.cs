using System.Collections.Generic;
using System.Threading.Tasks;
using Dictum.Business.Models.Domain;
using Dictum.Business.Models.Dto;
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
        public Task<ActionResult<QuoteDto>> GetRandom([FromQuery(Name = "l")] string? languageCode)
        {
            return WrapToActionResult<Quote, QuoteDto>(() => _quoteService.GetRandomQuote(languageCode));
        }

        [HttpGet("{uuid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public Task<ActionResult<QuoteDto>> Get(string uuid)
        {
            return WrapToActionResult<Quote, QuoteDto>(() => _quoteService.GetQuoteById(uuid));
        }

        [HttpGet("author/{uuid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public Task<ActionResult<IEnumerable<QuoteDto>>> GetAuthorQuotes(
            string uuid,
            [FromQuery(Name = "l")] string? languageCode,
            [FromQuery(Name = "p")] int? page,
            [FromQuery(Name = "c")] int? count
        )
        {
            return WrapToActionResult<Quote, QuoteDto>(() => _quoteService.GetAuthorQuotes(languageCode, uuid, page, count));
        }
    }
}