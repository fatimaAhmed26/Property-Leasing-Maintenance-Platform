using PropertyLeasing.API.Models;

namespace PropertyLeasingSystem.Services
{
    public interface IPaymentService
    {
        WorkflowValidationResult ValidateRecordPayment(
            Lease lease,
            decimal amount,
            string userRole);

        bool IsLeaseOverdue(Lease lease, DateTime asOf);
    }
}
