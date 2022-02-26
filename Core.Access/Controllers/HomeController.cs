using Microsoft.AspNetCore.Mvc;

namespace Core.Access.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Main route for server availability
        /// </summary>
        [HttpGet]
        public IActionResult Index()
        {
            return View("LandingPage", Resource.ServerWorking);
        }

        /// <summary>
        /// Conventional About section
        /// </summary>
        [HttpGet]
        public IActionResult About()
        {
            return View("LandingPage", Resource.About);
        }
    }
}
