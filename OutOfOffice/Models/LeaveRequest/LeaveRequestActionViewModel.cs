namespace OutOfOffice.Models.LeaveRequest
{
    public class LeaveRequestActionViewModel
    {
        public DAL.Models.LeaveRequest LeaveRequest { get; set; } = null!;

        public string Action { get; set; } = "Create";
    }
}
