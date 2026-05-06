using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertyLeasing.API.Data;
using PropertyLeasing.API.Models;
using PropertyLeasingSystem.DTOs;
using PropertyLeasingSystem.Services;
using System.Security.Claims;

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

        [HttpGet]
        [Authorize(Roles = WorkflowRoles.Tenant)]
        public IActionResult Submit()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = WorkflowRoles.Tenant)]
        public async Task<IActionResult> Submit(SubmitMaintenanceRequestDto dto)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fill in all required fields.";
                return View(dto);
            }

            var tenantIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(tenantIdClaim, out int tenantId))
            {
                TempData["Error"] = "Unable to identify tenant. Please log in again.";
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
                TenantId = tenantId,
                Description = dto.Description,
                Priority = dto.Priority,
                Status = MaintenanceStatuses.Submitted,
                ReportedAt = DateTime.UtcNow
            };

            _context.MaintenanceRequests.Add(request);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Maintenance request submitted. Your ticket number is {request.RequestId}.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
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
            await _context.SaveChangesAsync();

            TempData["Success"] = "Request assigned successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
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

            TempData["Success"] = $"Request status updated to {nextStatus}.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
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

            TempData["Success"] = "Request closed successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
