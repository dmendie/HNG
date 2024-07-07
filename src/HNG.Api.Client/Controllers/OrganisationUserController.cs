using HNG.Abstractions.Contracts;
using HNG.Abstractions.Services.Business;
using HNG.Api.Client.ActionFilters;
using Microsoft.AspNetCore.Mvc;

namespace HNG.Api.Client.Controllers
{
    /// <summary>
    /// Organisation user Controller
    /// </summary>
    [Tags("Stage 2 Task")]
    [Route("api/organisations/{orgId}/users")]
    public class OrganisationUserController : BaseSecuredApiController
    {
        readonly IOrganisationService OrganisationService;

        /// <summary>
        /// constructor
        /// </summary>
        public OrganisationUserController(IOrganisationService organisationService)
        {
            OrganisationService = organisationService;
        }

        /// <summary>
        /// Organisation user - adds a user to a particular organisation
        /// </summary>
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ResponseDataDTO> AddOrganisationUser([FromBody] OrgUserDTO userInfo, string orgId)
        {
            var response = await OrganisationService.AddUserToOrg(orgId, userInfo, UserContext);
            return response;
        }

    }
}
