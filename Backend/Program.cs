using Backend.Extensions;
using Backend.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

AppDomain.CurrentDomain.UnhandledException += (_, e) =>
{
    try
    {
        Log.Fatal((Exception)e.ExceptionObject, "Unhandled exception");
    }
    catch (Exception exception)
    {
        Log.Fatal(exception, "Could not retrieve information from unhandled exception");
    }

    Log.CloseAndFlush();
};

try
{
    var builder = WebApplication.CreateBuilder(args);

    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();
    builder.Host.UseSerilog(Log.Logger);

    builder.AddDatabase();

    builder.Services.AddScopedServices();
    builder.Services.AddSingletonServices();
    builder.Services.AddTransient();
    builder.Services.AddChannels();
    builder.Services.AddControllers();
    builder.Services.AddBackgroundServices();
    builder.Services.AddOpenApi();
    builder.Services.AddSignalR();
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddCustomJwt(builder.Configuration);
    builder.Services.AddAuthorization();

    var app = builder.Build();

    app.UseSerilogRequestLogging(options =>
    {
        options.GetLevel = (httpContext, elapsed, ex) =>
        {
            var path = httpContext.Request.Path;

            // Do not log these endpoints
            if (path.StartsWithSegments("/campaigns") && path.Value is not null && path.Value.Contains("/mouse-indicators"))
            {
                return LogEventLevel.Debug;
            }

            // Log every other endpoint
            return LogEventLevel.Information;
        };
    });

    app.MigrateDatabase();
    app.LoadDungeonsAndDragonsRules();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.UseHttpsRedirection();
    }

    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.MapHub<CampaignUpdateHub>("campaign-updates/{campaignId}");

    app.Run();
}
catch (Exception exception)
{
    Log.Fatal(exception, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}