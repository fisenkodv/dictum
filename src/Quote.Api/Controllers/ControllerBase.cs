using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Dictum.Api.Controllers
{
    public class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        protected async Task<ActionResult<T>> WrapToActionResult<T>(Func<Task<T>> resultFunc)
        {
            var result = await resultFunc();

            if (result != null) return Ok(result);

            return NotFound();
        }
    }
}