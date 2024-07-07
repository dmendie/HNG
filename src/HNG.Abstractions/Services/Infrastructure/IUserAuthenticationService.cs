using HNG.Abstractions.Contracts;

namespace HNG.Abstractions.Services.Infrastructure
{
    public interface IUserAuthenticationService
    {
        string ClientId { get; }
        string UserId { get; }
        string UserName { get; }
        string SessionId { get; }
        UserContextDTO User { get; }
    }
}
