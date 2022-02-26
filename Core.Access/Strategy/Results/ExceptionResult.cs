using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Core.Access.Models.Strategy.Results
{
    public class ExceptionResult : StrategyActionResult
    {
        public ExceptionResult(Exception ex)
        {
            ShouldDisplayView = false;
            Ex = ex;
        }

        public Exception Ex { get; }

        public override async Task<IActionResult> Process(HttpContext httpContext)
        {
            var result = new ObjectResult(Ex);
            result.StatusCode = StatusCodes.Status500InternalServerError;

            return await Task.FromResult(result);
        }
    }
}
