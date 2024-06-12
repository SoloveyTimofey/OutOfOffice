using OutOfOffice.DAL.Models;

namespace OutOfOffice.DAL.Repositories.Interfaces
{
    public interface IProjectActionRepository
    {
        Task<Project> CreateProjectAsync(Project project);
        Task DeleteJunctionsWithProjectAsync(long projectId);
        Task AddRangeEmployeeProjectJunctionsAsync(List<EmployeeProjectJunction> employeeProjectJunctions);
        Task<Project> UpdateProjectAsync(Project project);
        Task ChangeProjectActivity(long id, bool isActive);
    }
}
