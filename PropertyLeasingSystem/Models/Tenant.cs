using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace PropertyLeasingSystem.Models
{
    public class Tenant
    {
        [Key] //  Primary Key
        public int TenantId { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string Phone { get; set; }

        //  links the Tenant to their Applications
        public ICollection<Application> Applications { get; set; }
    }
}