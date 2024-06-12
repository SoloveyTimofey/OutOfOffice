using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OutOfOffice.Models.ApprovalRequest;
using OutOfOffice.DAL.Models;
using OutOfOffice.DAL.Repositories.Interfaces;
using OutOfOffice.StaticClasses;
using OutOfOffice.Services.Facade;
using OutOfOffice.Infrastructure;
using OutOfOffice.Models.Account;

namespace OutOfOffice.Controllers
{
    [Authorize(Roles = "Project Manager,HR Manager,Administrator")]
    public class ApprovalRequestController : Controller
    {
        private readonly IRequestGetRepository requestGetRepository;
        private readonly IUserGetRepository userGetRepository;
        private readonly IProjectGetRepository projectGetRepository;
        private readonly ICreateApprivalRequestFacade createComplexEntityFacade;
        public ApprovalRequestController(IRequestGetRepository requestGetRepository, IUserGetRepository userGetRepository, ICreateApprivalRequestFacade createComplexEntityFacade, IProjectGetRepository projectGetRepository)
        {
            this.requestGetRepository = requestGetRepository;
            this.userGetRepository = userGetRepository;
            this.createComplexEntityFacade = createComplexEntityFacade;
            this.projectGetRepository = projectGetRepository;
        }
        public async Task<IActionResult> Index()
        {
            List<LeaveRequest> leaveRequests;
            if (HttpContext.Session.GetJson<EmployeeType>("EmployeeType")==EmployeeType.HR)
            {
                EmployeeBase emp = CurrentUserGetter.GetCurrentUser(HttpContext);
                List<long> employeeIDs = await userGetRepository.GetHREmployees(emp.Id).Select(emp => emp.Id).ToListAsync();
                leaveRequests = await requestGetRepository.GetLeaveRequestsByEmployeeIDs(employeeIDs).ToListAsync();
            }
            else
            {
                EmployeeBase emp = CurrentUserGetter.GetCurrentUser(HttpContext);
                List<long> employeeIDs = await projectGetRepository.GetProjectManagerEmployeeIDs(emp.Id).ToListAsync();
                leaveRequests = await requestGetRepository.GetLeaveRequestsByEmployeeIDs(employeeIDs).ToListAsync();
            }

            return View(leaveRequests);
        }

        [Authorize(Roles = "HR Manager,Administrator")]
        public IActionResult CreateApprovalRequest(long leaveRequestId)
        {
            return View("ApprovalRequestAction", new ApprovalRequestActionViewModel
            {
                ApprovalRequest = new ApprovalRequest
                {
                    LeaveRequestId = leaveRequestId
                }
            });
        }

        [HttpPost]
        [Authorize(Roles ="HR Manager,Administrator")]
        public async Task<IActionResult> CreateApprovalRequest([FromForm]ApprovalRequest approvalRequest)
        {
            ModelState.Remove("approvalRequest.Approver");
            ModelState.Remove("approvalRequest.LeaveRequest");
            if (ModelState.IsValid)
            {
                approvalRequest.ApproverId = CurrentUserGetter.GetCurrentUser(HttpContext).Id;

                await createComplexEntityFacade.CreateApprovalRequest(approvalRequest);

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(CreateApprovalRequest));
        }
    }
}
