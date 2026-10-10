using Backend.Authentication;
using Backend.Login;
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

    builder.Services.Configure<ReturnUrlOptions>(builder.Configuration.GetSection(ReturnUrlOptions.SectionName));

    builder.Services.AddGoogleAuthentication(builder.Configuration);

    builder.Services.Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    });

    var app = builder.Build();

    app.UseForwardedHeaders();

    app.UseSerilogRequestLogging();

    // In production Caddy terminates TLS and redirects HTTP to HTTPS.
    if (app.Environment.IsDevelopment())
    {
        app.UseHttpsRedirection();
    }

    app.UseCsrfProtection();

    app.UseDefaultFiles();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    if (app.Environment.IsProduction())
    {
        app.MapStaticAssets();
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
