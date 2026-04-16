using System.ComponentModel.DataAnnotations;

namespace PropertyLeasingSystem.Models
{
    public class StatusHistory
    {
        [Key]
        public int AuditId { get; set; }
        public string EntityName { get; set; } // e.g., "Unit" or "Lease"
        public int EntityId { get; set; }
        public string OldStatus { get; set; }
        public string NewStatus { get; set; }
        public DateTime ChangedAt { get; set; } = DateTime.Now;
    }
}
