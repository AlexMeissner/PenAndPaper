var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsProduction())
{
    app.UseDefaultFiles();
    app.MapStaticAssets();
}

// Configure the HTTP request pipeline.

app.UseAuthorization();
app.MapControllers();

if (app.Environment.IsProduction())
{
    app.MapFallbackToFile("/index.html");
}

app.Run();
