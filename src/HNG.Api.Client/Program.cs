using HNG.Api.Client.Extensions;
using IPinfo;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Converters;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var settings = SettingsConfig.GetConfiguredAppSettings(args, builder);

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
builder.Services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

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