using HNG.Abstractions.Contracts;
using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

namespace HNG.Tests.Common
{
    public class BaseIntegrationTestRequestBuilder
    {
        static readonly TestAppSettings settings = null!;
        static readonly JsonSerializerSettings serializerSettings;

        protected static readonly TestAppSettings.SourceConfig clientConfig;
        static readonly RestClientOptions customerRestClientOptions;

        protected static TokenResponse? clientToken = null;

        static RestClient client = null!;

        protected const string ClientIdClaimType = "client_id";
        protected const string ProfileIdClaimType = "sub";
        protected const string SessionIdClaimType = "sid";
        protected const string UserNameClaimType = "preferred_username";

        static BaseIntegrationTestRequestBuilder()
        {
            var root = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var configuration = root as IConfiguration;
            Assert.That(configuration != null);

            settings = new TestAppSettings();
            root.Bind(settings);

            clientConfig = settings.GetConfig(AppName.Client);

            customerRestClientOptions = new RestClientOptions
            {
                BaseUrl = new Uri(clientConfig.ApiUrl),
                ThrowOnAnyError = false,
            };

            serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DefaultValueHandling = DefaultValueHandling.Include,
                TypeNameHandling = TypeNameHandling.None,
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            };
        }

        public BaseIntegrationTestRequestBuilder()
        {
        }

        public TokenResponse? GetToken(TestAppSettings.SourceConfig config)
        {
            switch (config.App)
            {
                case AppName.Client:
                    return clientToken;
            }

            return null;
        }

        public void SetToken(TestAppSettings.SourceConfig config, TokenResponse? token)
        {
            switch (config.App)
            {
                case AppName.Client:
                    clientToken = token;
                    break;
            }
        }

        public async Task<RestClient> GetClient(TestAppSettings.SourceConfig config, RestClientOptions options)
        {
            ValidateRequiredConfigValues();

            if (config.UseAuthServer)
            {
                var token = GetToken(config);
                bool needToRequestToken = (token?.ExpiresIn ?? 0) < 60;

                if (needToRequestToken)
                {
                    token = await RequestTokenAsync(config);
                    SetToken(config, token);

                    if ((token == null) || (token.IsError))
                    {
                        throw new ArgumentNullException(nameof(token));
                    }
                    else
                    {
                        Console.WriteLine("Access Token:");
                        Console.WriteLine(token.AccessToken);
                        Console.WriteLine();
                    }
                }
            }

            var defaultHeaders = SetupDefaultHeaders();

            client = new RestClient(options, configureSerialization: s => s.UseNewtonsoftJson(serializerSettings));
            client.AddDefaultHeaders(defaultHeaders);

            return client;
        }

        public async Task<RestClient> GetCustomerClient()
        {
            return await GetClient(clientConfig, customerRestClientOptions);
        }

        protected virtual void ValidateRequiredConfigValues()
        {

        }

        protected virtual Dictionary<string, string> SetupDefaultHeaders()
        {
            return new Dictionary<string, string>()
                        {
                            {"Authorization", $"Bearer {clientToken?.AccessToken}" }
                        };
        }

        public T? ProcessResponse<T>(RestResponse? response)
        {
            var retVal = default(T);
            if (response != null)
            {
                if (response.IsSuccessful)
                {
                    if (response?.Content != null)
                    {
                        retVal = JsonConvert.DeserializeObject<T>(response.Content, serializerSettings);

                        Console.WriteLine();
                        Console.WriteLine("Response:");
                        Console.WriteLine("------------------");
                        Console.WriteLine(JsonConvert.SerializeObject(retVal, serializerSettings));
                        Console.WriteLine();


                    }
                }
                else
                {
                    if (!String.IsNullOrWhiteSpace(response?.Content))
                    {
                        Console.WriteLine("Response:");
                        Console.WriteLine("------------------");
                        Console.WriteLine(response.Content);
                        Console.WriteLine();
                    }

                    if (response?.ErrorMessage != null)
                    {
                        Console.WriteLine("Error:");
                        Console.WriteLine(response.ErrorMessage);
                        Console.WriteLine();
                        Console.WriteLine("Status:");
                        Console.WriteLine((int)response.StatusCode);
                        Console.WriteLine(response.StatusDescription);
                        Console.WriteLine();
                        Assert.Fail(response.ErrorMessage);
                    }

                    if (!String.IsNullOrWhiteSpace(response?.Content))
                    {
                        string responseContent = response.Content;
                        var error = JsonConvert.DeserializeObject<ValidationResponseDTO?>(responseContent, serializerSettings);
                        var ex = new ApiException(error);
                        throw ex;
                    }
                    else
                    {
                        throw new ApiException($"Response failed with no content - status {response?.StatusCode} - {response?.StatusDescription} ");
                    }
                }
            }
            else
            {
                throw new ApiException("Response is null");
            }

            return retVal;
        }

        public void ProcessResponse(RestResponse? response)
        {
            if (response != null)
            {
                if (response.IsSuccessful)
                {
                    if (response?.Content != null)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Response:");
                        Console.WriteLine("------------------");
                        Console.WriteLine(response.Content);
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("Response: None");
                    }
                }
                else
                {
                    if (response?.Content != null)
                    {
                        string responseContent = response.Content;
                        Console.WriteLine(response?.Content);
                        var error = JsonConvert.DeserializeObject<ValidationResponseDTO?>(responseContent, serializerSettings);
                        var ex = new ApiException(error);
                        throw ex;
                    }
                    else
                    {
                        throw new ApiException($"Response failed with no content - status {response?.StatusDescription} ");
                    }
                }
            }
            else
            {
                throw new ApiException("Response is null");
            }

        }

        public void LogRequestPayload<T>(RestRequest request, T data)
        {
            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, Formatting = Formatting.Indented };
            var json = JsonConvert.SerializeObject(data, settings);

            Console.WriteLine();
            Console.WriteLine($"Request Url: {request.Method.ToString().ToUpper()} {request.Resource}");
            Console.WriteLine($"Payload:");
            Console.WriteLine(json);
            Console.WriteLine();
        }

        public void LogRequestPayload(RestRequest request)
        {
            Console.WriteLine();
            Console.WriteLine($"Request Url: {request.Method.ToString().ToUpper()} {request.Resource}");
            if (request.Parameters.Count > 0)
            {
                Console.WriteLine($"Payload:");
                foreach (var parameter in request.Parameters)
                {
                    if (parameter.Type == ParameterType.RequestBody)
                    {
                        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(parameter.Value, serializerSettings));
                    }
                    else
                    {
                        Console.WriteLine($"{parameter.Name} = {parameter.Value}");
                    }
                }
            }
            Console.WriteLine();
        }

        public void LogRequestPayload(RestRequest request, string json)
        {
            Console.WriteLine();
            Console.WriteLine($"Request Url: {request.Method.ToString().ToUpper()} {request.Resource}");
            Console.WriteLine($"Payload:");
            Console.WriteLine(json);
            Console.WriteLine();
        }

        public async Task<TokenResponse> RequestTokenAsync(TestAppSettings.SourceConfig config)
        {
            if (String.IsNullOrWhiteSpace(config.AuthServer))
            {
                throw new Exception($"AuthServer is missing from Integration Test Config - {config.Source}");
            }

            if (String.IsNullOrWhiteSpace(config.ClientId))
            {
                throw new Exception($"Client Id is missing from Integration Test Config - {config.Source}");
            }

            if (String.IsNullOrWhiteSpace(config.ClientSecret))
            {
                throw new Exception($"Client Secret is missing from Integration Test Config - {config.Source}");
            }

            if (String.IsNullOrWhiteSpace(config.Scopes))
            {
                throw new Exception($"Scopes is missing from Integration Test Config - {config.Source}");
            }

            var client = new HttpClient();

            var disco = await client.GetDiscoveryDocumentAsync(config.AuthServer);
            if (disco.IsError)
            {
                throw new Exception(disco.Error);
            }

            var response = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = config.ClientId,
                ClientSecret = config.ClientSecret,

                Scope = config.Scopes,
            });

            if (response.IsError)
            {
                throw new Exception(response.Error);
            }

            return response;
        }
    }
}
