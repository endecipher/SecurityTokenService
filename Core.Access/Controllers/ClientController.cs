using Core.Access.Models;
using Core.Access.Models.Strategy;
using Microsoft.AspNetCore.Mvc;

namespace Core.Access.Controllers
{
    public class ClientController : AbstractController<ClientModel>
    {
        [HttpGet]
        public IActionResult Index(string message = null)
        {
            return Ok(message ?? Resource.ClientIndexMessage);
        }

        /// <summary>
        /// This route is used to register Client Applications who want to participate in the OAuth 2.0 flow
        /// </summary>
        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            return View(new ClientModel
            {
                ReturnUrl = returnUrl
            });
        }

        /// <summary>
        /// The form displayed by the GET /client/register will post to this endpoint with the credentials selected
        /// We will create a client and send them the client_id and secret back.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(ClientModel model, [FromServices] IStrategy<ClientModel, OnClientGenerationContext> strategy)
        {
            ViewData.Clear();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = strategy.Execute(model).GetAwaiter().GetResult();

            return ParseResult(result).GetAwaiter().GetResult();
        }
    }
}
