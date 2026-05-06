using System.ComponentModel.DataAnnotations;

namespace PropertyLeasing.API.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        public int LeaseId { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }

        public DateTime TransactionTimestamp { get; set; }

        public string Method { get; set; }

        public string PaymentType { get; set; }

        public string Status { get; set; }

        public Lease? Lease { get; set; }
    }
}