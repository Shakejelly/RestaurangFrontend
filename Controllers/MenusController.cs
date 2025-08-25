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
            ViewData["Title"] = "Menu";

            try
            {
                var respons = await _httpClient.GetAsync($"{baseUrl}api/Menu/getAllDishes");
                if (!respons.IsSuccessStatusCode)
                {
                    ViewBag.Error = "Kunde inte hämta menyn just nu.";
                    return View(Enumerable.Empty<Menu>());
                }

                var json = await respons.Content.ReadAsStringAsync();
                var menuList = JsonConvert.DeserializeObject<List<Menu>>(json) ?? new List<Menu>();
                return View(menuList);
            }
            catch
            {
                ViewBag.Error = "Något gick snett när menyn skulle hämtas.";
                return View(Enumerable.Empty<Menu>());
            }
        }


    }
}
