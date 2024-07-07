using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace HNG.Api.Client.Helpers.Authentication
{
    /// <summary>
    /// Attaches auth for Swagger operations
    /// </summary>
    public class AuthorizeOperationFilter : IOperationFilter
    {
        /// <summary>
        /// Attaches auth for Swagger operations
        /// </summary>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {

            var authorizeAttributes = context.MethodInfo.DeclaringType?.GetCustomAttributes(true)
                            .Union(context.MethodInfo.GetCustomAttributes(true))
                            .OfType<AuthorizeAttribute>();

            if ((authorizeAttributes ?? Enumerable.Empty<AuthorizeAttribute>()).Any())
            {
                operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
                operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

                var oAuthScheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "OAuth2" }
                };

                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        [ oAuthScheme ] = new[]{ "OAuth2" }
                    }
                };
            }
        }
    }
}
