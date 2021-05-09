using Microsoft.AspNetCore.Mvc;

namespace JW.KS.API.Controllers
{
    public class HomeController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}