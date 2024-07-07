using HNG.Abstractions.Contracts;
using HNG.Abstractions.Services.Business;
using Microsoft.AspNetCore.Mvc;

namespace HNG.Api.Client.Controllers
{
    /// <summary>
    /// Organisation Detail Controller
    /// </summary>
    [Tags("Stage 2 Task")]
    [Route("api/organisations/{orgId}")]
    public class OrganisationDetailController : BaseSecuredApiController
    {
        readonly IOrganisationService OrganisationService;

        /// <summary>
        /// constructor
        /// </summary>
        public OrganisationDetailController(IOrganisationService organisationService)
        {
            OrganisationService = organisationService;
        }

        /// <summary>
        /// Get organisation - gets an organisation detail by id
        /// </summary>
        [HttpGet]
        public async Task<ResponseDataDTO> GetOrganisationDetail(string orgId)
        {
            var response = await OrganisationService.GetById<ResponseDataDTO>(UserId, orgId);
            return response;
        }

    }
}
