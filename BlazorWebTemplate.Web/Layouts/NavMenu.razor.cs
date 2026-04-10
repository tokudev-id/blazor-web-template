using BlazorWebTemplate.Web.Services.AppInfo;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;

namespace BlazorWebTemplate.Web.Layouts;

public partial class NavMenu
{
    [Inject]
    private IOptions<AppBrandOptions> BrandOptionsAccessor { get; set; } = default!;

    protected AppBrandOptions Brand => BrandOptionsAccessor.Value;

    protected static string GetInitials(string? name)
    {
        if (string.IsNullOrWhiteSpace(name)) return "?";
        var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return parts.Length >= 2
            ? $"{parts[0][0]}{parts[^1][0]}".ToUpperInvariant()
            : name[0].ToString().ToUpperInvariant();
    }
}
