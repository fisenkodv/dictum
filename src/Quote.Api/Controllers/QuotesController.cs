using Dictum.Business.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
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
        public Task<ActionResult<Business.Models.Quote>> GetRandom([FromQuery] string lang)
        {
            return WrapToActionResult(() => _quoteService.GetRandom(lang));
        }

        [HttpGet("{uuid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public Task<ActionResult<Business.Models.Quote>> Get(string uuid)
        {
            return WrapToActionResult(() => _quoteService.GetDictum(uuid));
        }

        private async Task<ActionResult<T>> WrapToActionResult<T>(Func<Task<T>> resultFunc)
        {
            var result = await resultFunc();

            if (result != null) return Ok(result);

            return NotFound();
        }
    }
}