namespace OutOfOffice.DAL.Models
{
    public class Employee : EmployeeBase
    {
        public long HRManagerId { get; set; }
        public HRManager HRManager { get; set; } = null!;

        public List<Project> Projects { get; set; } = [];
        public List<EmployeeProjectJunction> EmployeeProjectJunctions { get; set; } = [];
    }
}