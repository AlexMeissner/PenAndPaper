using Microsoft.AspNetCore.Authentication.JwtBearer;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Events;
using Server.Extensions;
using Server.Hubs;
using Server.Services;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

try
{
    var builder = WebApplication.CreateBuilder(args);

    Log.Logger = new LoggerConfiguration()
             .ReadFrom.Configuration(builder.Configuration)
             .WriteToAspire(builder.Configuration)
             .CreateLogger();
    builder.Host.UseSerilog(Log.Logger);

    builder.Services.AddOpenApi();
    builder.Services.AddDatabase();
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSingletonServices();
    builder.Services.AddScopedServices();
    builder.Services.AddSignalR();
    builder.AddServiceDefaults();

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddGoogle();
    builder.Services.AddAuthorization();

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference(); // https://localhost:7099/scalar/v1
        app.UseHttpsRedirection();
    }

    app.UseWebSockets();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.MapHub<CampaignUpdateHub>("CampaignUpdates");
    app.MigrateDatabase();
    app.UpdateRulesInDatabase();
    app.MapDefaultEndpoints();
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

static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
{
    try
    {
        Log.Fatal((Exception)e.ExceptionObject, "Unhandled excpetion");
    }
    catch (Exception exception)
    {
        Log.Fatal(exception, "Could not retrieve information from unhandled excpetion");
    }

    Log.CloseAndFlush();
}
