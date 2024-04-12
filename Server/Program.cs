using Server.Middleware;
using Server.Services;

class ServerMain
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.CreateLogger();
        // builder.Configuration.AddJsonFile("appsettings.json"); // It seems like this is done automatically

        builder.Services.AddDatabase(builder.Configuration);
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSingletonServices();
        builder.Services.AddScopedServices();
        builder.Services.AddHttpsRedirection(options =>
        {
            options.RedirectStatusCode = 307;
            options.HttpsPort = 7099;
        });

        var app = builder.Build();

        // SWAGGER: Automatically open Browser -> Server > Properties > Debug > Open Debug launch profile UI > Launch browser
        // https://localhost:7099/swagger/index.html
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseMiddleware<HttpLogger>();
        app.UseWebSockets();
        app.UseAuthorization();
        app.MapControllers();
        app.MigrateDatabase();
        app.UpdateRulesInDatabase();
        app.Run();
    }
}
