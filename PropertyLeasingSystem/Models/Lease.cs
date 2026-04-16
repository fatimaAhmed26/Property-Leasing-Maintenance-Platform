using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PropertyLeasingSystem.Models
{
    public class Lease
    {
        [Key]
        public int LeaseId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }
    }
}