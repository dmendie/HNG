using HNG.Abstractions.Contracts;
using HNG.Abstractions.Models;
using HNG.Abstractions.Services.Infrastructure;
using HNG.Web.Common.Helpers;

namespace HNG.Api.Client.Helpers.Authentication
{
    /// <summary>
    /// Claims Authentication Context - returns mock claim values for authenticated requests
    /// </summary>
    public class MockAuthenticationContext : IUserAuthenticationService
    {
        readonly AppSettings AppSettings;
        readonly IHttpContextAccessor ContextAccessor;
        string? _sessionId = null;

        /// <summary>
        /// ClaimsAuthenticationContext - constructor
        /// </summary>
        public MockAuthenticationContext(AppSettings settings, IHttpContextAccessor contextAccessor)
        {
            AppSettings = settings;
            ContextAccessor = contextAccessor;
        }

        /// <summary>
        /// Client Id
        /// </summary>
        public string ClientId => "web";

        /// <summary>
        /// User Id
        /// </summary>
        public string UserId
        {
            get
            {
                var request = ContextAccessor?.HttpContext?.Request;
                if (request != null)
                {
                    var headerValue = HttpHelpers.GetHeaderValue(request.Headers, "sub", false);
                    if (!String.IsNullOrWhiteSpace(headerValue))
                    {
                        return headerValue;
                    }
                }

                return AppSettings.Settings.MockUserId == null ? "7acbba30-a989-4aa4-c702-08db3920bd4e" : AppSettings.Settings.MockUserId;
            }
        }

        /// <summary>
        /// User Name
        /// </summary>
        public string UserName
        {
            get
            {
                var request = ContextAccessor?.HttpContext?.Request;
                if (request != null)
                {
                    var headerValue = HttpHelpers.GetHeaderValue(request.Headers, "preferred_username", false);
                    if (!String.IsNullOrWhiteSpace(headerValue))
                    {
                        return headerValue;
                    }
                }

                return AppSettings.Settings.MockUserName ?? "test@email.com";
            }
        }

        /// <summary>
        /// Session Id
        /// </summary>
        public string SessionId => _sessionId == null ? _sessionId = Guid.NewGuid().ToString() : _sessionId;

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
    }
}
