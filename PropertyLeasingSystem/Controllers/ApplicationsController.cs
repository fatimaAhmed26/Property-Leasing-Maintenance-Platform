using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertyLeasing.API.Data;
using PropertyLeasing.API.Services;
using System.Security.Claims;

using LeaseApplication = PropertyLeasing.API.Models.Application;

namespace PropertyLeasingSystem.Controllers
{
    [Authorize]
    public class ApplicationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApplicationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = WorkflowRoles.PropertyManager)]
        public async Task<IActionResult> Index()
        {
            var applications = await _context.Applications
                .Include(a => a.Tenant)
                .Include(a => a.Unit)
                    .ThenInclude(u => u!.Property)
                .OrderByDescending(a => a.SubmittedAt)
                .ToListAsync();
            return View(applications);
        }

        [HttpGet]
        [Authorize(Roles = WorkflowRoles.Tenant)]
        public async Task<IActionResult> Submit()
        {
            var availableUnits = await _context.Units
                .Include(u => u.Property)
                .Where(u => u.IsAvailable)
                .ToListAsync();
            ViewBag.AvailableUnits = availableUnits;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = WorkflowRoles.Tenant)]
        public async Task<IActionResult> Submit(int unitId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Find or create tenant linked to the current user
            var appUser = await _context.Users.FindAsync(userId);
            if (appUser == null)
            {
                TempData["Error"] = "User not found.";
                return RedirectToAction(nameof(Submit));
            }

            int tenantId;
            if (appUser.TenantId.HasValue)
            {
                tenantId = appUser.TenantId.Value;
            }
            else
            {
                // Auto-create a tenant record for this user
                var tenant = new PropertyLeasing.API.Models.Tenant
                {
                    FullName = appUser.FullName,
                    Email = appUser.Email ?? string.Empty,
                    Phone = appUser.PhoneNumber ?? string.Empty
                };
                _context.Tenants.Add(tenant);
                await _context.SaveChangesAsync();

                appUser.TenantId = tenant.TenantId;
                await _context.SaveChangesAsync();
                tenantId = tenant.TenantId;
            }

            var unit = await _context.Units.FindAsync(unitId);
            if (unit == null || !unit.IsAvailable)
            {
                TempData["Error"] = "Selected unit is not available.";
                return RedirectToAction(nameof(Submit));
            }

            var application = new LeaseApplication
            {
                TenantId = tenantId,
                UnitId = unitId,
                SubmittedAt = DateTime.UtcNow,
                Status = ApplicationStatuses.Submitted
            };

            _context.Applications.Add(application);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Application submitted successfully. Application ID: {application.ApplicationId}";
            return RedirectToAction("Index", "Home");
        }

    }
}
