using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurangFrontend.Models;
using System.Net;
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
            // URL encode the phone number to avoid special character issues
            var encodedPhoneNumber = WebUtility.UrlEncode(create.PhoneNumber);
            var checkCustomerResponse = await _httpClient.GetAsync($"{baseUri}api/Customer/GetByPhoneNumber?phoneNumber={encodedPhoneNumber}");

            // If the customer is not found (404), proceed to add the customer
            if (checkCustomerResponse.StatusCode == HttpStatusCode.NotFound)
            {
                // Proceed to add the customer
                var json = JsonConvert.SerializeObject(create);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var customerResponse = await _httpClient.PostAsync($"{baseUri}api/Customer/AddCustomer", content);

                if (customerResponse.IsSuccessStatusCode)
                {
                    var newCustomer = JsonConvert.DeserializeObject<Customer>(await customerResponse.Content.ReadAsStringAsync());
                    return RedirectToAction("Create", "Booking", new { customerId = newCustomer.CustomerId, customerName = newCustomer.CustomerName });
                }
                ModelState.AddModelError(string.Empty, "Unable to create customer. Please try again.");
                return View(create);
            }
            else if (checkCustomerResponse.IsSuccessStatusCode)
            {
                // If the customer was found, deserialize the response and redirect
                var existingCustomer = JsonConvert.DeserializeObject<Customer>(await checkCustomerResponse.Content.ReadAsStringAsync());

                if (existingCustomer != null)
                {
                    return RedirectToAction("Create", "Booking", new { customerId = existingCustomer.CustomerId, customerName = existingCustomer.CustomerName });
                }
            }

            ModelState.AddModelError(string.Empty, "An unexpected error occurred.");
            return View(create);
        }

    }
}
