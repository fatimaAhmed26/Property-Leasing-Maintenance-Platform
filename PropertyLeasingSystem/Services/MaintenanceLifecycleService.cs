using PropertyLeasingSystem.Models;

namespace PropertyLeasingSystem.Services
{
    public class MaintenanceLifecycleService : IMaintenanceLifecycleService
    {
        private static readonly Dictionary<string, HashSet<string>> AllowedTransitions = new()
        {
            [MaintenanceStatuses.Submitted] = new HashSet<string> { MaintenanceStatuses.Assigned },
            [MaintenanceStatuses.Assigned] = new HashSet<string> { MaintenanceStatuses.InProgress },
            [MaintenanceStatuses.InProgress] = new HashSet<string> { MaintenanceStatuses.Resolved },
            [MaintenanceStatuses.Resolved] = new HashSet<string> { MaintenanceStatuses.Closed }
        };

        private static readonly Dictionary<string, HashSet<string>> RolesByNextStatus = new()
        {
            [MaintenanceStatuses.Assigned] = new HashSet<string> { WorkflowRoles.PropertyManager },
            [MaintenanceStatuses.InProgress] = new HashSet<string> { WorkflowRoles.MaintenanceStaff },
            [MaintenanceStatuses.Resolved] = new HashSet<string> { WorkflowRoles.MaintenanceStaff },
            [MaintenanceStatuses.Closed] = new HashSet<string>
            {
                WorkflowRoles.PropertyManager,
                WorkflowRoles.Tenant
            }
        };

        public WorkflowValidationResult ValidateMaintenanceTransition(
            MaintenanceRequest request,
            string nextStatus,
            string userRole)
        {
            if (request == null)
                return WorkflowValidationResult.Failure("Maintenance request was not found.");

            if (string.IsNullOrWhiteSpace(nextStatus))
                return WorkflowValidationResult.Failure("Next status is required.");

            var currentStatus = NormalizeStatus(request.Status);
            var normalizedNextStatus = NormalizeStatus(nextStatus);

            if (!AllowedTransitions.TryGetValue(currentStatus, out var allowedStatuses) ||
                !allowedStatuses.Contains(normalizedNextStatus))
            {
                return WorkflowValidationResult.Failure(
                    $"Maintenance status cannot change from {currentStatus} to {normalizedNextStatus}.");
            }

            if (!IsRoleAllowed(normalizedNextStatus, userRole))
                return WorkflowValidationResult.Failure("Your role is not allowed to perform this status change.");

            return WorkflowValidationResult.Success();
        }

        private static bool IsRoleAllowed(string nextStatus, string userRole)
        {
            if (!RolesByNextStatus.TryGetValue(nextStatus, out var allowedRoles))
                return false;

            return allowedRoles.Any(role => string.Equals(role, userRole, StringComparison.OrdinalIgnoreCase));
        }

        private static string NormalizeStatus(string? status)
        {
            return string.IsNullOrWhiteSpace(status) ? MaintenanceStatuses.Submitted : status.Trim();
        }
    }
}
