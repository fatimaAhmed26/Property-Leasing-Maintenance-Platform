using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertyLeasingSystem.Data;

namespace PropertyLeasingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaintenanceController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MaintenanceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // PUBLIC - no login needed
        // GET: api/maintenance/lookup?ticketNumber=1&phoneNumber=12345678
        [AllowAnonymous]
        [HttpGet("lookup")]
        public async Task<IActionResult> PublicLookup(
            string ticketNumber, string phoneNumber)
        {
            if (!int.TryParse(ticketNumber, out int requestId))
                return BadRequest("Invalid ticket number");

            // Get the request first
            var request = await _context.MaintenanceRequests
                .FirstOrDefaultAsync(m => m.RequestId == requestId);

            if (request == null)
                return NotFound("No request found");

            // Check phone number via Tenant separately
            var tenant = await _context.Tenants
                .FirstOrDefaultAsync(t => t.TenantId == request.TenantId
                                       && t.Phone == phoneNumber);

            if (tenant == null)
                return NotFound("No request found with those details");

            return Ok(new
            {
                ticketNumber = request.RequestId,
                status = request.Status,
                description = request.Description,
                priority = request.Priority,
                reportedAt = request.ReportedAt,
                tenantName = tenant.FullName
            });
        }

        // GET: api/maintenance
        [Authorize(Roles = "PropertyManager,MaintenanceStaff")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var requests = await _context.MaintenanceRequests.ToListAsync();
            return Ok(requests);
        }

        // GET: api/maintenance/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var request = await _context.MaintenanceRequests
                .FirstOrDefaultAsync(m => m.RequestId == id);

            if (request == null)
                return NotFound();

            return Ok(request);
        }
    }
}