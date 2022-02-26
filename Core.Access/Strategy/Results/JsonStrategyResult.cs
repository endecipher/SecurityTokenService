using Core.Access.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static Core.Access.Utility.Strings;

namespace Core.Access.Models.Strategy.Results
{
    public class JsonStrategyResult : StrategyActionResult
    {
        public JsonStrategyResult(JsonResponse jsonResponse)
        {
            ShouldDisplayView = false;
            JsonResponse = jsonResponse;
        }

        public JsonResponse JsonResponse { get; }

        public override async Task<IActionResult> Process(HttpContext httpContext)
        {
            httpContext.Response.ContentType = Common.JsonContentType;

            await httpContext.Response.WriteAsJsonAsync(JsonResponse);
            await httpContext.Response.CompleteAsync();

            return await Task.FromResult(new OkResult());
        }
    }
}
