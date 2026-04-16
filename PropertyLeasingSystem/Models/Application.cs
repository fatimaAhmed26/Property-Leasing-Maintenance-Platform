using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PropertyLeasingSystem.Models
{
    public class Application
    {
        [Key]
        public int ApplicationId { get; set; }

        public int TenantId { get; set; }
        [ForeignKey("TenantId")]
        public Tenant Tenant { get; set; }

        public int UnitId { get; set; }
        [ForeignKey("UnitId")]
        public Unit Unit { get; set; }

        public DateTime SubmittedAt { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Pending";
    }
}