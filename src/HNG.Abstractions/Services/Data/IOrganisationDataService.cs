using HNG.Abstractions.Models;
using System.Data;

namespace HNG.Abstractions.Services.Data
{
    public interface IOrganisationDataService : IDataService
    {
        Task AddUser(IDbTransaction? transaction, string UserId, string OrgId, string CreatedBy);
        Task<Organisation?> GetById(IDbTransaction? transaction, string UserId, string OrgId);
        Task<Organisation?> GetById(IDbTransaction? transaction, string OrgId);
        Task<string> Insert(IDbTransaction? transaction, string UserId, string Name, string Description, string CreatedBy);
    }
}
