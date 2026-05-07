using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertyLeasing.API.Data;

namespace PropertyLeasing.API.Controllers
{
    [ApiController]
    [Route("api/properties")]
    [Authorize(Roles = "Property Manager")]
    public class PropertiesApiController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public PropertiesApiController(ApplicationDbContext db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var props = await _db.Properties
                .Include(p => p.Units)
                .Select(p => new
                {
                    p.PropertyId, p.PropertyName, p.Address, p.City, p.PropertyType,
                    TotalUnits = p.Units.Count,
                    AvailableUnits = p.Units.Count(u => u.IsAvailable)
                }).ToListAsync();
            return Ok(props);
        }
    }
}
