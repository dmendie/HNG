
using HNG.Abstractions.Contracts;

namespace HNG.Abstractions.Services.Infrastructure
{
    public interface IPasswordGeneratorService
    {
        Task<string> GenerateHashedPassword(string password, UserDTO user);
        Task<bool> ValidatePassword(UserDTO user, string password, string hashPassword);
    }
}
