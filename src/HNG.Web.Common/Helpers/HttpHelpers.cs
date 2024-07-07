using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace HNG.Web.Common.Helpers
{
    public static class HttpHelpers
    {
        public static string GetClaimValue(ClaimsPrincipal? principal, string claim, bool throwExceptionIfNotFound)
        {
            var identity = principal?.Identity as ClaimsIdentity;

            if (identity != null)
            {
                return GetClaimValue(identity, claim, throwExceptionIfNotFound);
            }

            if (throwExceptionIfNotFound)
            {
                throw new Exception($"Invalid claim value for '{claim}' claim");
            }

            return string.Empty;
        }

        public static string GetClaimValue(ClaimsIdentity identity, string claim, bool throwExceptionIfNotFound)
        {
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
