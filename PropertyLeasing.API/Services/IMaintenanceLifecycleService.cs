using PropertyLeasing.API.Models;

namespace PropertyLeasing.API.Services
{
    public interface IMaintenanceLifecycleService
    {
        WorkflowValidationResult ValidateMaintenanceTransition(
            MaintenanceRequest request,
            string nextStatus,
            string userRole);

        WorkflowValidationResult ValidateStaffAssignment(
            MaintenanceRequest request,
            Staff staff,
            string userRole);
    }
}
