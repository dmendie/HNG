using HNG.Abstractions.Models;
using System.Data;

namespace HNG.Abstractions.Services.Data
{
    public interface IUserDataService : IDataService
    {
        Task<User?> GetByEmailAddress(IDbTransaction? transaction, string EmailAddress);
        Task<User?> GetById(IDbTransaction? transaction, string UserId);
        Task<IEnumerable<Organisation>> GetOrganisations(IDbTransaction? transaction, string UserId);
        Task<string> InsertUser(IDbTransaction? transaction, string FirstName, string LastName, string Phone, string EmailAddress, string Password, string CreatedBy);
        Task<User?> SearchUser(IDbTransaction? transaction, string SearchUserId, string UserId);
        Task UpdatePassword(IDbTransaction? transaction, string UserId, string Password);
    }
}
