using Core.ResourceApi.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core.Web.Controllers
{
    [Route("[controller]")]
    public class SecretController : Controller
    {
        public SecretController(IJoker joker)
        {
            Joker = joker;
        }

        public IJoker Joker { get; }

        [Authorize]
        [HttpGet("")]
        [HttpGet("Index")]
        /// <summary>
        /// This endpoint will be called for Knock-Knock Jokes
        /// </summary>
        public string Index()
        {
            return Joker.GetRandomJoke();
        }
    }
}
