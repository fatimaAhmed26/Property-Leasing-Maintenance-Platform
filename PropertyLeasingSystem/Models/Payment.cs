using System.ComponentModel.DataAnnotations;

namespace PropertyLeasingSystem.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        public int LeaseId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentType { get; set; } // Rent or Service
        public string Status { get; set; }
    }
}