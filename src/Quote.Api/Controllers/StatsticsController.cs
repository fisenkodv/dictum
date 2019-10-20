using System.Threading.Tasks;
using Dictum.Business.Models.Domain;
using Dictum.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dictum.Api.Controllers
{
    [ApiController]
    [Route("statistics")]
    public class StatisticsController : ControllerBase
    {
        private readonly StatisticsService _statisticsService;

        public StatisticsController(StatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }
        
        [HttpGet]
        public Task<ActionResult<Statistics>> GetStatistics()
        {
            return WrapToActionResult<Statistics, Statistics>(() => _statisticsService.GetStatistics());
        }
    }
}