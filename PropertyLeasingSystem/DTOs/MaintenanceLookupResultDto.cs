namespace PropertyLeasingSystem.DTOs
{
    public class MaintenanceLookupResultDto
    {
        public int TicketNumber { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public DateTime ReportedAt { get; set; }
        public string TenantName { get; set; }
    }
}
