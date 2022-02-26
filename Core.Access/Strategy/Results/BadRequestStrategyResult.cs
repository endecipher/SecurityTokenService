using Core.Access.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Core.Access.Models.Strategy.Results
{
    public class BadRequestStrategyResult : StrategyActionResult
    {
        public string error { get; init; }
        public string error_description { get; init; }

        public override async Task<IActionResult> Process(HttpContext httpContext)
        {
            httpContext.Response.ContentType = Strings.Common.UrlEncodedContentType;

            return await Task.FromResult(new BadRequestObjectResult(new
            {
                error,
                error_description
            }));
        }
    }
}
