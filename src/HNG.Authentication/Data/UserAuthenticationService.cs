using HNG.Abstractions.Contracts;
using HNG.Abstractions.Models;
using HNG.Abstractions.Services.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace HNG.Authentication.Data
{
    public class UserAuthenticationService : IAuthenticationService
    {
        readonly AppSettings AppSettings;
        readonly IHttpContextAccessor ContextAccessor;
        readonly ILogger<UserAuthenticationService> Logger;

        ClaimsPrincipal? User;
        const string ClientIdClaimType = "client_id";
        const string ProfileIdClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
        const string SessionIdClaimType = "sid";
        const string UserNameClaimType = "preferred_username";

        /// <summary>
        /// ClaimsAuthenticationContext - constructor
        /// </summary>
        public UserAuthenticationService(AppSettings settings, IHttpContextAccessor contextAccessor, ILogger<UserAuthenticationService> logger)
        {
            AppSettings = settings;
            ContextAccessor = contextAccessor;
            User = contextAccessor?.HttpContext?.User as ClaimsPrincipal;
            Logger = logger;
        }

        public UserContextDTO UserContext => GetUserContext();

        private UserContextDTO GetUserContext()
        {
            return new UserContextDTO
            {
                ClientId = GetCredentialValue(ClientIdClaimType),
                UserId = GetCredentialValue(ProfileIdClaimType),
                UserName = GetCredentialValue(UserNameClaimType),
                SessionId = GetCredentialValue(SessionIdClaimType)
            };
        }

        private string GetClaimValue(ClaimsPrincipal? principal, string claim, bool throwExceptionIfNotFound = true)
        {

            var identity = principal?.Identity as ClaimsIdentity;
            string? claimValue = null;

            if (identity != null)
            {
                claimValue = identity.Claims
                    .FirstOrDefault(c =>
                        c.Type.Equals(claim, StringComparison.OrdinalIgnoreCase)
                    )?.Value;
            }

            if (throwExceptionIfNotFound && claimValue == null)
            {
                throw new Exception($"Invalid claim value for '{claim}' claim");
            }

            return claimValue ?? String.Empty;
        }

        private string GetCredentialValue(string claim, bool throwExceptionIfNotFound = true)
        {
            if (AppSettings.Settings.InTestMode)
            {
                //IMPORTANT: test in Customer API is only used in test environments

                string clientId = GetClaimValue(User, "client_id", false);
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
                                return GetClaimValue(User, "jti", throwExceptionIfNotFound);
                            case ProfileIdClaimType:
                                return GetHeaderValue(request.Headers, "sub", throwExceptionIfNotFound);
                            default:
                                return GetHeaderValue(request.Headers, claim, throwExceptionIfNotFound);
                        }
                    }
                }
            }
            return GetClaimValue(User, claim, throwExceptionIfNotFound);
        }

        public static string GetHeaderValue(IHeaderDictionary Headers, string headerKey, bool throwExceptionIfNotFound)
        {
            var headerExists = Headers.TryGetValue(headerKey, out var headerValue);
            var stringHeaderValue = $"{headerValue}";
            if (throwExceptionIfNotFound && String.IsNullOrWhiteSpace(stringHeaderValue))
            {
                throw new Exception($"Invalid header value for '{headerKey}' header");
            }

            return headerExists ? stringHeaderValue : string.Empty;
        }
    }
}
