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

    builder.AddServiceDefaults();

    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .WriteToAspire(builder.Configuration)
        .CreateLogger();
    builder.Host.UseSerilog(Log.Logger);

    builder.AddDatabase();

    builder.Services.AddScopedServices();
    builder.Services.AddSingletonServices();
    builder.Services.AddRepositories();
    builder.Services.AddChannels();
    builder.Services.AddControllers();
    builder.Services.AddBackgroundServices();
    builder.Services.AddOpenApi();
    builder.Services.AddSignalR();
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddGoogle(builder.Configuration);
    builder.Services.AddAuthorization();

    var app = builder.Build();

    app.UseSerilogRequestLogging();
    app.MigrateDatabase();

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