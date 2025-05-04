using System.ComponentModel.DataAnnotations;

namespace Presentation.Models
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "First name is required")]
        [Display(Name = "First Name", Prompt = "Enter first name")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last name is required")]
        [Display(Name = "Last Name", Prompt = "Enter last name")]
        [DataType(DataType.Text)]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^\S+@\S+\.\S+$", ErrorMessage = "Please enter a valid email address")]
        [Display(Name = "Email", Prompt = "Enter email")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$",
            ErrorMessage = "Password must be at least 8 characters and include upper, lower case, and a number"
        )]
        [Display(Name = "Password", Prompt = "Enter password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Please confirm your password")]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
        [Display(Name = "Confirm Password", Prompt = "Confirm password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = null!;

        [Range(
            typeof(bool),
            "true",
            "true",
            ErrorMessage = "You must accept the Terms and Conditions"
        )]
        [Display(Name = "I accept Terms and Conditions")]
        public bool TermsAndConditions { get; set; }
    }
}
