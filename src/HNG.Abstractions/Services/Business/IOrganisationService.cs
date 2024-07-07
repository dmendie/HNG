using HNG.Abstractions.Contracts;

namespace HNG.Abstractions.Services.Business
{
    public interface IOrganisationService : IBusinessService
    {
        Task<ResponseDataDTO> AddUserToOrg(string orgId, OrgUserDTO userInfo, UserContextDTO user);
        Task<T> Create<T>(OrgCreationDTO orgInfo, UserContextDTO user);
        Task<T> GetById<T>(string UserId, string OrgId);
    }
}
