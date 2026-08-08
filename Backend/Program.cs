using Microsoft.AspNetCore.HttpOverrides;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
    .WriteTo.Console()
    .Enrich.FromLogContext()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();
    builder.Host.UseSerilog(Log.Logger);

    builder.Services.AddControllers();

    var app = builder.Build();

    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders =
            ForwardedHeaders.XForwardedFor | // Update the remote ip address with the ip address of the external client instead of caddy's internal ip
            ForwardedHeaders.XForwardedProto // Match the original protocol (https) used by the user instead of server internal protocol (http)
    });

    if (app.Environment.IsProduction())
    {
        app.UseDefaultFiles();
        app.MapStaticAssets();
    }

    app.UseSerilogRequestLogging();
    app.UseAuthorization();
    app.MapControllers();

    if (app.Environment.IsProduction())
    {
        app.MapFallbackToFile("/index.html");
    }

    app.Run();
}
catch (Exception exception)
{
    Log.Fatal(exception, "An error occurred while starting the application.");
}
finally
{
    Log.CloseAndFlush();
}
