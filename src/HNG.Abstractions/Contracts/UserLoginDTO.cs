using System.ComponentModel.DataAnnotations;

namespace HNG.Abstractions.Contracts
{
    public class UserLoginDTO
    {
        [Required(ErrorMessage = "Email is required*")]
        [EmailAddress(ErrorMessage = "Provide a valid email address")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Password is required*")]
        public string Password { get; set; } = null!;
    }
}
