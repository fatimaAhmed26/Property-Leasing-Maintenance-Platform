using System.ComponentModel.DataAnnotations;

namespace PropertyLeasingSystem.DTOs
{
    public class LeaseActivationRequestDto
    {
        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }
}
