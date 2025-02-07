using Backend;

var builder = WebApplication.CreateBuilder(args);

builder.AddDatabase();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

app.MigrateDatabase();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();