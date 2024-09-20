using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurangFrontend.Models;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;

namespace RestaurangFrontend.Controllers
{
    public class BookingController : Controller
    {
        private readonly HttpClient _httpClient;
        private string baseUrl = "https://localhost:7053/";

        public BookingController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create(int customerId)
        {
            ViewData["Title"] = "Booking";

            Booking createBooking = new Booking();
            createBooking.CustomerId = customerId;
            return View(createBooking);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(Booking create)
        {
            if (!ModelState.IsValid)
            {
                return View(create);
            }
            var json = JsonConvert.SerializeObject(create);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var customerRespons = await _httpClient.PostAsync($"{baseUrl}api/Booking/addBooking", content);

            return RedirectToAction("ThankYou");

        }
        public IActionResult ThankYou()
        {
            ViewData["Title"] = "Thank you!";
            return View();
        }

    }
}
