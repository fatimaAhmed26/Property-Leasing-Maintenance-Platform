using System.ComponentModel.DataAnnotations;

namespace PropertyLeasingSystem.Models
{
    public class Staff
    {
        [Key]
        public int StaffId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string RoleType { get; set; } 
        public decimal HourlyRate { get; set; }
    }
}
