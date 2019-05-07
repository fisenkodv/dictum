using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Dictum.Api.Controllers
{
    [Route("quotes/authors")]
    [ApiController]
    public class QuoteAuthors : ControllerBase
    {
        public QuoteAuthors()
        {
        }

        public Task GetAuthors(string lang)
        {
            //TODO
            return null;
        }
    }
}