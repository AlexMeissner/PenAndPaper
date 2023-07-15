using Microsoft.EntityFrameworkCore;
using Serilog;
using Server.Database;
using Server.Middleware;

class ServerMain
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog(logger);

        string DbPath = @"W:\\Code\\PenAndPaper\\Database.db"; // TODO

        //builder.Services.AddDbContext<SQLDatabase>(options => options.UseSqlite($"Data Source={DbPath}").LogTo(Console.WriteLine, LogLevel.Warning, DbContextLoggerOptions.DefaultWithLocalTime | DbContextLoggerOptions.SingleLine));
        builder.Services.AddDbContext<SQLDatabase>(options => options.UseSqlite($"Data Source={DbPath}"));
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        // SWAGGER: Automatically open Browser -> Server > Properties > Debug > Open Debug launch profile UI > Launch browser
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<HttpLogger>();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}

//DungeonsAndDragons5e.DungeonsAndDragons5e rules = new();
//var a = rules.ToString();