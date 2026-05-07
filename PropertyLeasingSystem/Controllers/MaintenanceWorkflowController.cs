using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PropertyLeasing.API.Data;
using PropertyLeasing.API.Models;
using PropertyLeasing.API.DTOs;
using PropertyLeasing.API.Services;
using PropertyLeasingSystem.Hubs;
using System.Security.Claims;

namespace PropertyLeasingSystem.Controllers
{
    [Authorize]
    public class MaintenanceWorkflowController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMaintenanceLifecycleService _maintenanceLifecycleService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHubContext<MaintenanceHub> _hub;

        public MaintenanceWorkflowController(
            ApplicationDbContext context,
            IMaintenanceLifecycleService maintenanceLifecycleService,
            UserManager<AppUser> userManager,
            IHubContext<MaintenanceHub> hub)
        {
            _context = context;
            _maintenanceLifecycleService = maintenanceLifecycleService;
            _userManager = userManager;
            _hub = hub;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var appUser = await _context.Users.FindAsync(userId);

            IQueryable<MaintenanceRequest> query = _context.MaintenanceRequests
                .Include(r => r.Tenant)
                .Include(r => r.Unit)
                    .ThenInclude(u => u!.Property);

            if (User.IsInRole(WorkflowRoles.Tenant) && appUser?.TenantId != null)
                query = query.Where(r => r.TenantId == appUser.TenantId);
            else if (User.IsInRole(WorkflowRoles.MaintenanceStaff) && appUser?.StaffId != null)
                query = query.Where(r => r.AssignedStaffId == appUser.StaffId);

            var requests = await query.OrderByDescending(r => r.ReportedAt).ToListAsync();

            if (User.IsInRole(WorkflowRoles.PropertyManager))
                ViewBag.StaffList = await _context.Staffs.Where(s => s.IsAvailable).ToListAsync();

            return View(requests);
        }

        [HttpGet]
        [Authorize(Roles = WorkflowRoles.Tenant)]
        public async Task<IActionResult> Submit()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var appUser = await _context.Users.FindAsync(userId);

            if (appUser?.TenantId != null)
            {
                var leases = await _context.Leases
                    .Include(l => l.Unit)
                    .Where(l => l.TenantId == appUser.TenantId &&
                           (l.Status == ApplicationStatuses.LeaseActive || l.Status == ApplicationStatuses.Renewed))
                    .ToListAsync();
                ViewBag.TenantUnits = leases.Select(l => l.Unit).ToList();
            }
            else
            {
                ViewBag.TenantUnits = new List<Unit>();
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = WorkflowRoles.Tenant)]
        public async Task<IActionResult> Submit(SubmitMaintenanceRequestDto dto)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fill in all required fields.";
                return View(dto);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var appUser = await _context.Users.FindAsync(userId);

            if (appUser?.TenantId == null)
            {
                TempData["Error"] = "No tenant profile found. Please contact the property manager.";
                return View(dto);
            }

            var unit = await _context.Units.FirstOrDefaultAsync(u => u.UnitId == dto.UnitId);
            if (unit == null)
            {
                TempData["Error"] = "Unit was not found.";
                return View(dto);
            }

            var request = new MaintenanceRequest
            {
                UnitId = dto.UnitId,
                TenantId = appUser.TenantId.Value,
                Description = dto.Description,
                Priority = dto.Priority,
                Status = MaintenanceStatuses.Submitted,
                ReportedAt = DateTime.UtcNow
            };

            _context.MaintenanceRequests.Add(request);
            await _context.SaveChangesAsync();
            await _hub.Clients.Group("MaintenanceBoard").SendAsync("NewMaintenanceRequest");

            TempData["Success"] = $"Maintenance request submitted. Your ticket number is {request.RequestId}.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = WorkflowRoles.PropertyManager)]
        public async Task<IActionResult> Assign(int id, int staffId)
        {
            var request = await _context.MaintenanceRequests.FirstOrDefaultAsync(r => r.RequestId == id);
            if (request == null)
            {
                TempData["Error"] = "Maintenance request was not found.";
                return RedirectToAction(nameof(Index));
            }

            var userRole = User.FindFirstValue(ClaimTypes.Role) ?? string.Empty;
            var validation = _maintenanceLifecycleService.ValidateMaintenanceTransition(
                request,
                MaintenanceStatuses.Assigned,
                userRole);

            if (!validation.IsValid)
            {
                TempData["Error"] = validation.ErrorMessage;
                return RedirectToAction(nameof(Index));
            }

            request.Status = MaintenanceStatuses.Assigned;
            request.AssignedStaffId = staffId > 0 ? staffId : null;
            await _context.SaveChangesAsync();
            await _hub.Clients.Group("MaintenanceBoard").SendAsync("MaintenanceStatusUpdated", request.RequestId, MaintenanceStatuses.Assigned);
            await NotifyTenant(request.TenantId, $"Your maintenance request #{request.RequestId} has been assigned to staff.");

            TempData["Success"] = "Request assigned successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = WorkflowRoles.MaintenanceStaff)]
        public async Task<IActionResult> UpdateStatus(int id, string nextStatus)
        {
            if (string.IsNullOrWhiteSpace(nextStatus))
            {
                TempData["Error"] = "Next status is required.";
                return RedirectToAction(nameof(Index));
            }

            var request = await _context.MaintenanceRequests.FirstOrDefaultAsync(r => r.RequestId == id);
            if (request == null)
            {
                TempData["Error"] = "Maintenance request was not found.";
                return RedirectToAction(nameof(Index));
            }

            var userRole = User.FindFirstValue(ClaimTypes.Role) ?? string.Empty;
            var validation = _maintenanceLifecycleService.ValidateMaintenanceTransition(
                request,
                nextStatus,
                userRole);

            if (!validation.IsValid)
            {
                TempData["Error"] = validation.ErrorMessage;
                return RedirectToAction(nameof(Index));
            }

            request.Status = nextStatus;
            await _context.SaveChangesAsync();
            await _hub.Clients.Group("MaintenanceBoard").SendAsync("MaintenanceStatusUpdated", request.RequestId, nextStatus);
            await NotifyTenant(request.TenantId, $"Your maintenance request #{request.RequestId} status updated to: {nextStatus}.");

            TempData["Success"] = $"Request status updated to {nextStatus}.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = WorkflowRoles.PropertyManager + "," + WorkflowRoles.Tenant)]
        public async Task<IActionResult> Close(int id)
        {
            var request = await _context.MaintenanceRequests.FirstOrDefaultAsync(r => r.RequestId == id);
            if (request == null)
            {
                TempData["Error"] = "Maintenance request was not found.";
                return RedirectToAction(nameof(Index));
            }

            var userRole = User.FindFirstValue(ClaimTypes.Role) ?? string.Empty;
            var validation = _maintenanceLifecycleService.ValidateMaintenanceTransition(
                request,
                MaintenanceStatuses.Closed,
                userRole);

            if (!validation.IsValid)
            {
                TempData["Error"] = validation.ErrorMessage;
                return RedirectToAction(nameof(Index));
            }

            request.Status = MaintenanceStatuses.Closed;
            await _context.SaveChangesAsync();
            await NotifyTenant(request.TenantId, $"Your maintenance request #{request.RequestId} has been closed.");

            TempData["Success"] = "Request closed successfully.";
            return RedirectToAction(nameof(Index));
        }

        private async Task NotifyTenant(int tenantId, string message)
        {
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
