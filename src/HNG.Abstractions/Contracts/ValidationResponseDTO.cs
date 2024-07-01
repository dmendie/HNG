using SimpleValidator.Results;

namespace HNG.Abstractions.Contracts
{
    public class ValidationResponseDTO
    {
        public string Message { get; set; } = null!;
        public List<ValidationError> Details { get; set; } = new List<ValidationError>();
    }
}
