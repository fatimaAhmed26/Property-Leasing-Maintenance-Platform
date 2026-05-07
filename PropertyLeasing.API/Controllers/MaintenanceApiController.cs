using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PropertyLeasing.API.Data;
using PropertyLeasing.API.DTOs;
using PropertyLeasing.API.Hubs;
using PropertyLeasing.API.Models;

namespace PropertyLeasing.API.Controllers
{
    [ApiController]
    [Route("api/maintenance")]
    public class MaintenanceApiController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IHubContext<MaintenanceHub> _hub;

        public MaintenanceApiController(ApplicationDbContext db, IHubContext<MaintenanceHub> hub)
        {
            _db = db;
            _hub = hub;
        }

        // Public lookup — no auth required (used by MVC via HttpClient)
        [HttpGet("lookup")]
        public async Task<IActionResult> Lookup([FromQuery] int ticketId, [FromQuery] string phone)
        {
            var request = await _db.MaintenanceRequests
                .Include(r => r.Tenant)
                .Include(r => r.MaintenanceLogs)
                .FirstOrDefaultAsync(r => r.RequestId == ticketId && r.Tenant.Phone == phone);

            if (request == null)
                return NotFound(new { message = "No maintenance request found with the given ticket number and phone number." });

            return Ok(new MaintenanceLookupResultDto
            {
                Status = request.Status,
                Description = request.Description,
                Priority = request.Priority,
                ReportedAt = request.ReportedAt,
                TenantName = request.Tenant.FullName,
                Logs = request.MaintenanceLogs.Select(l => new MaintenanceLogDto
                {
                    ActionTaken = l.ActionTaken,
                    WorkStarted = l.WorkStarted,
                    WorkCompleted = l.WorkCompleted
                }).ToList()
            });
        }

        // Submit a maintenance request (Tenant)
        [HttpPost("submit")]
        [Authorize(Roles = "Tenant")]
        public async Task<IActionResult> Submit([FromBody] SubmitMaintenanceRequestDto dto)
        {
            var request = new MaintenanceRequest
            {
                UnitId = dto.UnitId,
                TenantId = dto.TenantId,
                Description = dto.Description,
                Priority = dto.Priority,
                ReportedAt = DateTime.Now,
                Status = "Submitted"
            };
            _db.MaintenanceRequests.Add(request);
            await _db.SaveChangesAsync();

            await _hub.Clients.Group("MaintenanceBoard")
                .SendAsync("RequestSubmitted", new { request.RequestId, request.Description, request.Priority, request.Status });

            return CreatedAtAction(nameof(Lookup), new { ticketId = request.RequestId }, new { request.RequestId });
        }

        // Get all maintenance requests (Property Manager / Maintenance Staff)
        [HttpGet]
        [Authorize(Roles = "Property Manager,Maintenance Staff")]
        public async Task<IActionResult> GetAll()
        {
            var requests = await _db.MaintenanceRequests
                .Include(r => r.Tenant)
                .Include(r => r.Unit)
                .Select(r => new
                {
                    r.RequestId, r.Description, r.Priority, r.Status, r.ReportedAt,
                    TenantName = r.Tenant.FullName,
                    UnitNumber = r.Unit.UnitNumber
                }).ToListAsync();
            return Ok(requests);
        }

        // Update status (Property Manager / Maintenance Staff)
        [HttpPatch("{id}/status")]
        [Authorize(Roles = "Property Manager,Maintenance Staff")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusDto dto)
        {
            var request = await _db.MaintenanceRequests.FindAsync(id);
            if (request == null) return NotFound();

            var old = request.Status;
            request.Status = dto.Status;

            _db.StatusHistories.Add(new StatusHistory
            {
                EntityName = "MaintenanceRequest",
                EntityId = id,
                OldStatus = old,
                NewStatus = dto.Status,
                ChangedAt = DateTime.Now
            });
            await _db.SaveChangesAsync();

            await _hub.Clients.Group("MaintenanceBoard")
                .SendAsync("StatusUpdated", new { id, newStatus = dto.Status });

            return NoContent();
        }

        // Backlog report (Property Manager — for reporting app)
        [HttpGet("report/backlog")]
        [Authorize(Roles = "Property Manager")]
        public async Task<IActionResult> BacklogReport()
        {
            var data = await _db.MaintenanceRequests
                .GroupBy(r => r.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync();
            return Ok(data);
        }
    }

    public class UpdateStatusDto
    {
        public string Status { get; set; } = string.Empty;
    }
}
