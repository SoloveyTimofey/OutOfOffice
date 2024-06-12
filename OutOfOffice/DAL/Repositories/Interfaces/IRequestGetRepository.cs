using OutOfOffice.DAL.Models;

namespace OutOfOffice.DAL.Repositories.Interfaces
{
    public interface IRequestGetRepository
    {
        IQueryable<LeaveRequest> GetAllLeaveRequests();

        IQueryable<LeaveRequest> GetLeaveRequestsByEmployeeIDs(List<long> emloyeeIDs);
        IQueryable<LeaveRequest> GetLeaveRequestsByEmployeeId(long emloyeeId);
        Task<LeaveRequest> GetLeaveRequestByIdAsync(long id);
    }
}
