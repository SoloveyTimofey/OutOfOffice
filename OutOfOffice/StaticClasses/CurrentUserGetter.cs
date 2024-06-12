using OutOfOffice.DAL.Models;
using OutOfOffice.Infrastructure;
using OutOfOffice.Models.Account;

namespace OutOfOffice.StaticClasses
{
    public static class CurrentUserGetter
    {
        public static EmployeeBase GetCurrentUser(HttpContext httpContext) 
        {
            EmployeeType employeeType = httpContext.Session.GetJson<EmployeeType>("EmployeeType");

            EmployeeBase emp;
            switch (employeeType)
            {
                case EmployeeType.Employee:
                    emp = httpContext.Session.GetJson<Employee>("CurrentUser") ?? throw new Exception("Session data lost");
                    break;
                case EmployeeType.ProjectManager:
                    emp = httpContext.Session.GetJson<ProjectManager>("CurrentUser") ?? throw new Exception("Session data lost");
                    break;
                case EmployeeType.HR:
                    emp = httpContext.Session.GetJson<HRManager>("CurrentUser") ?? throw new Exception("Session data lost");
                    break;
                default:
                    throw new Exception("Unknown employee type");
            }
            return emp;
        }

        public static EmployeeBase? GetCurrentNullableUser(HttpContext httpContext)
        {
            EmployeeBase emp;
            try
            {
                EmployeeType employeeType = httpContext.Session.GetJson<EmployeeType>("EmployeeType");

                switch (employeeType)
                {
                    case EmployeeType.Employee:
                        emp = httpContext.Session.GetJson<Employee>("CurrentUser") ?? throw new Exception("Session data lost");
                        break;
                    case EmployeeType.ProjectManager:
                        emp = httpContext.Session.GetJson<ProjectManager>("CurrentUser") ?? throw new Exception("Session data lost");
                        break;
                    case EmployeeType.HR:
                        emp = httpContext.Session.GetJson<HRManager>("CurrentUser") ?? throw new Exception("Session data lost");
                        break;
                    default:
                        throw new Exception("Unknown employee type");
                }
            }
            catch (Exception)
            {
                return null;
            }
            return emp;
        }
    }
}
