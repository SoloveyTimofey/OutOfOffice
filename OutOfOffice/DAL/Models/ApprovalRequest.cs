namespace OutOfOffice.DAL.Models
{
    public class ApprovalRequest
    {
        public long Id { get; set; }
        
        public long ApproverId { get; set; }
        public EmployeeBase Approver { get; set; } = null!;

        public long LeaveRequestId { get; set; }
        public LeaveRequest LeaveRequest { get; set; } = null!;

        public bool IsApproved { get; set; }
        public string? Comment { get; set; }
    }

    
}
