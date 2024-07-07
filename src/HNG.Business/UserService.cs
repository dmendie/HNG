using HNG.Abstractions.Contracts;
using HNG.Abstractions.Exceptions;
using HNG.Abstractions.Services.Business;
using HNG.Abstractions.Services.Data;
using HNG.Abstractions.Services.Infrastructure;
using SimpleValidator;

namespace HNG.Business
{
    public class UserService : IUserService
    {
        readonly IAuthUserService SignInService;
        readonly IUserDataService UserDataService;
        readonly IMappingService MappingService;
        readonly IPasswordGeneratorService PasswordGeneratorService;

        public UserService(IUserDataService userDataService,
            IMappingService mappingService,
            IPasswordGeneratorService passwordGeneratorService,
            IAuthUserService signInService)
        {
            UserDataService = userDataService;
            MappingService = mappingService;
            PasswordGeneratorService = passwordGeneratorService;
            SignInService = signInService;
        }

        public async Task<T> GetById<T>(string userId)
        {
            var data = await UserDataService.GetById(null, userId);

            if (data is null)
            {
                throw new NotFoundException("Specified user not found");
            }

            var ret = MappingService.Map<UserDTO>(data);
            var val = MappingService.Map<T>(ret);
            return val;
        }

        public async Task<T> Search<T>(string searchUserId, UserContextDTO user)
        {
            var data = await UserDataService.SearchUser(null, searchUserId, user.UserId);
            var ret = MappingService.Map<UserDTO>(data);
            var val = MappingService.Map<T>(ret);
            return val;
        }

        public async Task<T?> CreateUser<T>(UserCreationDTO createUser)
        {
            //validate
            var validator = new Validator();
            validator
                .IsNotNullOrEmpty(nameof(createUser.Firstname), createUser.Firstname)
                .IsNotNullOrEmpty(nameof(createUser.Lastname), createUser.Lastname)
                .IsNotNullOrEmpty(nameof(createUser.Phone), createUser.Phone)
                .IsNotNullOrEmpty(nameof(createUser.Email), createUser.Email)
                .IsNotNullOrEmpty(nameof(createUser.Password), createUser.Password)
                .IsEmail(createUser.Email)
                .EnsureNoHtml(nameof(createUser.Firstname), createUser.Firstname)
                .EnsureNoHtml(nameof(createUser.Lastname), createUser.Lastname)
                .EnsureNoHtml(nameof(createUser.Phone), createUser.Phone)
                .EnsureNoHtml(nameof(createUser.Email), createUser.Email)
                .EnsureNoHtml(nameof(createUser.Password), createUser.Password)
                ;
            validator.ThrowValidationExceptionIfInvalid();

            var newUserId = await UserDataService.InsertUser(null, createUser.Firstname, createUser.Lastname, createUser.Phone, createUser.Email, createUser.Password, createUser.Firstname);
            var data = await UserDataService.GetById(null, newUserId);
            var user = MappingService.Map<UserDTO>(data!);

            var hashedPassword = await PasswordGeneratorService.GenerateHashedPassword(createUser.Password, user);
            await UserDataService.UpdatePassword(null, newUserId, hashedPassword);

            var retVal = MappingService.Map<T>(data!);
            return retVal;
        }

        public async Task<T> GetUserOrganisations<T>(string UserId)
        {
            var data = await UserDataService.GetOrganisations(null, UserId);
            var ret = MappingService.Map<IEnumerable<OrganisationDTO>>(data);
            var Vel = MappingService.Map<T>(new OrganisationListDTO { Organisations = ret });

            return Vel;
        }

        public async Task<(int status, T?)> Register<T>(UserCreationDTO userInfo)
        {
            var existingEmail = await UserDataService.GetByEmailAddress(null, userInfo.Email);
            if (existingEmail != null)
            {
                return (400, default(T));
            }

            var user = await CreateUser<UserDTO>(userInfo);
            var singInResult = await SignInService.CreateToken(user!);

            var retVal = MappingService.Map<T>(singInResult);
            return (200, retVal);
        }

        public async Task<(int status, T?)> LoginUser<T>(UserLoginDTO userInfo)
        {
            var data = await UserDataService.GetByEmailAddress(null, userInfo.Email);
            if (data == null)
            {
                return (400, default(T));
            }

            var user = MappingService.Map<UserDTO>(data);
            var IsValidPassword = await PasswordGeneratorService.ValidatePassword(user, userInfo.Password, data.Password);
            if (!IsValidPassword)
            {
                return (400, default(T));
            }

            var singInResult = await SignInService.CreateToken(user!);

            var retVal = MappingService.Map<T>(singInResult);
            return (200, retVal);
        }
    }
}
