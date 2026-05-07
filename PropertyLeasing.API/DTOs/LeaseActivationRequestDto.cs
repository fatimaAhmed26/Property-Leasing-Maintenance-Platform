using System.ComponentModel.DataAnnotations;

namespace PropertyLeasing.API.DTOs
{
    public class LeaseActivationRequestDto
    {
        [Required] public DateTime StartDate { get; set; }
        [Required] public DateTime EndDate { get; set; }
    }
}
