using Core.Access.Models;
using Core.Access.Models.Strategy;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Core.Access.Controllers
{
    public class UserController : AbstractController<UserModel>
    {
        [HttpGet]
        public IActionResult Index(string message = null)
        {
            return Ok($"{message ?? Resource.UserIndexMessage}");
        }

        /// <summary>
        /// This route is used to register Users/ResourceOwners who want to participate in the OAuth 2.0 flow
        /// </summary>
        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            return View(new UserModel
            {
                ReturnUrl = returnUrl
            });
        }

        /// <summary>
        /// The form displayed by the GET /user/register will post to this endpoint with the credentials selected
        /// We will create a user and persist claims 
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserModel model, [FromServices] IStrategy<UserModel, OnUserGenerationContext> strategy)
        {
            ViewData.Clear();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await strategy.Execute(model);

            return await ParseResult(result);
        }
    }
}
