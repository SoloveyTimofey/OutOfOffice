using OutOfOffice.DAL.Models;

namespace OutOfOffice.Services.Facade
{
    public interface ICreateApprivalRequestFacade
    {
        Task<ApprovalRequest> CreateApprovalRequest(ApprovalRequest approvalRequest);
    }
}
