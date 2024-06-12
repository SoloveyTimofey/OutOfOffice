namespace OutOfOffice.DAL.Models
{
    public class Project
    {
        public long Id { get; set; }

        public ProjectType ProjectType { get; set; }

        public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public DateOnly? EndDate { get; set; } = DateOnly.FromDateTime(DateTime.Now.AddDays(15));

        public long ProjectManagerId { get; set; }
        public ProjectManager ProjectManager { get; set; } = null!;

        public string? Comment { get; set; }

        public bool IsActive { get; set; }

        public List<Employee> Employees { get; set; } = [];
        public List<EmployeeProjectJunction> EmployeeProjectJunctions { get; set; } = [];
    }

    public enum ProjectType
    {
        SoftwareDevelopment,
        SystemIntegration,
        DataScience
    }

}
