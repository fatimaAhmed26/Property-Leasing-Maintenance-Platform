using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PropertyLeasingSystem.Models
{
    public class Unit
    {
        [Key]
        public int UnitId { get; set; }

        [Required]
        public int PropertyId { get; set; }

        [ForeignKey("PropertyId")]
        public Property Property { get; set; }

        public string UnitNumber { get; set; }

        [Column(TypeName = "decimal(18,2)")] // Essential for money
        public decimal RentAmount { get; set; }

        public bool IsAvailable { get; set; }

        public string Size { get; set; } 

        public string Amenities { get; set; }
    }
}