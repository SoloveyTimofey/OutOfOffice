using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OutOfOffice.DAL.Models;
using OutOfOffice.DAL.Repositories.Interfaces;
using OutOfOffice.StaticClasses;
using OutOfOffice.Models.Project;
using OutOfOffice.DAL.DTO;

namespace OutOfOffice.Controllers
{
    [Authorize(Roles = "Project Manager,HR Manager,Administrator")]
    public class ProjectController : Controller
    {
        private readonly IProjectGetRepository projectGetRepository;
        private readonly IProjectActionRepository projectActionRepository;
        private readonly IUserGetRepository userGetRepository;
        public ProjectController(IProjectGetRepository projectGetRepository, IProjectActionRepository projectActionRepository, IUserGetRepository userGetRepository)
        {
            this.projectActionRepository = projectActionRepository;
            this.projectGetRepository = projectGetRepository;
            this.userGetRepository = userGetRepository;
        }

        [Authorize(Roles = "Project Manager,Administrator")]
        public async Task<IActionResult> Index()
        {
            EmployeeBase emp = CurrentUserGetter.GetCurrentUser(HttpContext);

            List<Project> projects = await projectGetRepository.GetProjectsByProjectManagerId(emp.Id).ToListAsync();

            return View(projects);
        }

        [Authorize(Roles = "Project Manager,Administrator")]
        public async Task<IActionResult> CreateProject()
        {
            IQueryable<EmployeeBase> employees = userGetRepository.GetAllEmployees();
            List<EmployeeStatusViewModel> allEmployees = await (from emp in employees
                                                         select new EmployeeStatusViewModel
                                                         {
                                                             Employee = new EmployeeDTO
                                                             {
                                                                 Id = emp.Id,
                                                                 FullName = emp.FullName,
                                                                 Position = emp.Position,
                                                                 Subdivision = emp.Subdivition
                                                             },
                                                             IsSelected = false
                                                         }).ToListAsync();

            return View("ProjectAction", new ProjectActionViewModel
            {
                Project = new Project(),
                Employees = allEmployees
            });
        }

        [HttpPost]
        [Authorize(Roles = "Project Manager,Administrator")]
        public async Task<IActionResult> CreateProject([FromForm] Project project, List<EmployeeStatusViewModel> employees)
        {
            ModelState.Remove("project.ProjectManager");
            if (project.EndDate <= project.StartDate)
            {
                ModelState.AddModelError("Date", "End Date must be greater then Start Date");
            }
            if (ModelState.IsValid)
            {
                EmployeeBase pm = CurrentUserGetter.GetCurrentUser(HttpContext);
                project.ProjectManagerId = pm.Id;
                Project createdProject = await projectActionRepository.CreateProjectAsync(project);

                IEnumerable<long> selectedEmployeeIDs = from emp in employees where emp.IsSelected select emp.Employee.Id;
                List<EmployeeProjectJunction> employeeProjectJunctions = new List<EmployeeProjectJunction>();
                foreach (long empId in selectedEmployeeIDs)
                {
                    employeeProjectJunctions.Add(new EmployeeProjectJunction
                    {
                        EmployeeId = empId,
                        ProjectId = createdProject.Id
                    });
                }

                await projectActionRepository.AddRangeEmployeeProjectJunctionsAsync(employeeProjectJunctions);

                return RedirectToAction("Index", "Project");
            }

            return RedirectToAction(nameof(CreateProject));
        }

        [Authorize(Roles = "Project Manager,Administrator")]
        public async Task<IActionResult> UpdateProject(long id)
        {
            ModelState.Remove("ProjectManager");
            IQueryable<EmployeeBase> employees = userGetRepository.GetAllEmployees();
            List<EmployeeStatusViewModel> allEmployes = await(from emp in employees
                                                               select new EmployeeStatusViewModel
                                                               {
                                                                   Employee = new EmployeeDTO
                                                                   {
                                                                       Id = emp.Id,
                                                                       FullName = emp.FullName,
                                                                       Position = emp.Position,
                                                                       Subdivision = emp.Subdivition
                                                                   },
                                                                   IsSelected = false
                                                               }).ToListAsync();
            List<Employee> projectEmployees = await projectGetRepository.GetEmployeesFromProject(id).ToListAsync();

            foreach (var vm in allEmployes)
            {
                if (projectEmployees.Any(pe=>pe.Id==vm.Employee.Id))
                {
                    vm.IsSelected = true;
                    continue;
                }
            }

            Project project = await projectGetRepository.GetProjectById(id);
            return View("ProjectAction", new ProjectActionViewModel
            {
                Project = project,
                Action = "Update",
                Employees = allEmployes,
            });
        }

        [HttpPost]
        [Authorize(Roles = "Project Manager,Administrator")]
        public async Task<IActionResult> UpdateProject(Project project, List<EmployeeStatusViewModel> employees)
        {
            ModelState.Remove("project.ProjectManager");
            if (project.EndDate <= project.StartDate)
            {
                ModelState.AddModelError("Date", "End Date must be greater then Start Date");
            }
            if (ModelState.IsValid)
            {
                await projectActionRepository.UpdateProjectAsync(project);
                await projectActionRepository.DeleteJunctionsWithProjectAsync(project.Id);

                List<EmployeeProjectJunction> employeeProjectJunctions = new List<EmployeeProjectJunction>();
                foreach (var vm in employees.Where(emp=>emp.IsSelected==true))
                {
                    employeeProjectJunctions.Add(new EmployeeProjectJunction
                    {
                        EmployeeId = vm.Employee.Id,
                        ProjectId = project.Id
                    });
                }

                await projectActionRepository.AddRangeEmployeeProjectJunctionsAsync(employeeProjectJunctions);

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(UpdateProject));
        }

        [Authorize(Roles = "Project Manager,Administrator")]
        public async Task<IActionResult> ChangeProjectActivity(long id, bool active)
        {
            await projectActionRepository.ChangeProjectActivity(id, active);

            return RedirectToAction("Index", "Project");
        }

    }
}
