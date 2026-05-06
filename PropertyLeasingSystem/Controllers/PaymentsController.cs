using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertyLeasing.API.Data;
using PropertyLeasing.API.Models;
using PropertyLeasingSystem.Services;
using System.Security.Claims;

namespace PropertyLeasingSystem.Controllers
{
    [Authorize(Roles = WorkflowRoles.PropertyManager)]
    public class PaymentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPaymentService _paymentService;

        public PaymentsController(
            ApplicationDbContext context,
            IPaymentService paymentService)
        {
            _context = context;
            _paymentService = paymentService;
        }

        public async Task<IActionResult> Index()
        {
            var leases = await _context.Leases
                .Include(l => l.Tenant)
                .Include(l => l.Unit)
                .Include(l => l.Payments)
                .Where(l => l.Status == ApplicationStatuses.LeaseActive ||
                            l.Status == ApplicationStatuses.Renewed)
                .ToListAsync();

            var overdueLeaseIds = leases
                .Where(l => _paymentService.IsLeaseOverdue(l, DateTime.UtcNow))
                .Select(l => l.LeaseId)
                .ToHashSet();

            ViewBag.OverdueLeaseIds = overdueLeaseIds;

            return View(leases);
        }

        [HttpPost]
        public async Task<IActionResult> RecordPayment(int leaseId, decimal amount, string method, string paymentType)
        {
            var lease = await _context.Leases
                .Include(l => l.Payments)
                .FirstOrDefaultAsync(l => l.LeaseId == leaseId);

            if (lease == null)
            {
                TempData["Error"] = "Lease was not found.";
                return RedirectToAction(nameof(Index));
            }

            var userRole = User.FindFirstValue(ClaimTypes.Role) ?? string.Empty;
            var validation = _paymentService.ValidateRecordPayment(lease, amount, userRole);

            if (!validation.IsValid)
            {
                TempData["Error"] = validation.ErrorMessage;
                return RedirectToAction(nameof(Index));
            }

            var payment = new Payment
            {
                LeaseId = leaseId,
                Amount = amount,
                PaymentDate = DateTime.UtcNow,
                TransactionTimestamp = DateTime.UtcNow,
                Method = method,
                PaymentType = paymentType,
                Status = PaymentStatuses.Paid
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Payment of {amount:C} recorded successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> LeasePayments(int leaseId)
        {
            var lease = await _context.Leases
                .Include(l => l.Tenant)
                .Include(l => l.Unit)
                .Include(l => l.Payments)
                .FirstOrDefaultAsync(l => l.LeaseId == leaseId);

            if (lease == null)
            {
                TempData["Error"] = "Lease was not found.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.IsOverdue = _paymentService.IsLeaseOverdue(lease, DateTime.UtcNow);

            return View(lease);
        }
    }
}
