using PropertyLeasingSystem.Models;

using LeaseApplication = PropertyLeasingSystem.Models.Application;

namespace PropertyLeasingSystem.Services
{
    public interface ILeaseLifecycleService
    {
        WorkflowValidationResult ValidateApplicationTransition(
            LeaseApplication application,
            string nextStatus,
            string userRole);

        WorkflowValidationResult ValidateLeaseActivation(
            LeaseApplication application,
            Unit unit,
            string userRole);
    }
}
