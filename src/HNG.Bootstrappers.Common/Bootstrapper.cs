using HNG.Abstractions.Contracts;
using HNG.Abstractions.Models;
using HNG.Abstractions.Services.Business;
using HNG.Abstractions.Services.Data;
using HNG.Abstractions.Services.Infrastructure;
using HNG.Business;
using HNG.Data.Mock;
using HNG.Data.Sql;
using HNG.Identity;
using HNG.Mappers;
using HNG.ServiceLogProvider;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace HNG.Bootstrappers.Common
{
    public static class Bootstrapper
    {
        /// <summary>
        /// Extension method that adds support for registering dependencies to IOC container
        /// </summary>
        public static IServiceCollection AddDependencies(this IServiceCollection services, AppSettings appSettings)
        {
            return AddDependenciesToServiceCollection(services, appSettings);
        }

        /// <summary>
        /// Extension method that adds support for registering dependencies to IOC container
        /// </summary>
        public static IServiceCollection AddDependenciesToServiceCollection(IServiceCollection services, AppSettings appSettings)
        {
            //add appsettings
            services.AddSingleton(appSettings);

            //add mapper
            services.AddScoped<IMappingService, MappingService>();

            //data layer services
            if (appSettings.Settings.UseMockForDatabase)
            {
                services.AddScoped<IOrganisationDataService, MockOrganisationDataService>();
                services.AddScoped<IServiceLogDataService, MockServiceLogDataService>();
                services.AddScoped<IUserDataService, MockUserDataService>();
            }
            else
            {
                services.AddScoped<IOrganisationDataService, DBOrganisationDataService>();
                services.AddScoped<IServiceLogDataService, DBServiceLogDataService>();
                services.AddScoped<IUserDataService, DBUserDataService>();
            }

            //business logic services
            services.AddScoped<IAuthUserService, AuthUserService>();
            services.AddScoped<IHelloService, HelloService>();
            services.AddScoped<IOrganisationService, OrganisationService>();
            services.AddScoped<IPasswordHasher<UserDTO>, PasswordHasher<UserDTO>>();
            services.AddScoped<IPasswordGeneratorService, PasswordGeneratorService>();
            services.AddScoped<IServiceLogService, ServiceLogService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
