using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertyLeasing.API.Data;
using PropertyLeasingSystem.Services;

namespace PropertyLeasingSystem.Controllers
{
    [Authorize]
    public class MaintenanceWorkflowController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMaintenanceLifecycleService _maintenanceLifecycleService;

        public MaintenanceWorkflowController(
            ApplicationDbContext context,
            IMaintenanceLifecycleService maintenanceLifecycleService)
        {
            _context = context;
            _maintenanceLifecycleService = maintenanceLifecycleService;
        }

        public async Task<IActionResult> Index()
        {
            var requests = await _context.MaintenanceRequests
                .Include(r => r.Tenant)
                .Include(r => r.Unit)
                .ToListAsync();

            return View(requests);
        }

        [HttpPost]
        [Authorize(Roles = WorkflowRoles.PropertyManager)]
        public async Task<IActionResult> Assign(int id, int staffId)
        {
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = WorkflowRoles.MaintenanceStaff)]
        public async Task<IActionResult> UpdateStatus(int id, string nextStatus)
        {
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = WorkflowRoles.PropertyManager + "," + WorkflowRoles.Tenant)]
        public async Task<IActionResult> Close(int id)
        {
            return RedirectToAction(nameof(Index));
        }
    }
}
