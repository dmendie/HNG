using System.ComponentModel.DataAnnotations;

namespace HNG.Abstractions.Contracts
{
    public class UserCreationDTO
    {
        [Required(ErrorMessage = "First name is required*")]
        public string Firstname { get; set; } = null!;
        [Required(ErrorMessage = "Last name is required*")]
        public string Lastname { get; set; } = null!;
        [Required(ErrorMessage = "Phone number is required*")]
        [Phone(ErrorMessage = "Provide a valid phone number")]
        public string Phone { get; set; } = null!;
        [Required(ErrorMessage = "Email is required*")]
        [EmailAddress(ErrorMessage = "Provide a valid email address")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Password is required*")]
        public string Password { get; set; } = null!;
    }
}
