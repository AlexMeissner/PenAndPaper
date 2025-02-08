using Backend;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
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

    builder.Services.AddControllers();
    builder.Services.AddOpenApi();
    builder.Services.AddSignalR();

    var app = builder.Build();

    app.UseSerilogRequestLogging();
    app.MigrateDatabase();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

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