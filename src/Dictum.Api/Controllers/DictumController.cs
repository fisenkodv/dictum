using Dictum.Business.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

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
        public Task<ActionResult<Business.Models.Quote>> GetRandom([FromQuery] string lang)
        {
            return WrapToActionResult(() => _dictumService.GetRandom(lang));
        }

        [HttpGet("{uuid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public Task<ActionResult<Business.Models.Quote>> Get(string uuid)
        {
            return WrapToActionResult(() => _dictumService.GetDictum(uuid));
        }

        private async Task<ActionResult<T>> WrapToActionResult<T>(Func<Task<T>> resultFunc)
        {
            var result = await resultFunc();

            if (result != null) return Ok(result);

            return NotFound();
        }
    }
}