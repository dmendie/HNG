namespace HNG.Tests.Common
{
    public enum ApiUrlSourceType
    {
        Local,
        Dev,
        Test
    }

    public enum AppName
    {
        Client
    }

    public class TestAppSettings
    {
        public ApiUrlSourceType Source { get; set; }

        public List<SourceConfig> Configs { get; set; } = new List<SourceConfig>();

        public List<TestSetting> Settings { get; set; } = new List<TestSetting>();

        public SourceConfig GetConfig(AppName app)
        {
            return this.Configs.First(x => x.Source == this.Source && x.App == app);
        }

        public class SourceConfig
        {
            public ApiUrlSourceType Source { get; set; }
            public string ApiUrl { get; set; } = null!;
            public string? AppUrl { get; set; }
            public AppName App { get; set; }
            public bool UseAuthServer { get; set; }
            public string? AuthServer { get; set; }
            public string? ClientId { get; set; }
            public string? ClientSecret { get; set; }
            public string? Scopes { get; set; }
            public string? CompanyId { get; set; }
            public string? ProfileId { get; set; }
            public string? UserId { get; set; }
            public string? UserName { get; set; }

        }

        public class TestSetting
        {
            public string Key { get; set; } = null!;
            public string? Description { get; set; }
            public string? Local { get; set; }
            public string? Dev { get; set; }
            public string? Test { get; set; }
            public string? Demo { get; set; }
        }
    }
}
