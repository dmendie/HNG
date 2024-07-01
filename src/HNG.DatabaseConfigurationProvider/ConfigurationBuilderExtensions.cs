using Microsoft.Extensions.Configuration;

namespace HNG.DatabaseConfigurationProvider
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddAppSettingConfiguration(
            this IConfigurationBuilder builder, Action<DbConfigurationSource>? configurationSource)
        {
            return builder.Add(configurationSource);
        }

        public static IConfigurationBuilder AddAppSettingConfiguration(
            this IConfigurationBuilder builder, DbConfigurationSource configurationSource)
        {
            if (builder.Sources.Any())
            {
                //intentionally add as second to last source - so that ChainedConfigurationProvider is the last one
                builder.Sources.Insert(builder.Sources.Count - 1, configurationSource);
                return builder;
            }
            else
            {
                return builder.Add(configurationSource);
            }
        }
    }
}
