using HNG.Abstractions.Contracts;
using HNG.Abstractions.Models;
using HNG.Abstractions.Services.Infrastructure;
using HNG.Api.Client.Helpers.Authentication;
using HNG.Authentication.Mock;
using HNG.Tests.Common;
using HNG.Tests.Common.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

[assembly: Isolated]
namespace HNG.Business.Unit.Tests
{
    public class BaseTest
    {
#pragma warning disable NUnit1032 // An IDisposable field/property should be Disposed in a TearDown method
        public static ServiceProvider ServiceProvider;
#pragma warning restore NUnit1032 // An IDisposable field/property should be Disposed in a TearDown method

        protected string UserName { get; } = "johndoe";
        protected int UserId { get; } = 1;

        protected Guid ProfileId { get; set; } = new Guid("7acbba30-a989-4aa4-c702-08db3920bd4e");

        protected UserContextDTO UserContext { get; set; } = new UserContextDTO
        {
            ClientId = "web",
            UserId = "7acbba30-a989-4aa4-c702-08db3920bd4e",
            SessionId = "7acbba30-a989-4aa4-c702-08db3920bd4e",
            UserName = "johndoe"
        };

        protected string OrgId { get; set; } = "89E75A35-A8E0-4B17-B89A-E4E929B0929C";

        public UserContextDTO GetContext => Get();

        private UserContextDTO Get()
        {
            return new UserContextDTO
            {
                ClientId = "web",
                UserId = "7acbba30-a989-4aa4-c702-08db3920bd4e",
                UserName = "johndoe",
                SessionId = Guid.NewGuid().ToString()
            };
        }

        static BaseTest()
        {
            ServiceProvider = SetupDependencies();
        }

        public BaseTest()
        {
        }

        [SetUp]
        public void Init()
        {
            //some tests need affect static mock data need to be reset before each test
            ServiceProvider = SetupDependencies();
        }

        private static ServiceProvider SetupDependencies()
        {
            var appSettings = new AppSettings
            {
                Settings = new AppSettings.SettingsObject
                {
                    InTestMode = true,
                    UseMockForAuthentication = true,
                    UseMockForDatabase = true,
                    UseMockForIntegrations = true
                }
            };

            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection = HNG.Bootstrappers.Common.Bootstrapper.AddDependenciesToServiceCollection(serviceCollection, appSettings);

            //profile tests specific?
            serviceCollection.AddScoped<IAuthenticationService, MockUserAuthenticationService>();
            serviceCollection.AddScoped<IUserAuthenticationService, MockAuthenticationContext>();
            serviceCollection.AddScoped(typeof(ILogger<>), typeof(MockLogger<>));

            return serviceCollection.BuildServiceProvider();
        }

        public string GenerateId(string prefix)
        {
            string stamp = string.Format("{0}{1:yyMMddHHmmssfff}", prefix, DateTime.Now);
            return stamp;
        }

        public string ReadResource(string resourceName)
        {
            // Get the current assembly
            Assembly assembly = Assembly.GetExecutingAssembly();

            // Get all resource names in a case-insensitive manner
            var resourcePath = assembly.GetManifestResourceNames()
                                       .FirstOrDefault(name => string.Equals(name, $"{assembly.GetName().Name}.{resourceName}", StringComparison.OrdinalIgnoreCase));

            if (resourcePath == null)
            {
                throw new FileNotFoundException($"Resource {resourceName} not found.");
            }

            // Use the assembly to access the resource stream
            using Stream? stream = assembly.GetManifestResourceStream(resourcePath);
            if (stream == null)
            {
                throw new FileNotFoundException($"Resource {resourceName} not found.");
            }

            using StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}
