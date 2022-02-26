using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Core.Access.Models.Strategy.Results
{
    public class RedirectionStrategyResult : StrategyActionResult
    {
        public RedirectionStrategyResult(string uri)
        {
            ShouldDisplayView = false;
            Uri = uri;
        }

        public string Uri { get; }

        public override async Task<IActionResult> Process(HttpContext httpContext)
        {
            var result = new RedirectResult(Uri);
            return await Task.FromResult(result);
        }
    }
}
