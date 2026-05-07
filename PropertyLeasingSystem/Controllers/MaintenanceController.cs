using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertyLeasing.API.Data;
using PropertyLeasing.API.Services;

namespace PropertyLeasingSystem.Controllers
{
    [Authorize(Roles = WorkflowRoles.PropertyManager + "," + WorkflowRoles.MaintenanceStaff)]
    public class MaintenanceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MaintenanceController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Board()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetRequests()
        {
            var requests = await _context.MaintenanceRequests
                .Include(r => r.Tenant)
                .Include(r => r.Unit)
                .Select(r => new
                {
                    r.RequestId,
                    r.Description,
                    r.Priority,
                    r.Status,
                    r.ReportedAt,
                    TenantName = r.Tenant != null ? r.Tenant.FullName : "",
                    UnitNumber = r.Unit != null ? r.Unit.UnitNumber : ""
                })
                .ToListAsync();

            return Json(requests);
        }
    }
}
