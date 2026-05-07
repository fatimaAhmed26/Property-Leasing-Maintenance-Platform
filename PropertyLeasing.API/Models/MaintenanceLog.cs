using System.ComponentModel.DataAnnotations;

namespace PropertyLeasing.API.Models
{
    public class MaintenanceLog
    {
        [Key]
        public int LogId { get; set; }

        public int RequestId { get; set; }

        public int StaffId { get; set; }

        public DateTime WorkStarted { get; set; }

        public DateTime? WorkCompleted { get; set; }

        public string ActionTaken { get; set; }

        public MaintenanceRequest? MaintenanceRequest { get; set; }

        public Staff? Staff { get; set; }
    }
}
