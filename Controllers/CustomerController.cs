using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurangFrontend.Models;
using System.Text;
using System.Text.Json.Serialization;

namespace RestaurangFrontend.Controllers
{
    public class CustomerController : Controller
    {
        private readonly HttpClient _httpClient;
        private string baseUri = "https://localhost:7053/";

        public CustomerController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Create()
        {
            ViewData["Title"] = "Booking";
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(Customer create) 
        {
            var checkCustomerResponse = await _httpClient.GetAsync($"{baseUri}api/Customer/GetByPhoneNumber?phoneNumber={create.PhoneNumber}");

            if (checkCustomerResponse.IsSuccessStatusCode)
            {
                var existingCustomer = JsonConvert.DeserializeObject<Customer>(await checkCustomerResponse.Content.ReadAsStringAsync());

                if (existingCustomer != null)
                {
                    return RedirectToAction("Create", "Booking", new { customerId = existingCustomer.CustomerId });
                }
            }
        
            var json = JsonConvert.SerializeObject(create);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var customerResponse = await  _httpClient.PostAsync($"{baseUri}api/Customer/AddCustomer",content);

            if (customerResponse.IsSuccessStatusCode)
            {
                var newCustomer = JsonConvert.DeserializeObject<Customer>(await customerResponse.Content.ReadAsStringAsync());

                return RedirectToAction("Create", "Booking", new { customerId = newCustomer.CustomerId });
            }
            ModelState.AddModelError(string.Empty, "Unable to create customer. Please try again.");
            return View(create);
        }

    }
}
