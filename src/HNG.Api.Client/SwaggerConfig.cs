using HNG.Abstractions.Models;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension class that adds support for configuring Swagger Generation
    /// </summary>
    public static class SwaggerConfigExtensions
    {
        /// <summary>
        /// Extension method that adds support for registering dependencies to IOC container
        /// </summary>
        public static IServiceCollection AddSwaggerConfig(this IServiceCollection services, AppSettings appSettings)
        {
            var settings = appSettings.Swagger;
            if (settings.Enabled)
            {
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                services.AddEndpointsApiExplorer();
                services.AddSwaggerGen(options =>
                {
                    options.EnableAnnotations();
                    if (!appSettings.Settings.UseMockForAuthentication)
                    {
                        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                        {
                            Name = "Authorization",
                            Type = SecuritySchemeType.ApiKey,
                            Scheme = "Bearer",
                            BearerFormat = "JWT",
                            In = ParameterLocation.Header,
                            Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer D0C3A122-9F4F-4938-8B2E-94A743939474\""
                        });
                        options.AddSecurityRequirement(new OpenApiSecurityRequirement
                        {
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                    }
                                },
                                Array.Empty<string>()
                            },
                        });
                    }

                    foreach (var doc in settings.Documents)
                    {
                        options.SwaggerDoc(doc.Version, new OpenApiInfo
                        {
                            Version = doc.Version,
                            Title = doc.Title,
                            Description = doc.Description,
                        });
                    }

                    //get metadata from xml comments
                    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

                });
                services.AddSwaggerGenNewtonsoftSupport();
            }

            return services;
        }

        /// <summary>
        /// Extension method that adds support/options for using Swagger UI
        /// </summary>
        public static WebApplication UseSwaggerCustomConfig(this WebApplication app, AppSettings appSettings)
        {
            if (appSettings.Swagger.Enabled)
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            return app;
        }
    }
}
