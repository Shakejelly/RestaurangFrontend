using Microsoft.AspNetCore.Mvc;
using RestaurangFrontend.Models;

namespace RestaurangFrontend.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Logins()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginUser)
        {
             return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            return RedirectToAction("Logins", "Account");
        }
    }
}
