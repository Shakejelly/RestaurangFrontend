using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurangFrontend.Models;

namespace RestaurangFrontend.Controllers
{
    public class MenusController : Controller
    {
        private readonly HttpClient _httpClient;
        private string baseUrl = "https://localhost:7053/";

        public MenusController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "The Menu";

            var respons = await _httpClient.GetAsync($"{baseUrl}api/Menu/getAllDishes");

            var json = await respons.Content.ReadAsStringAsync();

            var menuList = JsonConvert.DeserializeObject<List<Menu>>(json);

            return View(menuList);
        }
        
    }
}
