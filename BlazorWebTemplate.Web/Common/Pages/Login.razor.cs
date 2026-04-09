using BlazorWebTemplate.Web.Common.Constants;
using BlazorWebTemplate.Web.Services.AppInfo;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;

namespace BlazorWebTemplate.Web.Common.Pages;

public partial class Login
{
    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private IOptions<AppBrandOptions> BrandOptionsAccessor { get; set; } = default!;

    [CascadingParameter]
    private HttpContext? HttpContext { get; set; }

    [SupplyParameterFromQuery(Name = "returnUrl")]
    public string? ReturnUrl { get; set; }

    [SupplyParameterFromQuery(Name = "error")]
    public string? Error { get; set; }

    protected string? _errorMessage;
    protected string _email = string.Empty;
    protected string _password = string.Empty;
    protected AppBrandOptions Brand => BrandOptionsAccessor.Value;

    protected override void OnInitialized()
    {
        if (HttpContext?.User.Identity?.IsAuthenticated == true)
        {
            NavigationManager.NavigateTo(CommonRouteFor.Dashboard, forceLoad: true);
        }
    }

    protected override void OnParametersSet()
    {
        _errorMessage = Error;
    }
}
