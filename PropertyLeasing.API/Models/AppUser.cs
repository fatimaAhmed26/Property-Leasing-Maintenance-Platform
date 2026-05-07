using Microsoft.AspNetCore.Identity;

namespace PropertyLeasing.API.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public int? TenantId { get; set; }
        public int? StaffId { get; set; }
    }
}
