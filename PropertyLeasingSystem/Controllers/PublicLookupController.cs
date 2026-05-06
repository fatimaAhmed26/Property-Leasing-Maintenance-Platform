using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyLeasingSystem.DTOs;
using System.Text.Json;

namespace PropertyLeasingSystem.Controllers
{
    [AllowAnonymous]
    public class PublicLookupController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PublicLookupController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult MaintenanceLookup()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> MaintenanceLookup(string ticketNumber, string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(ticketNumber) || string.IsNullOrWhiteSpace(phoneNumber))
            {
                TempData["Error"] = "Please enter both ticket number and phone number.";
                return View();
            }

            var client = _httpClientFactory.CreateClient("MaintenanceAPI");

            var response = await client.GetAsync(
                $"api/maintenance/lookup?ticketNumber={ticketNumber}&phoneNumber={phoneNumber}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound ||
                response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                TempData["Error"] = "No maintenance request found with those details.";
                return View();
            }

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Unable to retrieve request details. Please try again later.";
                return View();
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<MaintenanceLookupResultDto>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View("MaintenanceLookupResult", result);
        }
    }
}
