using OutOfOffice.DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace OutOfOffice.Models.Account
{
    public class RegisterViewModel
    {
        [Required]
        public string FullName { get; set; } = string.Empty;
        [Required] 
        public Subdivision SelectedSubdivision { get; set; }

        [Required]
        public DAL.Models.Position SelectedPosition { get; set; }
        [Required]
        public EmployeeType SelectedEmployeeType { get; set; }

        public bool IsActive { get; set; }

        [Required]
        [MinLength(6)]
        public string Email { get; set; } = string.Empty;
        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        public long? HRManagerId { get; set; }

        public IFormFile? Photo { get; set; }
    }

    public enum EmployeeType
    {
        Employee,
        ProjectManager,
        HR
    }
}
