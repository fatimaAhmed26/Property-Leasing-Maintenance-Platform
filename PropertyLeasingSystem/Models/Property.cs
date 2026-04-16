using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PropertyLeasingSystem.Models
{
    public class Property
    {
        [Key]
        public int PropertyId { get; set; }

        [Required]
        public string PropertyName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string PropertyType { get; set; }

        // Links to the Staff member who manages this property
        public int ManagerId { get; set; }

        // Navigation property for Units
        public ICollection<Unit> Units { get; set; }
    }
}
