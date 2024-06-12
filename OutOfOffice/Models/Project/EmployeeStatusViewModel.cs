using OutOfOffice.DAL.DTO;

namespace OutOfOffice.Models.Project
{
    public class EmployeeStatusViewModel
    {
        public EmployeeDTO Employee { get; set; } = null!;
        public bool IsSelected { get; set; } 
    }
}
