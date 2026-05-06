using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertyLeasing.API.Data;
using PropertyLeasingSystem.Services;
using System.Security.Claims;

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

            TempData["Success"] = "Application moved to screening.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
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

            TempData["Success"] = "Application approved.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
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

            TempData["Success"] = "Application rejected.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
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

            var lease = new PropertyLeasing.API.Models.Lease
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

            TempData["Success"] = "Lease activated successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
