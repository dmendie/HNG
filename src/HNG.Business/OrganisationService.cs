using HNG.Abstractions.Contracts;
using HNG.Abstractions.Helpers;
using HNG.Abstractions.Services.Business;
using HNG.Abstractions.Services.Data;
using HNG.Abstractions.Services.Infrastructure;
using SimpleValidator;

namespace HNG.Business
{
    public class OrganisationService : IOrganisationService
    {
        readonly IOrganisationDataService OrganisationDataService;
        readonly IUserDataService UserDataService;
        readonly IMappingService MappingService;

        public OrganisationService(IOrganisationDataService organisationDataService,
            IMappingService mappingService,
            IUserDataService userDataService)
        {
            OrganisationDataService = organisationDataService;
            MappingService = mappingService;
            UserDataService = userDataService;
        }

        public async Task<T> GetById<T>(string UserId, string OrgId)
        {
            var data = await OrganisationDataService.GetById(null, UserId, OrgId);
            if (data is null)
            {
                throw new AccessViolationException("You are not a member of the specified Organisation");
            }

            var ret = MappingService.Map<OrganisationDTO>(data);
            var val = MappingService.Map<T>(ret);
            return val;
        }

        public async Task<T> Create<T>(OrgCreationDTO orgInfo, UserContextDTO user)
        {
            //validate
            var validator = new Validator();
            validator
                .IsNotNullOrEmpty(nameof(orgInfo.Name), orgInfo.Name)
                .IsNotNullOrEmpty(nameof(orgInfo.Description), orgInfo.Description)
                .EnsureNoHtml(nameof(orgInfo.Name), orgInfo.Name)
                .EnsureNoHtml(nameof(orgInfo.Description), orgInfo.Description);
            validator.ThrowValidationExceptionIfInvalid();

            var newOrgId = await OrganisationDataService.Insert(null, user.UserId, orgInfo.Name, orgInfo.Description, user.UserName);

            var data = await OrganisationDataService.GetById(null, user.UserId, newOrgId);
            var ret = MappingService.Map<OrganisationDTO>(data!);
            var val = MappingService.Map<T>(ret);
            return val;
        }

        public async Task<ResponseDataDTO> AddUserToOrg(string orgId, OrgUserDTO userInfo, UserContextDTO user)
        {
            //validate
            var validator = new Validator();
            validator
                .IsNotNullOrEmpty(nameof(userInfo.UserId), userInfo.UserId, "A valid user identifier is required*");
            validator.ThrowValidationExceptionIfInvalid();

            //check if request user is part of the specified organisation
            var getOrg = await OrganisationDataService.GetById(null, user.UserId, orgId);
            if (getOrg == null)
            {
                throw new AccessViolationException("You are not a member of the specified Organisation");
            }

            //check if user is a valid user
            var getUser = await UserDataService.GetById(null, userInfo.UserId);
            userInfo.UserId = getUser?.UserId ?? string.Empty;
            validator.IsNotNullOrEmpty(nameof(userInfo.UserId), userInfo.UserId, "No user with the specified identifier was found*");
            validator.IsNotNullOrEmpty(nameof(userInfo.UserId), userInfo.UserId, "specified identifier was found*");
            validator.ThrowValidationExceptionIfInvalid();

            await OrganisationDataService.AddUser(null, user.UserId, orgId, user.UserName);

            return new ResponseDataDTO { Status = Abstractions.Enums.ResponseStatusType.Success.GetDescription(), Message = "User added to organisation successfully" };
        }
    }
}
