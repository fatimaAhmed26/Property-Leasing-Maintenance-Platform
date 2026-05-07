namespace PropertyLeasing.API.DTOs
{
    public class MaintenanceLookupResultDto
    {
        public string Status { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public DateTime ReportedAt { get; set; }
        public string TenantName { get; set; } = string.Empty;
        public List<MaintenanceLogDto> Logs { get; set; } = new();
    }

    public class MaintenanceLogDto
    {
        public string ActionTaken { get; set; } = string.Empty;
        public DateTime WorkStarted { get; set; }
        public DateTime? WorkCompleted { get; set; }
    }
}
