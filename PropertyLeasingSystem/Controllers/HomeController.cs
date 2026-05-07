using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertyLeasing.API.Data;
using PropertyLeasing.API.Models;
using PropertyLeasing.API.Services;

namespace PropertyLeasingSystem.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.AvailableUnits = await _context.Units.CountAsync(u => u.IsAvailable);
            ViewBag.ActiveLeases = await _context.Leases
                .CountAsync(l => l.Status == ApplicationStatuses.LeaseActive || l.Status == ApplicationStatuses.Renewed);
            ViewBag.OpenMaintenance = await _context.MaintenanceRequests
                .CountAsync(r => r.Status != MaintenanceStatuses.Closed);
            ViewBag.TotalProperties = await _context.Properties.CountAsync();
            ViewBag.PendingApplications = await _context.Applications
                .CountAsync(a => a.Status == ApplicationStatuses.Submitted ||
                                 a.Status == ApplicationStatuses.Pending ||
                                 a.Status == ApplicationStatuses.Screening);
            return View();
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
