using OutOfOffice.DAL.Models;
using OutOfOffice.DAL.Repositories.Interfaces;

namespace OutOfOffice.Services.Facade
{
    public class CreateApprovalRequestFacade : ICreateApprivalRequestFacade
    {
        private readonly IRequestActionRepository requestActionRepository;
        private readonly IRequestGetRepository requestGetRepository;
        private readonly IUserGetRepository userGetRepository;
        private readonly IUserActionRepository userActionRepository;
        public CreateApprovalRequestFacade(IRequestActionRepository requestActionRepository, IRequestGetRepository requestGetRepository, IUserGetRepository userGetRepository, IUserActionRepository userActionRepository)
        {
            this.requestActionRepository = requestActionRepository;
            this.requestGetRepository = requestGetRepository;
            this.userGetRepository = userGetRepository;
            this.userActionRepository = userActionRepository;
        }
        public async Task<ApprovalRequest> CreateApprovalRequest(ApprovalRequest approvalRequest)
        {
            ApprovalRequest ar = await requestActionRepository.CreateApprovalRequestAsync(approvalRequest);

            LeaveRequestStatus leaveRequestStatus;
            if (ar.IsApproved)
            {
                leaveRequestStatus = LeaveRequestStatus.Approved;
            }
            else
            {
                leaveRequestStatus = LeaveRequestStatus.Canceled;
            }
            LeaveRequest lr = await requestGetRepository.GetLeaveRequestByIdAsync(approvalRequest.LeaveRequestId);
            lr.ApprovalRequestId = ar.Id;
            lr.Status = leaveRequestStatus;

            await requestActionRepository.UpdateLeaveRequestAsync(lr);

            EmployeeBase emp = await userGetRepository.GetEmployeeByIdAsync(lr.EmployeeId);

            long valueToSubtract = lr.EndDate.Day - lr.StartDate.Day;
            emp.OutOfOfficeBalance = emp.OutOfOfficeBalance-valueToSubtract;

            await userActionRepository.ChangeOutOfOfficeBalanceAsync(valueToSubtract, emp.Id);

            return ar;

        }
    }
}
