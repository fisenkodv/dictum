using System.Threading.Tasks;
using Dictum.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dictum.Api.Controllers
{
    [Route("quotes/authors")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly QuoteService _quoteService;

        public AuthorsController(QuoteService quoteService)
        {
            _quoteService = quoteService;
        }

        [HttpGet]
        public Task GetAuthors(string lang)
        {
            //TODO
            return null;
        }
    }
}