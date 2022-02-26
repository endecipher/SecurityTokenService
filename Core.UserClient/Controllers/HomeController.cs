using Core.UserClient;
using Core.UserClient.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core.Client.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index(MessageModel model = null)
        {
            model.Message ??= Resource.UserClientIndex;

            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult About()
        {
            return Index(model: new MessageModel { Message = Resource.AboutText });
        }
    }
}
