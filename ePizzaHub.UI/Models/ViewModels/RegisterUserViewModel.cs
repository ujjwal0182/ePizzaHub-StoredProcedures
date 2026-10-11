using System.ComponentModel.DataAnnotations;

namespace ePizzaHub.UI.Models.ViewModels
{
    public class RegisterUserViewModel
    {
        [Required(ErrorMessage = "User can not empty.")]
        public string UserName { get; set; } = default!;

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; } = default!;

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [MaxLength(15, ErrorMessage = "Password cannot exceed 15 characters.")]
        public string Password { get; set; } = default!;

        [Required(ErrorMessage = "Confirm Password is required.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = default!;

        [Required(ErrorMessage = "Phone number is required.")]
        public string PhoneNumber { get; set; } = default!;
    }
}
