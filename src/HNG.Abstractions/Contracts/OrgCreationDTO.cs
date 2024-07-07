using System.ComponentModel.DataAnnotations;

namespace HNG.Abstractions.Contracts
{
    public class OrgCreationDTO
    {
        [Required(ErrorMessage = "Organisation name required*")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Description required*")]
        public string Description { get; set; } = null!;
    }
}
