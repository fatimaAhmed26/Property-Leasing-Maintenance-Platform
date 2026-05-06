using System.ComponentModel.DataAnnotations;

namespace PropertyLeasing.API.Models
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

        public int? ManagerId { get; set; }

        public Staff? Manager { get; set; }

        public ICollection<Unit>? Units { get; set; }
    }
}
