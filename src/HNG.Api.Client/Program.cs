using HNG.Api.Client.ActionFilters;
using HNG.Api.Client.Extensions;
using IPinfo;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Converters;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var settings = SettingsConfig.GetConfiguredAppSettings(args, builder);

//configure for integration testing
if (settings.Settings.UseMockForAuthenticationOverride)
{
    settings.Settings.UseMockForAuthentication = settings.Settings.UseMockForAuthenticationOverride;
    settings.Settings.EnableDetailedErrorMessages = true;
}

builder.Services.AddSingleton<IPinfoClient>(new IPinfoClient.Builder().AccessToken(settings.Settings.IPInfoKey).Build());
builder.Services.AddScoped<IpInfoService>();

// Add services to the container.
builder.Services
    .AddDependencies(settings)
    .AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.Converters.Add(new StringEnumConverter());
        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
    });

builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});
builder.Host.UseSerilog();
builder.Services.AddSwaggerConfig(settings);
builder.Services.AddScoped<ValidationFilterAttribute>();
builder.Services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});
if (!settings.Settings.UseMockForAuthentication)
{
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = settings.Jwt.Issuer,
                ValidAudience = settings.Jwt.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Jwt.Key))
            };
        });
}

builder.Services.AddMemoryCache();

var app = builder.Build();

Serilog.Log.Logger = new Serilog.LoggerConfiguration()
    .ReadFrom.Configuration(app.Configuration)
    .CreateLogger();

try
{
    Log.Information("HNG client API Application starting up");

    if (settings.Settings.InTestMode)
    {
        app.UseDeveloperExceptionPage();
    }

    var forwardedHeadersOptions = new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.All,
    };
    forwardedHeadersOptions.KnownNetworks.Clear();
    forwardedHeadersOptions.KnownProxies.Clear();

    app.UseForwardedHeaders(forwardedHeadersOptions);
    app.UseSwaggerCustomConfig(settings);

    app.UseMiddleware<IpInfoMiddleware>();

    app.UseHttpsRedirection();

    app.UseSerilogRequestLogging();

    if (!settings.Settings.UseMockForAuthentication)
    {
        app.UseAuthentication();
    }
    app.UseAuthorization();

    app.MapControllers();

    CustomExceptionHandler.AppSettings = settings;
    app.UseExceptionHandler(CustomExceptionHandler.HandleException);

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "The application terminated unexpectedly");
    throw;
}
finally
{
    Log.CloseAndFlush();
}