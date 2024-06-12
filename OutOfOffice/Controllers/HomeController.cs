using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OutOfOffice.DAL.Models;
using OutOfOffice.DAL.Repositories.Interfaces;
using OutOfOffice.Infrastructure;
using OutOfOffice.Models.Account;

namespace OutOfOffice.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserGetRepository userGetRepository;
        public HomeController(UserManager<ApplicationUser> userManager, IUserGetRepository userGetRepository)
        {
            this.userManager = userManager;
            this.userGetRepository = userGetRepository;
        }

        public async Task<IActionResult> Index()
        {
            if (!(User?.Identity?.IsAuthenticated)??false)
            {
                return RedirectToAction("SignIn", "Account");
            }

            var user = await userManager.GetUserAsync(User!);
            if (user == null)
            {
                return NotFound("User not found");
            }
            EmployeeBase emp = await userGetRepository.GetEmployeeByIdAsync(user.EmployeeId);

            EmployeeType empType;
            if (emp is Employee)
            {
                empType = EmployeeType.Employee;
            }
            else if(emp is HRManager)
            {
                empType = EmployeeType.HR;
            }
            else if(emp is ProjectManager)
            {
                empType = EmployeeType.ProjectManager;
            }
            else
            {
                throw new Exception("Unknown employee type");
            }

            HttpContext.Session.SetJson("EmployeeType", empType);
            HttpContext.Session.SetJson("CurrentUser", emp);

            if (User?.IsInRole("Employee")??false)
            {
                return RedirectToAction("Index","LeaveRequest");
            }
            else if(User?.IsInRole("Project Manager")?? false)
            {
                return RedirectToAction("Index", "Project");
            }
            else if(User?.IsInRole("HR Manager") ?? false)
            {
                return RedirectToAction("Index", "ApprovalRequest");
            }
            else
            {
                throw new Exception("User's Role not found");
            }
        }
    }
}
