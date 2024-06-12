using Microsoft.EntityFrameworkCore;
using OutOfOffice.DAL.Contexts;
using OutOfOffice.DAL.Models;
using OutOfOffice.DAL.Repositories.Interfaces;

namespace OutOfOffice.DAL.Repositories.Realisations
{
    public class RequestRepository : IRequestGetRepository, IRequestActionRepository
    {
        private readonly OutOfOfficeDbContext context;
        public RequestRepository(OutOfOfficeDbContext context)
        {
            this.context = context;
        }

        #region Get
        public IQueryable<LeaveRequest> GetAllLeaveRequests()
        {
            return context.LeaveRequests;
        }
        public IQueryable<LeaveRequest> GetLeaveRequestsByEmployeeId(long emloyeeId)
        {
            return context.LeaveRequests.Where(lr=>lr.EmployeeId == emloyeeId);
        }
        public IQueryable<LeaveRequest> GetLeaveRequestsByEmployeeIDs(List<long> employeeIds)
        {
            return context.LeaveRequests.Where(lr => employeeIds.Any(empId => empId == lr.EmployeeId))
                .Include(lr => lr.ApprovalRequest).ThenInclude(ar => ar!.Approver)
                .Include(lr => lr.Employee);
        }

        public IQueryable<ApprovalRequest> GetApprovalRequestsByEmployeeId(List<long> employeeIDs)
        {
            return context.ApprovalRequests.Where(ap=>employeeIDs.Any(empdId =>empdId==ap.Id)).Include(ar=>ar.LeaveRequest);
        }

        public async Task<LeaveRequest> GetLeaveRequestByIdAsync(long id)
        {
            return await context.LeaveRequests.FirstOrDefaultAsync(lr => lr.Id == id) ?? throw new ArgumentException(nameof(id));
        }
        #endregion

        #region Action
        public async Task<LeaveRequest> CreateLeaveRequestAsync(LeaveRequest leaveRequest)
        {
            await context.LeaveRequests.AddAsync(leaveRequest);
            await context.SaveChangesAsync();

            return context.Entry(leaveRequest).Entity;
        }

        public async Task<LeaveRequest> UpdateLeaveRequestAsync(LeaveRequest leaveRequest)
        {
            LeaveRequest requestToUpdate = await context.LeaveRequests.FindAsync(leaveRequest.Id) ?? throw new ArgumentException(nameof(leaveRequest));

            requestToUpdate.StartDate = leaveRequest.StartDate;
            requestToUpdate.EndDate = leaveRequest.EndDate;
            requestToUpdate.Comment = leaveRequest.Comment;
            requestToUpdate.Status = leaveRequest.Status;
            requestToUpdate.ApprovalRequest = leaveRequest.ApprovalRequest;

            await context.SaveChangesAsync();

            return requestToUpdate;
        }

        public async Task UpdateLeaveRequestStatusAsync(LeaveRequestStatus status, long id)
        {
            LeaveRequest requestToUpdate = await context.LeaveRequests.FindAsync(id) ?? throw new ArgumentException(nameof(id));

            requestToUpdate.Status = status;

            await context.SaveChangesAsync();
        }

        public async Task RejectLeaveRequestAsync(long id)
        {
            LeaveRequest requestToUpReject = await context.LeaveRequests.FindAsync(id) ?? throw new ArgumentException(nameof(id));

            requestToUpReject.Status = LeaveRequestStatus.Canceled;

            await context.SaveChangesAsync();
        }

        public async Task<ApprovalRequest> CreateApprovalRequestAsync(ApprovalRequest approvalRequest)
        {
            await context.ApprovalRequests.AddAsync(approvalRequest);
            await context.SaveChangesAsync();

            return context.Entry(approvalRequest).Entity;
        }

        public async Task<ApprovalRequest> UpdateApprovalRequestAsync(ApprovalRequest approvalRequest)
        {
            ApprovalRequest requestToUpdate = await context.ApprovalRequests.FindAsync(approvalRequest.Id) ?? throw new ArgumentException(nameof(approvalRequest));

            requestToUpdate.IsApproved = approvalRequest.IsApproved;
            requestToUpdate.Comment = approvalRequest.Comment;

            await context.SaveChangesAsync();

            return requestToUpdate;
        }
        #endregion
    }
}
