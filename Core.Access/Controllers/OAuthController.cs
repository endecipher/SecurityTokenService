using Core.Access.Models;
using Core.Access.Models.Strategy;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Core.Access.Controllers
{
    public class OAuthController : AbstractController<OAuthModel>
    {
        [HttpGet]
        public string Index()
        {
            return Resource.ServerAuthWorking;
        }

        #region OAuth 2.0 Workflows

        /// <summary>
        /// This will be used for the first request from the client to Core.Access to initiate the code flow
        /// The Users will be shown a page where they are prompted to provide the necessary details from the View 
        /// Precursor to fetch the Authorization Code in OAuth2.0 flow 
        /// </summary>
        /// <param name="response_type"> Authorization flow type. For impicit Flow, it's "token", but for our example the Authorization Code Flow, it's "code"</param>
        /// <param name="client_id"> Client Unique Id already known to both the Client and Core.Access Server prior to establiching this request. </param>
        /// <param name="redirect_uri"> Redirect URI with specific callback endpoint of client. </param>
        /// <param name="scope"> The info requested. For now, I have no filters. All claims would be provided in the token. </param>
        /// <param name="state"> A Random Value generated to confirm client identity. </param>
        [HttpGet]
        public async Task<IActionResult> Authorize([FromServices] IStrategy<OAuthModel, OnAuthAttemptContext> strategy) 
        {
            var result = await strategy.Execute();

            return await ParseResult(result);
        }


        /// <summary>
        /// This endpoint handles the POST request to confirm authorization from the end-user
        /// The Authorization Code is sent as back to the Client Redirect URI to start the BackChannel communication for exchange of tokens 
        /// </summary>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Authorize(OAuthModel model, [FromServices] IStrategy<OAuthModel, OnAuthCodeGenerationContext> strategy)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await strategy.Execute(model);

            return await ParseResult(result);
        }



        /// <summary>
        /// BackChannel for the Clients to get the AccessToken after they received the Authorization Code
        /// The same endpoint will be called if a refresh token request is invoked
        /// </summary>
        /// <param name="grant_type"> Flow of access_token request. Either "authorization_code" or "refresh_token" is supported. </param>
        /// <param name="code"> Confirmation Code for the authorization process retrieved in the previous step. </param>
        /// <param name="redirect_uri"> Redirect URI with specific callback endpoint of client. </param>
        /// <param name="client_id"> Same Client Unique Id. </param>
        /// <param name="refresh_token"> Additional Refresh Token from Client in case access_token is invalid for "refresh_token" flow. </param>
        [HttpPost]
        public async Task<IActionResult> Token([FromServices] IStrategy<OAuthModel, OnTokenExchangeContext> strategy)
        {
            var result = await strategy.Execute();

            return await ParseResult(result);
        }

        /// <summary>
        /// This endpoint will be used to validate access tokens from Resource Servers
        /// </summary>
        public IActionResult Validate([FromServices] IStrategy<OAuthModel, OnTokenValidationContext> strategy)
        {
            var result = strategy.Execute().GetAwaiter().GetResult();

            return ParseResult(result).GetAwaiter().GetResult();
        }
        #endregion

    }
}
