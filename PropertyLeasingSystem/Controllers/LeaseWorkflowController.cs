using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertyLeasing.API.Data;
using PropertyLeasingSystem.Services;

namespace PropertyLeasingSystem.Controllers
{
    [Authorize(Roles = WorkflowRoles.PropertyManager)]
    public class LeaseWorkflowController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILeaseLifecycleService _leaseLifecycleService;

        public LeaseWorkflowController(
            ApplicationDbContext context,
            ILeaseLifecycleService leaseLifecycleService)
        {
            _context = context;
            _leaseLifecycleService = leaseLifecycleService;
        }

        public async Task<IActionResult> Index()
        {
            var applications = await _context.Applications
                .Include(a => a.Tenant)
                .Include(a => a.Unit)
                .ToListAsync();

            return View(applications);
        }

        [HttpPost]
        public async Task<IActionResult> MoveToScreening(int id)
        {
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Activate(int id, DateTime startDate, DateTime endDate)
        {
            return RedirectToAction(nameof(Index));
        }
    }
}
