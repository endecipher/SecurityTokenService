using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Core.Access.Models.Strategy
{
    public abstract class StrategyActionResult : StrategyResult
    {
        public abstract Task<IActionResult> Process(HttpContext httpContext);
    }
}
