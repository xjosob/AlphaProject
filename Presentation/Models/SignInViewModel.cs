using System.ComponentModel.DataAnnotations;

namespace Presentation.Models
{
    public class SignInViewModel
    {
        [Required(ErrorMessage = "Please enter your email")]
        [Display(Name = "Email", Prompt = "Enter email")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Please enter your password")]
        [Display(Name = "Password", Prompt = "Enter password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        public bool IsPersistent { get; set; }
    }
}
