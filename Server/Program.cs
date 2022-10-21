using Server.Services;
using Microsoft.EntityFrameworkCore;

class ServerMain
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        string DbPath = @"W:\\Code\\PenAndPaper\\Database.db"; // TODO

        builder.Services.AddDbContext<SQLDatabase>(options => options.UseSqlite($"Data Source={DbPath}"));
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        AddServices(builder.Services);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddTransient<IUserAuthentication, UserAuthentication>();
        services.AddTransient<ICampaignOverview, CampaignOverview>();
        services.AddTransient<ICampaignCreation, CampaignCreation>();
        services.AddTransient<IUser, User>();
    }
}

//DungeonsAndDragons5e.DungeonsAndDragons5e rules = new();
//var a = rules.ToString();