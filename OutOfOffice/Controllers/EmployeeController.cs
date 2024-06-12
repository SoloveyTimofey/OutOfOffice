using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OutOfOffice.DAL.Models;
using OutOfOffice.DAL.Repositories.Interfaces;
using OutOfOffice.StaticClasses;

namespace OutOfOffice.Controllers
{
    [Authorize(Roles = "Employee,Project Manager,HR Manager,Administrator")]
    public class EmployeeController : Controller
    {
        private readonly IUserGetRepository userGetRepository;
        private readonly IUserActionRepository userActionRepository;
        private readonly IProjectGetRepository projectGetRepository;
        public EmployeeController(IUserGetRepository userGetRepository, IUserActionRepository userActionRepository, IProjectGetRepository projectGetRepository)
        {
            this.userGetRepository = userGetRepository;
            this.userActionRepository = userActionRepository;
            this.projectGetRepository = projectGetRepository;
        }

        [Authorize(Roles = "HR Manager,Administrator")]
        public async Task<IActionResult> GetSpecifiedEmployees()
        {
            EmployeeBase emp = CurrentUserGetter.GetCurrentUser(HttpContext);
            List<long> employeeIDs = await userGetRepository.GetHREmployees(emp.Id).Select(emp => emp.Id).ToListAsync();

            return View("Employees", await userGetRepository.GetEmployeeByIdAsync(employeeIDs).ToListAsync());
        }

        [Authorize(Roles = "HR Manager,Administrator")]
        public async Task<IActionResult> ChangeActivity(bool activity, long empId)
        {
            await userActionRepository.ChangeEmployeeActivity(activity, empId);
            return RedirectToAction(nameof(GetSpecifiedEmployees));
        }

        [Authorize(Roles = "Employee,HR Manager,Administrator")]
        public async Task<IActionResult> EmployeeProjects()
        {
            EmployeeBase emp = CurrentUserGetter.GetCurrentUser(HttpContext);

            return View(await projectGetRepository.GetEmployeeProjects(emp.Id).ToListAsync());
        }
    }
}
