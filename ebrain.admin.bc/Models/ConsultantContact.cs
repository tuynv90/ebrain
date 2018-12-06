using System;

namespace ebrain.admin.bc.Models
{
    public class ConsultantContact : HistoricalEntity
    {
        public Guid ConsultantContactId { get; set; }
        public string ConsultantContactCode { get; set; }
        public string ConsultantContactName { get; set; }
        public string PhoneContact { get; set; }
        public string ContactName { get; set; }
        public Guid? BranchId { get; set; }
        public DateTime? ScheduleStartDate { get; set; }
        public DateTime? ScheduleEndDate { get; set; }
        public string ScheduleNote { get; set; }
        public bool IsConfirm { get; set; }
    }
}