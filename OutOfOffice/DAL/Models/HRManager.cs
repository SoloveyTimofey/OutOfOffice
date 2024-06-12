namespace OutOfOffice.DAL.Models
{
    public class HRManager : EmployeeBase
    {
        public List<Employee> Employees { get; set; } = [];
        public List<ProjectManager> ProjectManagers { get; set; } = [];

        public override string ToString()
        {
            return FullName;
        }
    }
}
