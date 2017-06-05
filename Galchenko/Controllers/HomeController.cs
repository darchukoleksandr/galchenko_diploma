using Microsoft.AspNetCore.Mvc;

namespace Galchenko.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
