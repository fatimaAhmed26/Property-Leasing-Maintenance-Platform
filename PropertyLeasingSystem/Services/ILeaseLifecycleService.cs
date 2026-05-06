using PropertyLeasingSystem.Models;

using LeaseApplication = PropertyLeasingSystem.Models.Application;

namespace PropertyLeasingSystem.Services
{
    public interface ILeaseLifecycleService
    {
        WorkflowValidationResult ValidateMoveToScreening(
            LeaseApplication application,
            string userRole);

        WorkflowValidationResult ValidateApplicationApproval(
            LeaseApplication application,
            string userRole);

        WorkflowValidationResult ValidateApplicationRejection(
            LeaseApplication application,
            string userRole);

        WorkflowValidationResult ValidateApplicationTransition(
            LeaseApplication application,
            string nextStatus,
            string userRole);

        WorkflowValidationResult ValidateLeaseActivation(
            LeaseApplication application,
            Unit unit,
            string userRole);

        WorkflowValidationResult ValidateLeaseActivation(
            LeaseApplication application,
            Unit unit,
            string userRole,
            DateTime startDate,
            DateTime endDate);

        WorkflowValidationResult ValidateLeaseDates(Lease lease);
    }
}
