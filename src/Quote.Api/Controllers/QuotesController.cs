using Dictum.Business.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
        public Task<ActionResult<Business.Models.Quote>> GetRandom([FromQuery(Name = "l")] string languageCode)
        {
            return WrapToActionResult(() => _quoteService.GetRandom(languageCode));
        }

        [HttpGet("{uuid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public Task<ActionResult<Business.Models.Quote>> Get(string uuid)
        {
            return WrapToActionResult(() => _quoteService.GetDictum(uuid));
        }
    }
}