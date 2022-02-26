using Core.Access.Models;
using Core.Access.Models.Strategy;
using Core.Access.Models.Strategy.Results;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Core.Access.Controllers
{
    public abstract class AbstractController<TModel> : Controller where TModel : BaseModel, new()
    {
        /// <summary>
        /// Used for converting/processing the mvc action result from our DI registered strategies
        /// </summary>
        /// <param name="result"><see cref="IActionResult">Action Result of the controller</see></param>
        /// <returns></returns>
        protected async Task<IActionResult> ParseResult(StrategyResult result)
        {
            if (result.ShouldDisplayView)
            {
                var viewStrategyResult = (result as ViewableStrategyResult);

                if (viewStrategyResult.ClearModelState)
                {
                    ModelState.Clear();
                }

                if (viewStrategyResult.HasViewDataToSet)
                {
                    foreach (var property in viewStrategyResult.ViewDataAppends)
                    {
                        ViewData[property.Key] = property.Value;
                    }
                }

                if (viewStrategyResult.HasViewPath)
                {
                    return View(viewStrategyResult.ViewPath, viewStrategyResult.BaseModel as TModel);
                }

                return View((result as ViewableStrategyResult).BaseModel as TModel);
            }
            else
            {
                return await (result as StrategyActionResult).Process(HttpContext);
            }
        }
    }
}
