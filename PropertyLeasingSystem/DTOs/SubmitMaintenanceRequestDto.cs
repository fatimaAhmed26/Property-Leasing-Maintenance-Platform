using System.ComponentModel.DataAnnotations;

namespace PropertyLeasingSystem.DTOs
{
    public class SubmitMaintenanceRequestDto
    {
        [Required]
        public int UnitId { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Priority { get; set; }
    }
}
