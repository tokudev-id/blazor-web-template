using BlazorWebTemplate.Client;
using BlazorWebTemplate.Shared.Auth;
using BlazorWebTemplate.Web.Components;
using BlazorWebTemplate.Web.Common.Constants;
using BlazorWebTemplate.Web.Services;
using BlazorWebTemplate.Web.Services.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAuthentication(AuthConstants.CookieScheme)
    .AddCookie(AuthConstants.CookieScheme, options =>
    {
        options.Cookie.Name = "BlazorWebTemplate.Auth";
        options.LoginPath = AppRoutes.Login;
        options.AccessDeniedPath = AppRoutes.Login;
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

builder.Services.AddClient(builder.Configuration);
builder.Services.AddBlazorWebTemplateWeb(builder.Configuration);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(AppRoutes.Error, createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseBlazorWebTemplateWeb();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();
app.MapAuthenticationEndpoints();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
