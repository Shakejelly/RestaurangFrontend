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
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync($"{baseUrl}api/Booking/getAllBookings");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Kunde inte hämta bokningar 😓";
                return View(new List<Booking>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var bookings = JsonConvert.DeserializeObject<List<Booking>>(json);

            return View(bookings);
        }
        public IActionResult Create(int customerId, string customerName)
        {
            ViewData["Title"] = "Booking";

            ViewBag.CustomerName = customerName;
            Booking createBooking = new Booking() { CustomerId = customerId };

            return View(createBooking);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(Booking create)
        {
            if (!ModelState.IsValid)
            {
                return View(create);
            }

            var availableTable = await GetAvailableTable(create.TableAmount, create.TimeToArrive);
            if (availableTable == null)
            {
                ModelState.AddModelError("", "No available table found");
                return View(create);
            }
            var json = JsonConvert.SerializeObject(create);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var customerRespons = await _httpClient.PostAsync($"{baseUrl}api/Booking/addBooking", content);

            return RedirectToAction("ThankYou");

        }
        private async Task<Table?> GetAvailableTable(int seatingsRequired, DateTime bookingTime)
        {
            var response = await _httpClient.GetAsync($"{baseUrl}Table/GetAvailableTable?seatingsRequired={seatingsRequired}&bookingTime={bookingTime}");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var availableTable = JsonConvert.DeserializeObject<Table>(jsonResponse);
                return availableTable;
            }

            return null; // Handle the error as needed
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"{baseUrl}api/Booking/booking/{id}");

            if (!response.IsSuccessStatusCode)
                return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var booking = JsonConvert.DeserializeObject<Booking>(json);

            return View(booking);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Booking booking)
        {
            if (!ModelState.IsValid)
                return View(booking);

            var json = JsonConvert.SerializeObject(booking);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{baseUrl}api/Booking/updateBooking/{booking.BookingId}", content);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Failed to update booking");
                return View(booking);
            }

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"{baseUrl}api/Booking/deleteBooking/{id}");

            if (!response.IsSuccessStatusCode)
                return BadRequest();

            return RedirectToAction("Index");
        }

        public IActionResult ThankYou()
        {
            ViewData["Title"] = "Thank you!";
            return View();
        }

    }
}
