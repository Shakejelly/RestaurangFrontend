using Microsoft.AspNetCore.Mvc;

namespace RestaurangFrontend.Controllers
{
    public class AdminController : Controller
    {
        private const string Username = "admin";
        private const string Password = "admin123";

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string userName, string password)
        {
            if (userName == Username && password == Password)
            {
                HttpContext.Session.SetString("IsAdmin", "true");
                return RedirectToAction("Index", "Admin");
            }

            ViewBag.Error = "Wrong username or password";
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("IsAdmin");
            return RedirectToAction("Login");
        }
    }
}
