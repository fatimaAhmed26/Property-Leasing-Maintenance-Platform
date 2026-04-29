using PropertyLeasingSystem.Models;

using LeaseApplication = PropertyLeasingSystem.Models.Application;

namespace PropertyLeasingSystem.Services
{
    public class LeaseLifecycleService : ILeaseLifecycleService
    {
        private static readonly Dictionary<string, HashSet<string>> AllowedTransitions = new()
        {
            [ApplicationStatuses.Pending] = new HashSet<string> { ApplicationStatuses.Screening },
            [ApplicationStatuses.Screening] = new HashSet<string>
            {
                ApplicationStatuses.Approved,
                ApplicationStatuses.Rejected
            },
            [ApplicationStatuses.Approved] = new HashSet<string> { ApplicationStatuses.LeaseActive },
            [ApplicationStatuses.LeaseActive] = new HashSet<string>
            {
                ApplicationStatuses.Renewed,
                ApplicationStatuses.Terminated
            },
            [ApplicationStatuses.Renewed] = new HashSet<string> { ApplicationStatuses.Terminated }
        };

        public WorkflowValidationResult ValidateApplicationTransition(
            LeaseApplication application,
            string nextStatus,
            string userRole)
        {
            if (application == null)
                return WorkflowValidationResult.Failure("Application was not found.");

            if (!IsPropertyManager(userRole))
                return WorkflowValidationResult.Failure("Only a Property Manager can change application status.");

            if (string.IsNullOrWhiteSpace(nextStatus))
                return WorkflowValidationResult.Failure("Next status is required.");

            var currentStatus = NormalizeStatus(application.Status);
            var normalizedNextStatus = NormalizeStatus(nextStatus);

            if (!AllowedTransitions.TryGetValue(currentStatus, out var allowedStatuses) ||
                !allowedStatuses.Contains(normalizedNextStatus))
            {
                return WorkflowValidationResult.Failure(
                    $"Application status cannot change from {currentStatus} to {normalizedNextStatus}.");
            }

            return WorkflowValidationResult.Success();
        }

        public WorkflowValidationResult ValidateLeaseActivation(
            LeaseApplication application,
            Unit unit,
            string userRole)
        {
            var transitionResult = ValidateApplicationTransition(
                application,
                ApplicationStatuses.LeaseActive,
                userRole);

            if (!transitionResult.IsValid)
                return transitionResult;

            if (unit == null)
                return WorkflowValidationResult.Failure("Unit was not found.");

            if (!unit.IsAvailable)
                return WorkflowValidationResult.Failure("This unit is not available for leasing.");

            return WorkflowValidationResult.Success();
        }

        private static bool IsPropertyManager(string userRole)
        {
            return string.Equals(userRole, WorkflowRoles.PropertyManager, StringComparison.OrdinalIgnoreCase);
        }

        private static string NormalizeStatus(string? status)
        {
            return string.IsNullOrWhiteSpace(status) ? ApplicationStatuses.Pending : status.Trim();
        }
    }
}
