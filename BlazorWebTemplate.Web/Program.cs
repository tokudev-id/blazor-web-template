using BlazorWebTemplate.Client;
using BlazorWebTemplate.Web;
using BlazorWebTemplate.Web.Common.Constants;
using BlazorWebTemplate.Web.Services;
using BlazorWebTemplate.Web.Services.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddClient(builder.Configuration);
builder.Services.AddBlazorWebTemplateWeb(builder.Configuration);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(CommonRouteFor.Error, createScopeForErrors: true);
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
