using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dictum.Business;
using Microsoft.AspNetCore.Mvc;

namespace Dictum.Api.Controllers
{
    public class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        protected async Task<ActionResult<TOut>> WrapToActionResult<TIn, TOut>(
            Func<Task<TIn>> resultFunc)
        {
            return await WrapToActionResult(resultFunc, Mapper.Map<TIn, TOut>);
        }

        protected async Task<ActionResult<IEnumerable<TOut>>> WrapToActionResult<TIn, TOut>(
            Func<Task<IEnumerable<TIn>>> resultFunc)
        {
            return await WrapToActionResult(resultFunc, Mapper.MapCollection<TIn, TOut>);
        }

        private async Task<ActionResult<TOut>> WrapToActionResult<TIn, TOut>(
            Func<Task<TIn>> resultFunc,
            Func<TIn, TOut> mapFunc)
        {
            var result = await resultFunc();
            var model = mapFunc(result);

            if (result != null) return Ok(model);

            return NotFound();
        }
    }
}