using OutOfOffice.DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace OutOfOffice.DAL.DTO
{
    public record LeaveRequestDTO
    {

        public long EmployeeId { get; set; }
        [Required]
        public AbsenceReason AbsenceReason { get; set; }

        [Required]
        public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        [Required]
        public DateOnly EndDate { get; set; } = DateOnly.FromDateTime(DateTime.Now.AddDays(1));

        public string? Comment { get; set; } = string.Empty;
        public LeaveRequestStatus Status { get; set; }

        public long? ApprovalRequestId { get; set; }
    }
}
