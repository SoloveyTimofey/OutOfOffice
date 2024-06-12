using System.ComponentModel.DataAnnotations;

namespace OutOfOffice.Models.Account
{
    public class SignInViewModel
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
