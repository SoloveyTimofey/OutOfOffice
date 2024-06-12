using Microsoft.Build.ObjectModelRemoting;
using OutOfOffice.DAL.Models;
using OutOfOffice.DAL.DTO;

namespace OutOfOffice.Models.Project
{
    public class ProjectActionViewModel
    {
        public DAL.Models.Project Project { get; set; } = null!;
        public List<EmployeeStatusViewModel> Employees { get; set; } = new List<EmployeeStatusViewModel>();


        public string Action = "Create";
    }
}
