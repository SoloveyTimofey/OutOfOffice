using OutOfOffice.DAL.Models;

namespace OutOfOffice.DAL.Repositories.Interfaces
{
    public interface IProjectGetRepository
    {
        IQueryable<Project> GetAllProjects();
        Task<Project> GetProjectById(long id);
        IQueryable<Project> GetProjectsByProjectManagerId(long id);
        IQueryable<Employee> GetEmployeesFromProject(long id);
        IQueryable<long> GetProjectManagerEmployeeIDs(long projectManagerId);
        IQueryable<Project> GetEmployeeProjects(long employeeId);
    }
}
