using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertyLeasing.API.Data;
using PropertyLeasing.API.Models;
using PropertyLeasing.API.Services;

namespace PropertyLeasingSystem.Controllers
{
    [Authorize(Roles = WorkflowRoles.PropertyManager)]
    public class PropertiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PropertiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var properties = await _context.Properties
                .Include(p => p.Units)
                .ToListAsync();
            return View(properties);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Property property)
        {
            if (!ModelState.IsValid)
                return View(property);

            _context.Properties.Add(property);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Property created successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var property = await _context.Properties.FindAsync(id);
            if (property == null) return NotFound();
            return View(property);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Property property)
        {
            if (id != property.PropertyId) return BadRequest();
            if (!ModelState.IsValid) return View(property);

            _context.Update(property);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Property updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var property = await _context.Properties
                .Include(p => p.Units)
                .FirstOrDefaultAsync(p => p.PropertyId == id);
            if (property == null) return NotFound();
            return View(property);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var property = await _context.Properties.FindAsync(id);
            if (property != null)
            {
                _context.Properties.Remove(property);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Property deleted successfully.";
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Units(int propertyId)
        {
            var property = await _context.Properties
                .Include(p => p.Units)
                .FirstOrDefaultAsync(p => p.PropertyId == propertyId);
            if (property == null) return NotFound();
            ViewBag.Property = property;
            return View(property.Units ?? new List<Unit>());
        }

        public async Task<IActionResult> CreateUnit(int propertyId)
        {
            var property = await _context.Properties.FindAsync(propertyId);
            if (property == null) return NotFound();
            ViewBag.PropertyId = propertyId;
            ViewBag.PropertyName = property.PropertyName;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUnit(Unit unit)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.PropertyId = unit.PropertyId;
                return View(unit);
            }

            _context.Units.Add(unit);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Unit created successfully.";
            return RedirectToAction(nameof(Units), new { propertyId = unit.PropertyId });
        }

        public async Task<IActionResult> EditUnit(int id)
        {
            var unit = await _context.Units
                .Include(u => u.Property)
                .FirstOrDefaultAsync(u => u.UnitId == id);
            if (unit == null) return NotFound();
            return View(unit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUnit(int id, Unit unit)
        {
            if (id != unit.UnitId) return BadRequest();
            if (!ModelState.IsValid) return View(unit);

            _context.Update(unit);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Unit updated successfully.";
            return RedirectToAction(nameof(Units), new { propertyId = unit.PropertyId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUnit(int id)
        {
            var unit = await _context.Units.FindAsync(id);
            if (unit != null)
            {
                int propertyId = unit.PropertyId;
                _context.Units.Remove(unit);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Unit deleted successfully.";
                return RedirectToAction(nameof(Units), new { propertyId });
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
