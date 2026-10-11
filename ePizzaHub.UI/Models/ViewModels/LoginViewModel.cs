using System.ComponentModel.DataAnnotations;

namespace ePizzaHub.UI.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage ="Please Enter a valid Email Address")]
        public string EmailAddress { get; set; } = default!;
        
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        [MaxLength(15, ErrorMessage = "Password cannot exceed 12 characters.")]
        public string Password { get; set; } = default!;
    }
}
