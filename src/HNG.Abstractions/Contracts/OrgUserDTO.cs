using System.ComponentModel.DataAnnotations;

namespace HNG.Abstractions.Contracts
{
    public class OrgUserDTO
    {
        [Required(ErrorMessage = "User id required*")]
        public string UserId { get; set; } = null!;
    }
}
