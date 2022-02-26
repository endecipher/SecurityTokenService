using Core.Access.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static Core.Access.Utility.Strings;

namespace Core.Access.Models.Strategy.Results
{
    public class OkStrategyResult : StrategyActionResult
    {
        public OkStrategyResult()
        {
            ShouldDisplayView = false;
        }

        public override async Task<IActionResult> Process(HttpContext httpContext)
        {
            return await Task.FromResult(new OkResult());
        }
    }
}
