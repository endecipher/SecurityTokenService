using Core.UserClient.Common;
using Core.UserClient.Models;
using Core.UserClient.Utility;
using Core.UserClient.Utility.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Core.Client.Controllers
{
    [Authorize(AuthenticationSchemes = Policies.AvailableSchemes)]
    public class SecretController : Controller
    {
        private const string SharedJokeView = "~/Views/Secret/Joke.cshtml";

        public IGeneralHttpClient HttpClient { get; }

        public SecretController(IGeneralHttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        /// <summary>
        /// Landing Page for Authenticated Users
        /// </summary>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult DadJokes([FromServices] IJoker joker)
        {
            return View(SharedJokeView, model: new MessageModel
            {
                Message = joker.GetRandomDadJoke()
            });
        }

        [HttpGet]
        [Authorize(policy: Policies.AdultPolicy)]
        public IActionResult NSFWChuckNorrisJokes([FromServices] IJoker joker)
        {
            return View(SharedJokeView, model: new MessageModel
            {
                Message = joker.GetRandomChuckNorrisJoke(unSafe: true)
            });
        }

        [HttpGet]
        public IActionResult SFWChuckNorrisJokes([FromServices] IJoker joker)
        {
            return View(SharedJokeView, model: new MessageModel
            {
                Message = joker.GetRandomChuckNorrisJoke(unSafe: false)
            });
        }

        [HttpGet]
        [Authorize(policy: Policies.OAuthSignedInPolicy)]
        public async Task<IActionResult> KnockKnockJokes([FromServices] IConfiguration configuration)
        {
            var message = await HttpClient.FireWithAccessToken(configuration["ResourceServers:Core.ResourceApi:Endpoint"]);
            return View(SharedJokeView, model: new MessageModel
            {
                Message = await message.Content.ReadAsStringAsync()
            });
        }

        [HttpGet]
        [Authorize(policy: Policies.HighestSecurityPolicy)]
        public IActionResult YoMommaJokes([FromServices] IJoker joker)
        {
            return View(SharedJokeView, model: new MessageModel
            {
                Message = joker.GetRandomYoMommaJoke()
            });
        }
    }
}
