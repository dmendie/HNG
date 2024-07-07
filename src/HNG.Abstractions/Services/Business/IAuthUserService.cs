using HNG.Abstractions.Contracts;

namespace HNG.Abstractions.Services.Business
{
    public interface IAuthUserService : IBusinessService
    {
        Task<UserTokenDTO> CreateToken(UserDTO user, string? token = null);
    }
}
