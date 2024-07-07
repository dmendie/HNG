using HNG.Abstractions.Contracts;

namespace HNG.Abstractions.Services.Infrastructure
{
    public interface IAuthenticationService
    {
        UserContextDTO UserContext { get; }
    }
}
