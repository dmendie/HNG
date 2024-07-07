using HNG.Abstractions.Contracts;
using HNG.Abstractions.Services.Infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace HNG.Api.Client.Controllers
{
    /// <summary>
    /// Base Secured API Controller  - enforces all actions on the controller require authentication
    /// </summary>
#if !DEBUG
    [Authorize]
#endif
    public class BaseSecuredApiController : BaseApiController
    {
        IUserAuthenticationService authenticationContext = null!;

        private IUserAuthenticationService AuthenticationContext
        {
            get
            {
                if (authenticationContext == null)
                {
                    authenticationContext = HttpContext.RequestServices.GetRequiredService<IUserAuthenticationService>();
                }
                return authenticationContext;
            }
        }

        /// <summary>
        /// UserName of currently logged in user - retrieved from Claims
        /// </summary>
        public string UserName => AuthenticationContext.UserName;

        /// <summary>
        /// User Id of currently logged in user - retrieved from Claims
        /// </summary>
        public string UserId => AuthenticationContext.UserId;

        /// <summary>
        /// User context of currently logged in user 
        /// </summary>
        public UserContextDTO UserContext => AuthenticationContext.User;
    }
}
