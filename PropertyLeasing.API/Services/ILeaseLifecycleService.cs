using PropertyLeasing.API.Models;

using LeaseApplication = PropertyLeasing.API.Models.Application;

namespace PropertyLeasing.API.Services
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
