using OutOfOffice.DAL.Contexts;
using OutOfOffice.DAL.Repositories.Interfaces;
using OutOfOffice.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace OutOfOffice.DAL.Repositories.Realisations
{
    public class ProjectRepository : IProjectGetRepository, IProjectActionRepository
    {
        private readonly OutOfOfficeDbContext context;
        public ProjectRepository(OutOfOfficeDbContext context)
        {
            this.context = context;
        }

        #region Get
        public async Task<Project> GetProjectById(long id)
        {
            return await context.Projects.FirstOrDefaultAsync(p => p.Id == id)??throw new ArgumentException(nameof(id));
        }
        public IQueryable<Project> GetAllProjects()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Project> GetProjectsByProjectManagerId(long id)
        {
            return context.Projects.Include(p => p.Employees);
        }

        public IQueryable<Employee> GetEmployeesFromProject(long id)
        {
            return context.EmployeeProjectJunctions.Where(epj=>epj.ProjectId == id).Include(epj=>epj.Employee).Select(epj=>epj.Employee);
        }

        public IQueryable<long> GetProjectManagerEmployeeIDs(long projectManagerId)
        {
            return from p in context.Projects where p.ProjectManagerId == projectManagerId
                      join epj in context.EmployeeProjectJunctions
                      on p.Id equals epj.ProjectId
                      join e in context.Employees
                      on epj.EmployeeId equals e.Id select e.Id;

        }

        public IQueryable<Project> GetEmployeeProjects(long employeeId)
        {
            return from p in context.Projects
                   join epj in context.EmployeeProjectJunctions
                   on p.Id equals epj.ProjectId
                   where epj.EmployeeId == employeeId
                   select p;
        }
        #endregion

        #region Action
        public async Task AddRangeEmployeeProjectJunctionsAsync(List<EmployeeProjectJunction> employeeProjectJunctions)
        {
            await context.EmployeeProjectJunctions.AddRangeAsync(employeeProjectJunctions);
            await context.SaveChangesAsync();
        }

        public async Task<Project> CreateProjectAsync(Project project)
        {
            await context.Projects.AddAsync(project);
            await context.SaveChangesAsync();

            return context.Entry(project).Entity;
        }

        public async Task<Project> UpdateProjectAsync(Project project)
        {
            Project projectToUpdate = await context.Projects.FindAsync(project.Id) ?? throw new ArgumentException(nameof(project));

            projectToUpdate.ProjectType = project.ProjectType;
            projectToUpdate.StartDate = project.StartDate;
            projectToUpdate.EndDate = project.EndDate;
            projectToUpdate.Comment = project.Comment;
            projectToUpdate.IsActive = project.IsActive;

            await context.SaveChangesAsync();

            return projectToUpdate;
        }

        public async Task ChangeProjectActivity(long id, bool isActive)
        {
            Project projectToUpdate = await context.Projects.FindAsync(id) ?? throw new ArgumentException(nameof(id));

            projectToUpdate.IsActive = isActive;

            await context.SaveChangesAsync();

        }

        public async Task DeleteJunctionsWithProjectAsync(long projectId)
        {
            var junctionsToDelete = context.EmployeeProjectJunctions.Where(epj => epj.ProjectId == projectId);
            await Task.Run(() =>
            {
                context.RemoveRange(junctionsToDelete);
            });
        }
        #endregion
    }
}
