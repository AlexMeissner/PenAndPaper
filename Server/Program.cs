using Serilog;
using Serilog.Events;
using Server.Services;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();

    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

    builder.Services.AddDatabase(builder.Configuration);
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddSingletonServices();
    builder.Services.AddScopedServices();

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    // SWAGGER: Automatically open Browser -> Server > Properties > Debug > Open Debug launch profile UI > Launch browser
    // https://localhost:7099/swagger/index.html
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseHttpsRedirection();
    }

    app.UseWebSockets();
    app.UseAuthorization();
    app.MapControllers();
    app.MigrateDatabase();
    app.UpdateRulesInDatabase();
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