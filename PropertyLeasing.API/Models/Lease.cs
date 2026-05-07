using System.ComponentModel.DataAnnotations;

namespace PropertyLeasing.API.Models
{
    public class Lease
    {
        [Key]
        public int LeaseId { get; set; }

        public int ApplicationId { get; set; }

        public int UnitId { get; set; }

        public int TenantId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal MonthlyRent { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Status { get; set; }

        public Application? Application { get; set; }

        public Unit? Unit { get; set; }

        public Tenant? Tenant { get; set; }

        public ICollection<Payment>? Payments { get; set; }
    }
}