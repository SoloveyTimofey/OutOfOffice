namespace OutOfOffice.DAL.Models
{
    public class LeaveRequest
    {
        public long Id { get; set; }

        public long EmployeeId { get; set; }
        public EmployeeBase Employee { get; set; } = null!;
        public AbsenceReason AbsenceReason { get; set; }

        public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public DateOnly EndDate { get; set; } = DateOnly.FromDateTime(DateTime.Now.AddDays(1));

        public string? Comment { get; set; }
        public LeaveRequestStatus Status { get; set; }

        public long? ApprovalRequestId { get; set; }
        public ApprovalRequest? ApprovalRequest { get; set; }
    }

    public enum AbsenceReason
    {
        Ilness,
        FamilyEmergency,
        BusinessTravel
    }

    public enum LeaveRequestStatus
    {
        New,
        Approved,
        Canceled
    }
}
