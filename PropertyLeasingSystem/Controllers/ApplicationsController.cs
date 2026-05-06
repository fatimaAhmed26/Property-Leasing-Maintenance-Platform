using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertyLeasingSystem.Data;
using PropertyLeasingSystem.DTOs;
using PropertyLeasingSystem.Services;
using System.Security.Claims;

using LeaseApplication = PropertyLeasingSystem.Models.Application;

namespace PropertyLeasingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = WorkflowRoles.PropertyManager)]
    public class ApplicationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILeaseLifecycleService _leaseLifecycleService;

        public ApplicationsController(
            ApplicationDbContext context,
            ILeaseLifecycleService leaseLifecycleService)
        {
            _context = context;
            _leaseLifecycleService = leaseLifecycleService;
        }

        [HttpPost("{id}/screening")]
        public async Task<IActionResult> MoveToScreening(int id)
        {
            return await ChangeApplicationStatus(
                id,
                ApplicationStatuses.Screening,
                _leaseLifecycleService.ValidateMoveToScreening);
        }

        [HttpPost("{id}/approve")]
        public async Task<IActionResult> Approve(int id)
        {
            return await ChangeApplicationStatus(
                id,
                ApplicationStatuses.Approved,
                _leaseLifecycleService.ValidateApplicationApproval);
        }

        [HttpPost("{id}/reject")]
        public async Task<IActionResult> Reject(int id)
        {
            return await ChangeApplicationStatus(
                id,
                ApplicationStatuses.Rejected,
                _leaseLifecycleService.ValidateApplicationRejection);
        }

        [HttpPost("{id}/activate")]
        public IActionResult Activate(int id, [FromBody] LeaseActivationRequestDto request)
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }

        private async Task<IActionResult> ChangeApplicationStatus(
            int id,
            string nextStatus,
            Func<LeaseApplication, string, WorkflowValidationResult> validate)
        {
            var application = await _context.Applications
                .FirstOrDefaultAsync(a => a.ApplicationId == id);

            if (application == null)
                return NotFound("Application was not found.");

            var userRole = User.FindFirstValue(ClaimTypes.Role) ?? string.Empty;
            var validation = validate(application, userRole);

            if (!validation.IsValid)
                return BadRequest(validation.ErrorMessage);

            application.Status = nextStatus;
            await _context.SaveChangesAsync();

            return Ok(application);
        }
    }
}
