using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertyLeasing.API.Data;
using PropertyLeasing.API.Models;
using PropertyLeasing.API.Services;
using System.Security.Claims;

namespace PropertyLeasingSystem.Controllers
{
    [Authorize(Roles = WorkflowRoles.PropertyManager)]
    public class LeaseWorkflowController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILeaseLifecycleService _leaseLifecycleService;
        private readonly UserManager<AppUser> _userManager;

        public LeaseWorkflowController(
            ApplicationDbContext context,
            ILeaseLifecycleService leaseLifecycleService,
            UserManager<AppUser> userManager)
        {
            _context = context;
            _leaseLifecycleService = leaseLifecycleService;
            _userManager = userManager;
        }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MoveToScreening(int id)
        {
            var application = await _context.Applications.FirstOrDefaultAsync(a => a.ApplicationId == id);
            if (application == null)
            {
                TempData["Error"] = "Application was not found.";
                return RedirectToAction(nameof(Index));
            }

            var userRole = User.FindFirstValue(ClaimTypes.Role) ?? string.Empty;
            var validation = _leaseLifecycleService.ValidateMoveToScreening(application, userRole);

            if (!validation.IsValid)
            {
                TempData["Error"] = validation.ErrorMessage;
                return RedirectToAction(nameof(Index));
            }

            application.Status = ApplicationStatuses.Screening;
            await _context.SaveChangesAsync();
            await NotifyTenant(application.TenantId, $"Your application #{application.ApplicationId} has moved to screening.");

            TempData["Success"] = "Application moved to screening.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var application = await _context.Applications.FirstOrDefaultAsync(a => a.ApplicationId == id);
            if (application == null)
            {
                TempData["Error"] = "Application was not found.";
                return RedirectToAction(nameof(Index));
            }

            var userRole = User.FindFirstValue(ClaimTypes.Role) ?? string.Empty;
            var validation = _leaseLifecycleService.ValidateApplicationApproval(application, userRole);

            if (!validation.IsValid)
            {
                TempData["Error"] = validation.ErrorMessage;
                return RedirectToAction(nameof(Index));
            }

            application.Status = ApplicationStatuses.Approved;
            application.ProcessedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            await NotifyTenant(application.TenantId, $"Your application #{application.ApplicationId} has been approved!");

            TempData["Success"] = "Application approved.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id)
        {
            var application = await _context.Applications.FirstOrDefaultAsync(a => a.ApplicationId == id);
            if (application == null)
            {
                TempData["Error"] = "Application was not found.";
                return RedirectToAction(nameof(Index));
            }

            var userRole = User.FindFirstValue(ClaimTypes.Role) ?? string.Empty;
            var validation = _leaseLifecycleService.ValidateApplicationRejection(application, userRole);

            if (!validation.IsValid)
            {
                TempData["Error"] = validation.ErrorMessage;
                return RedirectToAction(nameof(Index));
            }

            application.Status = ApplicationStatuses.Rejected;
            application.ProcessedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            await NotifyTenant(application.TenantId, $"Your application #{application.ApplicationId} has been rejected.");

            TempData["Success"] = "Application rejected.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activate(int id, DateTime startDate, DateTime endDate)
        {
            var application = await _context.Applications
                .Include(a => a.Unit)
                .FirstOrDefaultAsync(a => a.ApplicationId == id);

            if (application == null)
            {
                TempData["Error"] = "Application was not found.";
                return RedirectToAction(nameof(Index));
            }

            if (application.Unit == null)
            {
                TempData["Error"] = "Unit was not found.";
                return RedirectToAction(nameof(Index));
            }

            var userRole = User.FindFirstValue(ClaimTypes.Role) ?? string.Empty;
            var validation = _leaseLifecycleService.ValidateLeaseActivation(
                application,
                application.Unit,
                userRole,
                startDate,
                endDate);

            if (!validation.IsValid)
            {
                TempData["Error"] = validation.ErrorMessage;
                return RedirectToAction(nameof(Index));
            }

            var lease = new Lease
            {
                ApplicationId = application.ApplicationId,
                UnitId = application.UnitId,
                TenantId = application.TenantId,
                StartDate = startDate,
                EndDate = endDate,
                MonthlyRent = application.Unit.RentAmount,
                Status = ApplicationStatuses.LeaseActive,
                CreatedAt = DateTime.UtcNow
            };

            _context.Leases.Add(lease);
            application.Status = ApplicationStatuses.LeaseActive;
            application.ProcessedAt = DateTime.UtcNow;
            application.Unit.IsAvailable = false;

            await _context.SaveChangesAsync();
            await NotifyTenant(application.TenantId, $"Your lease for unit {application.Unit.UnitNumber} has been activated.");

            TempData["Success"] = "Lease activated successfully.";
            return RedirectToAction(nameof(Index));
        }

        private async Task NotifyTenant(int tenantId, string message)
        {
            var tenant = await _context.Tenants.FindAsync(tenantId);
            if (tenant == null) return;

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.TenantId == tenantId);
            if (user == null) return;

            _context.Notifications.Add(new Notification
            {
                UserId = user.Id,
                Message = message,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();
        }
    }
}
