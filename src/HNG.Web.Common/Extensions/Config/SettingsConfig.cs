using HNG.Abstractions.Models;
using HNG.DatabaseConfigurationProvider;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension class that adds support for registering and getting app settings from database
    /// </summary>
    public static class SettingsConfig
    {
        /// <summary>
        /// Extension method that adds support for registering and getting app settings from database
        /// </summary>
        public static AppSettings GetConfiguredAppSettings(string[] args, WebApplicationBuilder builder)
        {
            var appId = builder.Configuration.GetSection("AppId").Value;
            var appName = builder.Configuration.GetSection("AppName").Value;

            var config = new DbConfigurationSource
            {
                ConnectionString = builder.Configuration.GetConnectionString("ConnectionString"),
                AppId = appId,
                AppName = appName,
                RefreshInterval = TimeSpan.FromMinutes(1)
            };

            builder.Configuration.AddAppSettingConfiguration(config);

            var appSettings = builder.Configuration.Get<AppSettings>() ?? new AppSettings();
            return appSettings;
        }
    }
}
