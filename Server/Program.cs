using Microsoft.EntityFrameworkCore;
using Serilog;
using Server.Middleware;
using Server.Models;
using Server.Services;
using Server.Services.BusinessLogic;

class ServerMain
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog(logger);

        string DbPath = @"W:\\Code\\PenAndPaper\\Database.db"; // TODO

        builder.Services.AddDbContext<SQLDatabase>(options => options.UseSqlite($"Data Source={DbPath}"));
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddSingleton<IUpdateNotifier, UpdateNotifier>();

        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        builder.Services.AddScoped<ICampaign, Campaign>();
        builder.Services.AddScoped<ISound, Sound>();

        var app = builder.Build();

        // SWAGGER: Automatically open Browser -> Server > Properties > Debug > Open Debug launch profile UI > Launch browser
        // https://localhost:7099/swagger/index.html
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<HttpLogger>();

        app.UseWebSockets();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
