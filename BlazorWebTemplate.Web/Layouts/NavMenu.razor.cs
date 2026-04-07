using BlazorWebTemplate.Web.Services.AppInfo;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;

namespace BlazorWebTemplate.Web.Layouts;

public partial class NavMenu
{
    [Inject]
    private IOptions<AppBrandOptions> BrandOptionsAccessor { get; set; } = default!;

    protected AppBrandOptions Brand => BrandOptionsAccessor.Value;
}
