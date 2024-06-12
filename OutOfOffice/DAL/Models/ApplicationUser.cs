using Microsoft.AspNetCore.Identity;

namespace OutOfOffice.DAL.Models
{
    public class ApplicationUser : IdentityUser
    {
        public long EmployeeId { get; set; }
        public EmployeeBase Employee { get; set; } = null!;
    }
}
