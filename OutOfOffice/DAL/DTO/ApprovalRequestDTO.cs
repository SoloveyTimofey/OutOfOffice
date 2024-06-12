namespace OutOfOffice.DAL.DTO
{
    public record ApprovalRequestDTO
    {
        public long ApproverId { get; set; }

        public long LeaveRequestId { get; set; }

        public bool IsApproved { get; set; }
        public string? Comment { get; set; }
    }
}
