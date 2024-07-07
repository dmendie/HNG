using HNG.Abstractions.Contracts;

namespace HNG.Abstractions.Services.Business
{
    public interface IUserService : IBusinessService
    {
        Task<T?> CreateUser<T>(UserCreationDTO createUser);
        Task<T> GetById<T>(string UserId);
        Task<T> GetUserOrganisations<T>(string UserId);
        Task<(int status, T?)> LoginUser<T>(UserLoginDTO userInfo);
        Task<(int status, T?)> Register<T>(UserCreationDTO userInfo);
        Task<T> Search<T>(string searchUserId, UserContextDTO user);
    }
}
