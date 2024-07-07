namespace HNG.Abstractions.Contracts
{
    public class UserContextDTO
    {
        public string? OrgId { get; set; }
        public string UserId { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string SessionId { get; set; } = null!;
        public string ClientId { get; set; } = null!;
    }
}
