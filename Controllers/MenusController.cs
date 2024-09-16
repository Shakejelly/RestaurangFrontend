using Microsoft.AspNetCore.Mvc;

namespace RestaurangFrontend.Controllers
{
    public class MenusController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
