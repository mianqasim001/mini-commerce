using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
    public class RegisterDto 
    {
        [Required(ErrorMessage = "Name is required")] 
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")] 
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")] 
        public string Password { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;
    }

    public class LoginDto 
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")] 
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")] 
        public string Password { get; set; } = string.Empty;
    }

    public class UserUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
     public class deleteDto
     {
        [Required]
        public int userId { get; set; } 
     }
}