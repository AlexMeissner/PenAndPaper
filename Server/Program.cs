using Server.Middleware;
using Server.Services;

class ServerMain
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.CreateLogger();

        builder.Services.AddDatabase();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSingletonServices();
        builder.Services.AddScropedServices();

        var app = builder.Build();

        // SWAGGER: Automatically open Browser -> Server > Properties > Debug > Open Debug launch profile UI > Launch browser
        // https://localhost:7099/swagger/index.html
#if DEBUG
        app.UseSwagger();
        app.UseSwaggerUI();
#endif

        app.UseMiddleware<HttpLogger>();
        app.UseWebSockets();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.MigrateDatabase();
        app.Run();
    }
}
