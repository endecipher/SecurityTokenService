using Core.UserClient;
using Core.UserClient.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Core.Client.Controllers
{
    public class HomeController : Controller
    {
        public IConfiguration Configuration { get; }

        public HomeController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index(MessageModel model = null)
        {
            model.Message ??= Resource.UserClientIndex + Configuration["AuthorizationServers:Core.Access:Client_Id"];

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
