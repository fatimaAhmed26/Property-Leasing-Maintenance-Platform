using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PropertyLeasingSystem.Models
{
    public class MaintenanceLog
    {
        [Key]
        public int LogId { get; set; }

        public int RequestId { get; set; }
        [ForeignKey("RequestId")]
        public MaintenanceRequest Request { get; set; }

        public int StaffId { get; set; }
        [ForeignKey("StaffId")]
        public Staff Staff { get; set; }

        public DateTime WorkStarted { get; set; }
        public DateTime WorkCompleted { get; set; }
        public string ActionTaken { get; set; }
    }
}
