using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertyLeasing.API.Data;

namespace PropertyLeasing.API.Controllers
{
    [ApiController]
    [Route("api/leases")]
    [Authorize(Roles = "Property Manager")]
    public class LeasesApiController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public LeasesApiController(ApplicationDbContext db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var leases = await _db.Leases
                .Include(l => l.Tenant)
                .Include(l => l.Unit)
                .Select(l => new
                {
                    l.LeaseId, l.Status, l.StartDate, l.EndDate, l.MonthlyRent,
                    TenantName = l.Tenant.FullName,
                    UnitNumber = l.Unit.UnitNumber
                }).ToListAsync();
            return Ok(leases);
        }

        [HttpGet("report/occupancy")]
        public async Task<IActionResult> OccupancyReport()
        {
            var total = await _db.Units.CountAsync();
            var occupied = await _db.Units.CountAsync(u => !u.IsAvailable);
            return Ok(new
            {
                TotalUnits = total,
                OccupiedUnits = occupied,
                AvailableUnits = total - occupied,
                OccupancyRate = total == 0 ? 0 : Math.Round((double)occupied / total * 100, 1)
            });
        }

        [HttpGet("report/overdue-payments")]
        public async Task<IActionResult> OverduePayments()
        {
            var overdue = await _db.Payments
                .Where(p => p.Status == "Overdue")
                .Include(p => p.Lease).ThenInclude(l => l.Tenant)
                .Select(p => new
                {
                    p.PaymentId, p.Amount, p.PaymentDate,
                    TenantName = p.Lease.Tenant.FullName
                }).ToListAsync();
            return Ok(overdue);
        }
    }
}
