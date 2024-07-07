namespace HNG.Abstractions.Contracts
{
    public class ResponseDataDTO
    {
        public string Status { get; set; } = null!;
        public string Message { get; set; } = null!;
        public object? Data { get; set; }
    }
}
