using System.ComponentModel.DataAnnotations;

namespace PropertyLeasing.API.Models
{
    public class MaintenanceRequest
    {
        [Key]
        public int RequestId { get; set; }

        public int UnitId { get; set; }

        public int TenantId { get; set; }

        public string Description { get; set; }

        public string Priority { get; set; }

        public DateTime ReportedAt { get; set; }

        public string Status { get; set; }

        public int? AssignedStaffId { get; set; }

        public Unit? Unit { get; set; }

        public Tenant? Tenant { get; set; }

        public Staff? AssignedStaff { get; set; }

        public ICollection<MaintenanceLog>? MaintenanceLogs { get; set; }
    }
}