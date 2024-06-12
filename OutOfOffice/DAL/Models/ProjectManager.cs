using System.ComponentModel.DataAnnotations.Schema;

namespace OutOfOffice.DAL.Models
{
    public class ProjectManager : EmployeeBase
    {
        public long HRManagerId { get; set; }
        public HRManager HRManager { get; set; } = null!;

        [NotMapped]
        public List<Project> Projects { get; set; } = [];
    }
}
