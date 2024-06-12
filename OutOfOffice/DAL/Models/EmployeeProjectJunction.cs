namespace OutOfOffice.DAL.Models
{
    public class EmployeeProjectJunction
    {
        public long Id { get; set; }    
        
        public long EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;

        public long ProjectId { get; set; }
        public Project Project { get; set; } = null!;
    }
}
