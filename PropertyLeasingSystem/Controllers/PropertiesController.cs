using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertyLeasingSystem.Data;

namespace PropertyLeasingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PropertiesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PropertiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/properties
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var properties = await _context.Properties
                .Include(p => p.Units)
                .ToListAsync();
            return Ok(properties);
        }

        // GET: api/properties/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var property = await _context.Properties
                .Include(p => p.Units)
                .FirstOrDefaultAsync(p => p.PropertyId == id);

            if (property == null)
                return NotFound("Property not found");

            return Ok(property);
        }

        // GET: api/properties/units/available
        [HttpGet("units/available")]
        public async Task<IActionResult> GetAvailableUnits()
        {
            var units = await _context.Units
                .Include(u => u.Property)
                .Where(u => u.IsAvailable == true)
                .ToListAsync();
            return Ok(units);
        }
    }
}