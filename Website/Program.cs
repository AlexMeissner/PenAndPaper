using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.OAuth;
using Serilog;
using Serilog.Events;
using System.Security.Claims;
using System.Text.Json;
using Website.Components;
using Website.Services;

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
            .WriteToAspire(builder.Configuration)
            .CreateLogger();
    builder.Host.UseSerilog(Log.Logger);

    builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents();

    builder.Services.AddHttpContextAccessor();
    builder.Services.AddControllers();
    builder.Services.RegisterServicesFromAttributes();
    builder.AddServiceDefaults();

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
    })
        .AddCookie()
        .AddGoogle(configureOptions =>
        {
            configureOptions.ClientId = builder.Configuration["Google:ClientId"] ?? throw new Exception("ClientId not found");
            configureOptions.ClientSecret = builder.Configuration["Google:ClientSecret"] ?? throw new Exception("ClientSecret not found");
            configureOptions.SaveTokens = true;
            configureOptions.Scope.Add("openid");
            configureOptions.Scope.Add("profile");
            configureOptions.Scope.Add("email");
            configureOptions.Events = new OAuthEvents
            {
                OnCreatingTicket = context =>
                {
                    if (context.TokenResponse.Response is JsonDocument response)
                    {
                        var idToken = response.RootElement.GetProperty("id_token").ToString();
                        context.Identity?.AddClaim(new Claim("id_token", idToken));
                    }

                    return Task.CompletedTask;
                }
            };
        });
    builder.Services.AddAuthorization(configure => configure.FallbackPolicy = configure.DefaultPolicy);

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.MapStaticAssets();
    app.UseSerilogRequestLogging();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseAntiforgery();

    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();

    app.MapControllers();
    app.MapDefaultEndpoints();

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
