using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using System.Security.Claims;
using Website.Components;
using Website.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpContextAccessor();
builder.Services.RegisterServicesFromAttributes();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
    .AddCookie()
    .AddGoogle(configureOptions =>
    {
        configureOptions.SaveTokens = true;
        configureOptions.ClaimActions.MapJsonKey("urn:google:jwt", "id_token");
        configureOptions.Scope.Add("openid");
        configureOptions.Scope.Add("profile");
        configureOptions.Scope.Add("email");
        configureOptions.Events = new Microsoft.AspNetCore.Authentication.OAuth.OAuthEvents
        {
            OnCreatingTicket = context =>
            {
                var idToken = context.TokenResponse.Response.RootElement.GetProperty("id_token").ToString();
                context.Identity.AddClaim(new Claim("id_token", idToken));
                return Task.CompletedTask;
            }
        };
    });
    //.AddJwtBearer(); ???
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
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();


// https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/google-logins?view=aspnetcore-8.0
// https://console.cloud.google.com/apis/credentials/consent?project=penandpaper-434109&supportedpurview=project
// https://www.youtube.com/watch?v=r3tytnzCuNw
