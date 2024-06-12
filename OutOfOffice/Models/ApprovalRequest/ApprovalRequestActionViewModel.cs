using OutOfOffice.DAL.Models;

namespace OutOfOffice.Models.ApprovalRequest
{
    public class ApprovalRequestActionViewModel
    {
        public DAL.Models.ApprovalRequest ApprovalRequest { get; set; } = null!;

        public string Action { get; set; } = "Create";
    }
}
