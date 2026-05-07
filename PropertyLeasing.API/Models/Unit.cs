using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Net.Mime.MediaTypeNames;

namespace PropertyLeasing.API.Models
{
    public class Unit
    {
        [Key]
        public int UnitId { get; set; }

        [ForeignKey("Property")]
        public int PropertyId { get; set; }

        public string UnitNumber { get; set; }

        public decimal RentAmount { get; set; }

        public bool IsAvailable { get; set; }

        public string Size { get; set; }

        public string Amenities { get; set; }

        public Property? Property { get; set; }

        public ICollection<Application>? Applications { get; set; }

        public ICollection<Lease>? Leases { get; set; }

        public ICollection<MaintenanceRequest>? MaintenanceRequests { get; set; }
    }
}