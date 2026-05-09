using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertyLeasing.API.Data;
using PropertyLeasing.API.DTOs;

namespace PropertyLeasingSystem.Controllers
{
    [AllowAnonymous]
    public class PublicLookupController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PublicLookupController(ApplicationDbContext context)
        {
            _context = context;
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

            if (!int.TryParse(ticketNumber, out int ticketId))
            {
                TempData["Error"] = "Ticket number must be a valid number.";
                return View();
            }

            var request = await _context.MaintenanceRequests
                .Include(r => r.Tenant)
                .Include(r => r.MaintenanceLogs)
                .FirstOrDefaultAsync(r => r.RequestId == ticketId && r.Tenant!.Phone == phoneNumber.Trim());

            if (request == null)
            {
                TempData["Error"] = "No maintenance request found with those details.";
                return View();
            }

            var result = new MaintenanceLookupResultDto
            {
                Status      = request.Status,
                Description = request.Description,
                Priority    = request.Priority,
                ReportedAt  = request.ReportedAt,
                TenantName  = request.Tenant!.FullName,
                Logs        = request.MaintenanceLogs?.Select(l => new MaintenanceLogDto
                {
                    ActionTaken   = l.ActionTaken,
                    WorkStarted   = l.WorkStarted,
                    WorkCompleted = l.WorkCompleted
                }).ToList() ?? []
            };

            return View("MaintenanceLookupResult", result);
        }
    }
}
