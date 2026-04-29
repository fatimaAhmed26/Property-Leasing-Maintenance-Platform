using PropertyLeasingSystem.Models;

namespace PropertyLeasingSystem.Services
{
    public interface IMaintenanceLifecycleService
    {
        WorkflowValidationResult ValidateMaintenanceTransition(
            MaintenanceRequest request,
            string nextStatus,
            string userRole);
    }
}
