using Core.ResourceApi;
using Microsoft.AspNetCore.Mvc;

namespace Core.Web.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {
        [HttpGet("")]
        public string Index()
        {
            return Resource.ServerUp;
        }
    }
}
