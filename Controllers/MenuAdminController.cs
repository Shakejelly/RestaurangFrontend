using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurangFrontend.Models;
using System.Text;

namespace RestaurangFrontend.Controllers
{
    public class MenuAdminController : Controller
    {
        private void SetTokenHeader()
        {
            var token = HttpContext.Session.GetString("JwtToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }
        private readonly HttpClient _httpClient;
        private readonly string baseUrl = "https://localhost:7053/";

        public MenuAdminController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync($"{baseUrl}api/Menu/getAllDishes");
            if (!response.IsSuccessStatusCode)
                return View(new List<Menu>());

            var json = await response.Content.ReadAsStringAsync();
            var dishes = JsonConvert.DeserializeObject<List<Menu>>(json);

            return View(dishes);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Menu dish)
        {
            var json = JsonConvert.SerializeObject(new
            {
                dish.DishName,
                dish.Description,
                DishPrice = dish.DishPrice
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            SetTokenHeader();

            var response = await _httpClient.PostAsync($"{baseUrl}api/Menu/addDish", content);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Något gick snett!");
                return View(dish);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            SetTokenHeader();

            var response = await _httpClient.GetAsync($"{baseUrl}api/Menu/getDish/{id}");
            if (!response.IsSuccessStatusCode)
                return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var dish = JsonConvert.DeserializeObject<Menu>(json);

            return View(dish);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Menu dish)
        {
            var json = JsonConvert.SerializeObject(new
            {
                DishId = dish.Id,
                dish.DishName,
                dish.Description,
                DishPrice = dish.DishPrice
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            SetTokenHeader();

            var response = await _httpClient.PostAsync($"{baseUrl}api/Menu/updateDish?dishId={dish.Id}", content);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Kunde inte uppdatera rätten.");
                return View(dish);
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            SetTokenHeader(); 

            var response = await _httpClient.DeleteAsync($"{baseUrl}api/Menu/deleteDish/{id}");

            if ((int)response.StatusCode >= 200 && (int)response.StatusCode <= 299)
            {
                TempData["Success"] = "Rätten togs bort! 🍝💨";
            }
            else
            {
                TempData["Error"] = "Kunde inte ta bort rätten 😢";
            }

            return RedirectToAction("Index");
        }
    }
}
