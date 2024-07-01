using HNG.Abstractions.Models;
using HNG.Web.Common.Extensions.ServiceLogs;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension class that adds support for registering dependencies to IOC container
    /// </summary>
    public static class ServiceRegistrationExtensions
    {
        /// <summary>
        /// Default Cors Policy
        /// </summary>
        public const string DefaultCorsPolicy = "DefaultCorsPolicy";

        /// <summary>
        /// Extension method that adds support for registering dependencies to IOC container
        /// </summary>
        public static IServiceCollection AddDependencies(this IServiceCollection services, AppSettings appSettings)
        {
            HNG.Bootstrappers.Common.Bootstrapper.AddDependencies(services, appSettings);

            //add http context
            services.AddHttpContextAccessor();

            //add service log
            services.AddScoped<ClientServiceLogAttribute>();

            return services;
        }

        /// <summary>
        /// Extension method that adds Cors support
        /// </summary>
        public static void ConfigureCors(this IServiceCollection services, List<string> allowedCorsOrigins) => services.AddCors(options =>
        {
            options.AddPolicy(DefaultCorsPolicy, policy =>
            {
                policy
                    .WithOrigins(allowedCorsOrigins.ToArray())
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
    }
}
