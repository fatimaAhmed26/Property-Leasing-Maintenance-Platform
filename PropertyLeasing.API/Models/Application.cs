using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PropertyLeasing.API.Models
{
    public class Application
    {
        [Key]
        public int ApplicationId { get; set; }

        public int TenantId { get; set; }

        public int UnitId { get; set; }

        public DateTime SubmittedAt { get; set; }

        public DateTime? ProcessedAt { get; set; }

        public string Status { get; set; }

        public int? ApprovedByStaffId { get; set; }

        public Tenant? Tenant { get; set; }

        public Unit? Unit { get; set; }

        public Staff? ApprovedByStaff { get; set; }
    }
}