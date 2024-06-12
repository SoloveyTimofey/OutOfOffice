using OutOfOffice.DAL.Models;

namespace OutOfOffice.DAL.Repositories.Interfaces
{
    public interface IUserActionRepository
    {
        Task<EmployeeBase> CreateNewEmployeeAsync(EmployeeBase employee);
        Task DeleteEmployeeAsync(long id);
        Task ChangeEmployeeActivity(bool activity, long empId);

        Task ChangeOutOfOfficeBalanceAsync(long outOfOfficeBalance, long empId);
    }
}
