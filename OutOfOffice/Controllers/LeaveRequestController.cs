using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OutOfOffice.DAL.Models;
using OutOfOffice.DAL.Repositories.Interfaces;
using OutOfOffice.Infrastructure;
using OutOfOffice.DAL.DTO;
using OutOfOffice.Models.LeaveRequest;
using OutOfOffice.Models.Account;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using OutOfOffice.StaticClasses;

namespace OutOfOffice.Controllers
{
    [Authorize(Roles = "Employee,Project Manager,HR Manager,Administrator")]
    public class LeaveRequestController : Controller
    {
        private readonly IRequestActionRepository requestActionRepository;
        private readonly IRequestGetRepository requestGetRepository;
        public LeaveRequestController(IRequestGetRepository requestGetRepository, IRequestActionRepository requestActionRepository)
        {
            this.requestGetRepository = requestGetRepository;
            this.requestActionRepository  = requestActionRepository;
        }

        public async Task<IActionResult> Index()
        {
            EmployeeBase emp = CurrentUserGetter.GetCurrentUser(HttpContext);

            List<LeaveRequest> leaveRequests = await requestGetRepository.GetLeaveRequestsByEmployeeId(emp.Id).ToListAsync();

            return View(leaveRequests);
        }

        [Authorize(Roles = "Employee,Administrator")]
        public IActionResult CreateLeaveRequest()
        {
            return View("LeaveRequestAction",new LeaveRequestActionViewModel
            {
                LeaveRequest = new LeaveRequest(),
            });
        }

        [HttpPost]
        [Authorize(Roles = "Employee,Administrator")]
        public async Task<IActionResult> CreateLeaveRequest([FromForm] LeaveRequest leaveRequest)
        {
            ModelState.Remove("leaveRequest.Employee");
            if (leaveRequest.EndDate<=leaveRequest.StartDate)
            {
                ModelState.AddModelError("Date", "End Date must be greater then Start Date");
            }
            if (ModelState.IsValid)
            {
                leaveRequest.EmployeeId = HttpContext.Session.GetJson<Employee>("CurrentUser")?.Id ?? throw new Exception("Session data lost.");
                await requestActionRepository.CreateLeaveRequestAsync(leaveRequest);

                return RedirectToAction("Index", "LeaveRequest");
            }

            return RedirectToAction(nameof(CreateLeaveRequest));
        }

        [Authorize(Roles = "HR Manager,Employee,Administrator")]
        public IActionResult UpdateLeaveRequest(LeaveRequest leaveRequest, long id)
        {
            leaveRequest.Id = id;
            ModelState.Remove("Employee");
            return View("LeaveRequestAction", new LeaveRequestActionViewModel
            {
                LeaveRequest = leaveRequest,
                Action = "Update"
            });
        }

        [HttpPost]
        [Authorize(Roles = "HR Manager,Employee,Administrator")]
        public async Task<IActionResult> UpdateLeaveRequest([FromForm] LeaveRequest leaveRequest, bool update)
        {
            ModelState.Remove("leaveRequest.Employee");
            if (leaveRequest.EndDate <= leaveRequest.StartDate)
            {
                ModelState.AddModelError("Date", "End Date must be greater then Start Date");
            }
            if (ModelState.IsValid)
            {
                await requestActionRepository.UpdateLeaveRequestAsync(leaveRequest);
                return RedirectToAction("Index", "LeaveRequest");
            }

            return RedirectToAction(nameof(UpdateLeaveRequest));
        }

        [Authorize(Roles = "HR Manager,Employee,Administrator")]
        public async Task<IActionResult> RejectLeaveRequest(long id)
        {
            await requestActionRepository.RejectLeaveRequestAsync(id);

            return RedirectToAction("Index", "LeaveRequest");
        }

        private EmployeeBase GetCurrentUser<T>() where T : EmployeeBase
        {
            return HttpContext.Session.GetJson<T>("CurrentUser") ?? throw new Exception("Session data lost");
        }
    }
}
