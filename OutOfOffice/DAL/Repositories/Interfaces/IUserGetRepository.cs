using OutOfOffice.DAL.Models;

namespace OutOfOffice.DAL.Repositories.Interfaces
{
    public interface IUserGetRepository
    {
        IQueryable<EmployeeBase> GetAllEmployees();
        IQueryable<HRManager> GetAllHRsAsync();
        IQueryable<Employee> GetHREmployees(long hrID);
        Task<EmployeeBase> GetEmployeeByIdAsync(long id);
        IQueryable<EmployeeBase> GetEmployeeByIdAsync(List<long> IDs);
    }
}
