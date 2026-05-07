using System.ComponentModel.DataAnnotations;

namespace PropertyLeasing.API.DTOs
{
    public class SubmitMaintenanceRequestDto
    {
        [Required] public int UnitId { get; set; }
        [Required] public int TenantId { get; set; }
        [Required] public string Description { get; set; } = string.Empty;
        [Required] public string Priority { get; set; } = string.Empty;
    }
}
