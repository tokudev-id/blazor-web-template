using BlazorWebTemplate.Client;
using BlazorWebTemplate.Web;
using BlazorWebTemplate.Web.Common.Constants;
using BlazorWebTemplate.Web.Services;
using BlazorWebTemplate.Web.Services.Authentication;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration));

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

app.MapHealthChecks("/health");

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
