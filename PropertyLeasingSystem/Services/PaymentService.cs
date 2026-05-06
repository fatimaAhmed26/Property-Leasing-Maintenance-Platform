using PropertyLeasing.API.Models;

namespace PropertyLeasingSystem.Services
{
    public class PaymentService : IPaymentService
    {
        public WorkflowValidationResult ValidateRecordPayment(
            Lease lease,
            decimal amount,
            string userRole)
        {
            if (lease == null)
                return WorkflowValidationResult.Failure("Lease was not found.");

            if (!string.Equals(userRole, WorkflowRoles.PropertyManager, StringComparison.OrdinalIgnoreCase))
                return WorkflowValidationResult.Failure("Only a Property Manager can record payments.");

            if (amount <= 0)
                return WorkflowValidationResult.Failure("Payment amount must be greater than zero.");

            if (!string.Equals(lease.Status, ApplicationStatuses.LeaseActive, StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(lease.Status, ApplicationStatuses.Renewed, StringComparison.OrdinalIgnoreCase))
                return WorkflowValidationResult.Failure("Payments can only be recorded for active or renewed leases.");

            return WorkflowValidationResult.Success();
        }

        public bool IsLeaseOverdue(Lease lease, DateTime asOf)
        {
            if (lease == null || lease.Payments == null || !lease.Payments.Any())
                return false;

            var totalPaid = lease.Payments
                .Where(p => string.Equals(p.Status, PaymentStatuses.Paid, StringComparison.OrdinalIgnoreCase))
                .Sum(p => p.Amount);

            var monthsElapsed = ((asOf.Year - lease.StartDate.Year) * 12) + asOf.Month - lease.StartDate.Month;
            var expectedTotal = lease.MonthlyRent * Math.Max(monthsElapsed, 0);

            return totalPaid < expectedTotal;
        }
    }
}
