using System.ComponentModel.DataAnnotations;

namespace PropertyLeasingSystem.Models
{
    public class MaintenanceRequest
    {
        [Key]
        public int RequestId { get; set; }
        public int UnitId { get; set; }
        public int TenantId { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public DateTime ReportedAt { get; set; } = DateTime.Now;
        public string Status { get; set; }
    }
}