using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.AspNetCore.Http;

namespace RestaurangFrontend.Controllers
{
    public class AdminController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public AdminController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var client = _clientFactory.CreateClient();
            var body = new
            {
                Email = email,
                Password = password
            };

            var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:7053/api/User/Login", content);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Fel inloggning 🍝";
                return View();
            }

            var json = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(json);

            string token = result.token;

            HttpContext.Session.SetString("JwtToken", token);
            HttpContext.Session.SetString("IsAdmin", "true");

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("JwtToken");
            HttpContext.Session.Remove("IsAdmin");
            return RedirectToAction("Login");
        }
    }
}
