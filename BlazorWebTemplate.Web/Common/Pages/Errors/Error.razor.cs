using System.Diagnostics;
using Microsoft.AspNetCore.Components;

namespace BlazorWebTemplate.Web.Common.Pages.Errors;

public partial class Error
{
    [CascadingParameter]
    private HttpContext? HttpContext { get; set; }

    protected string? RequestId { get; private set; }
    protected bool ShowRequestId => !string.IsNullOrWhiteSpace(RequestId);

    protected override void OnInitialized()
    {
        RequestId = Activity.Current?.Id ?? HttpContext?.TraceIdentifier;
    }
}
