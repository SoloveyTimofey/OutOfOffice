using OutOfOffice.DAL.Models;

namespace OutOfOffice.DAL.Repositories.Interfaces
{
    public interface IRequestActionRepository
    {
        Task<LeaveRequest> CreateLeaveRequestAsync(LeaveRequest leaveRequest);
        Task<LeaveRequest> UpdateLeaveRequestAsync(LeaveRequest leaveRequest);
        Task UpdateLeaveRequestStatusAsync(LeaveRequestStatus status, long id);
        Task RejectLeaveRequestAsync(long id);

        Task<ApprovalRequest> CreateApprovalRequestAsync(ApprovalRequest approvalRequest);
        Task<ApprovalRequest> UpdateApprovalRequestAsync(ApprovalRequest approvalRequest);
    }
}
