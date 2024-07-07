using HNG.Abstractions.Contracts;
using HNG.Abstractions.Services.Business;
using HNG.Api.Client.ActionFilters;
using Microsoft.AspNetCore.Mvc;

namespace HNG.Api.Client.Controllers
{
    /// <summary>
    /// Organisation Controller
    /// </summary>
    [Tags("Stage 2 Task")]
    [Route("api/organisations")]
    public class OrganisationController : BaseSecuredApiController
    {
        readonly IOrganisationService OrganisationService;
        readonly IUserService UserService;

        /// <summary>
        /// constructor
        /// </summary>
        public OrganisationController(IOrganisationService organisationService,
            IUserService userService)
        {
            OrganisationService = organisationService;
            UserService = userService;

        }

        /// <summary>
        /// Get organisations - gets a list of all organisations a user belongs to
        /// </summary>
        [HttpGet]
        public async Task<ResponseDataDTO> GetUserOrganisations()
        {
            var response = await UserService.GetUserOrganisations<ResponseDataDTO>(UserId);
            return response;
        }

        /// <summary>
        /// Create organisation - creates a new organisation for the user
        /// </summary>
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ResponseDataDTO> CreateOrganisations([FromBody] OrgCreationDTO orgInfo)
        {
            var response = await OrganisationService.Create<ResponseDataDTO>(orgInfo, UserContext);
            response.Message = "Organisation created successfully";
            return response;
        }
    }
}
