using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertyLeasing.API.Data;
using PropertyLeasing.API.Services;

namespace PropertyLeasingSystem.Controllers
{
    [Authorize(Roles = WorkflowRoles.PropertyManager)]
    public class TenantsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TenantsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var tenants = await _context.Tenants.ToListAsync();
            return View(tenants);
        }

        public async Task<IActionResult> Details(int id)
        {
            var tenant = await _context.Tenants
                .Include(t => t.Applications!)
                    .ThenInclude(a => a.Unit)
                .Include(t => t.Leases!)
                    .ThenInclude(l => l.Unit)
                .Include(t => t.MaintenanceRequests!)
                    .ThenInclude(m => m.Unit)
                .FirstOrDefaultAsync(t => t.TenantId == id);

            if (tenant == null) return NotFound();
            return View(tenant);
        }
    }
}
