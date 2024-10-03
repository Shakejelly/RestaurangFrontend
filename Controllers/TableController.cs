using Microsoft.AspNetCore.Mvc;

namespace RestaurangFrontend.Controllers
{
    public class TableController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
