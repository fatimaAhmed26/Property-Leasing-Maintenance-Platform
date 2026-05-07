using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace PropertyLeasing.API.Models
{
    public class Tenant
    {
        [Key]
        public int TenantId { get; set; }

        [Required]
        public string FullName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public ICollection<Application>? Applications { get; set; }

        public ICollection<Lease>? Leases { get; set; }

        public ICollection<MaintenanceRequest>? MaintenanceRequests { get; set; }
    }
}