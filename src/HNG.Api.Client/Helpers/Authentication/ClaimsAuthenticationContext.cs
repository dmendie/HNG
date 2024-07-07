using HNG.Abstractions.Contracts;
using HNG.Abstractions.Models;
using HNG.Abstractions.Services.Infrastructure;
using HNG.Web.Common.Helpers;
using System.Security.Claims;

namespace HNG.Api.Client.Helpers.Authentication
{
    /// <summary>
    /// Claims Authentication Context - returns claim values for authenticated requests
    /// </summary>
    public class ClaimsAuthenticationContext : IUserAuthenticationService
    {
        readonly AppSettings AppSettings;
        readonly ClaimsPrincipal? claimsPrincipal;
        readonly IHttpContextAccessor ContextAccessor;

        const string ClientIdClaimType = "client_id";
        const string SessionIdClaimType = "sid";
        const string UserIdClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
        const string UserNameClaimType = "preferred_username";

        /// <summary>
        /// ClaimsAuthenticationContext - constructor
        /// </summary>
        public ClaimsAuthenticationContext(AppSettings settings, IHttpContextAccessor contextAccessor)
        {
            AppSettings = settings;
            ContextAccessor = contextAccessor;
            claimsPrincipal = contextAccessor?.HttpContext?.User as ClaimsPrincipal;
        }

        /// <summary>
        /// Client Id
        /// </summary>
        public string ClientId
        {
            get
            {
                return GetCredentialValue(ClientIdClaimType);
            }
        }

        /// <summary>
        /// User Id
        /// </summary>
        public string UserId
        {
            get
            {
                return GetCredentialValue(UserIdClaimType);
            }
        }

        /// <summary>
        /// User Name
        /// </summary>
        public string UserName
        {
            get
            {
                return GetCredentialValue(UserNameClaimType);
            }
        }

        /// <summary>
        /// Session Id
        /// </summary>
        public string SessionId
        {
            get
            {
                return GetCredentialValue(SessionIdClaimType);
            }
        }

        /// <summary>
        /// User
        /// </summary>
        public UserContextDTO User
        {
            get
            {
                return new UserContextDTO
                {
                    ClientId = ClientId,
                    UserName = UserName,
                    SessionId = SessionId,
                    UserId = UserId,
                };
            }
        }

        private string GetCredentialValue(string claim, bool throwExceptionIfNotFound = true)
        {
            if (AppSettings.Settings.InTestMode)
            {
                //IMPORTANT: integration in Admin API is only used in test environments

                string clientId = HttpHelpers.GetClaimValue(claimsPrincipal, "client_id", false);
                if (clientId.Equals("test", StringComparison.OrdinalIgnoreCase) || clientId.Equals("test", StringComparison.OrdinalIgnoreCase))
                {
                    var request = ContextAccessor?.HttpContext?.Request;
                    if (request != null)
                    {
                        switch (claim.ToLower())
                        {
                            case "client_id":
                                return clientId;
                            case "sid":
                                return HttpHelpers.GetClaimValue(claimsPrincipal, "jti", throwExceptionIfNotFound);
                            case UserIdClaimType:
                                return HttpHelpers.GetHeaderValue(request.Headers, "sub", throwExceptionIfNotFound);
                            default:
                                return HttpHelpers.GetHeaderValue(request.Headers, claim, throwExceptionIfNotFound);
                        }
                    }
                }
            }

            return HttpHelpers.GetClaimValue(claimsPrincipal, claim, throwExceptionIfNotFound);
        }
    }
}
