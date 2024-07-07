using HNG.Abstractions.Contracts;
using HNG.Abstractions.Services.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace HNG.Identity
{
    public class PasswordGeneratorService : IPasswordGeneratorService
    {
        readonly IPasswordHasher<UserDTO> _passwordHasher;

        public PasswordGeneratorService(IPasswordHasher<UserDTO> passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        public Task<string> GenerateHashedPassword(string password, UserDTO user)
        {
            var hashedPassword = _passwordHasher.HashPassword(user, password);
            return Task.FromResult(hashedPassword);
        }

        public Task<bool> ValidatePassword(UserDTO user, string password, string hashPassword)
        {
            var result = _passwordHasher.VerifyHashedPassword(user, hashPassword, password);
            return Task.FromResult(result == PasswordVerificationResult.Success ? true : false);
        }
    }
}
