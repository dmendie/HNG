using Microsoft.Extensions.Configuration;

namespace HNG.DatabaseConfigurationProvider
{
    public class DbConfigurationSource : IConfigurationSource
    {
        public string? ConnectionString { get; set; }
        public string? AppId { get; set; }
        public string? AppName { get; set; }
        public TimeSpan? RefreshInterval { get; set; }

        public IConfigurationProvider Build(IConfigurationBuilder builder) =>
            new DbConfigurationProvider(this);
    }
}
