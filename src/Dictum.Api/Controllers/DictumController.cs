using System.Threading.Tasks;
using Dictum.Business.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dictum.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DictumController : ControllerBase
    {
        private readonly DictumService _dictumService;

        public DictumController(DictumService dictumService)
        {
            _dictumService = dictumService;
        }

        [HttpGet]
        public async Task<ActionResult<Business.Models.Quote>> GetRandom([FromQuery] string lang)
        {
            return Ok(await _dictumService.GetRandom(lang));
        }

        [HttpGet("{uuid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Business.Models.Quote>> Get(string uuid)
        {
            var dictum = await _dictumService.GetDictum(uuid);

            if (dictum != null) return Ok(dictum);

            return NotFound();
        }
    }
}