using BlazorWebTemplate.Shared.AuditLogs.Queries.GetAuditLogs;
using BlazorWebTemplate.Client.Services.BackEnd.AuditLogs;
using BlazorWebTemplate.Web.Features.AuditLogs.Constants;
using BlazorWebTemplate.Web.Services.Shell;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace BlazorWebTemplate.Web.Features.AuditLogs.Pages;

public partial class AuditLogs
{
    [Inject]
    private IAuditLogService AuditLogService { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    [SupplyParameterFromQuery(Name = "entity")]
    public string? Entity { get; set; }

    [SupplyParameterFromQuery(Name = "action")]
    public string? ActionFilter { get; set; }

    [SupplyParameterFromQuery(Name = "from")]
    public string? From { get; set; }

    [SupplyParameterFromQuery(Name = "to")]
    public string? To { get; set; }

    [SupplyParameterFromQuery(Name = "page")]
    public int? Page { get; set; }

    protected IReadOnlyList<AppShellBreadcrumb> _breadcrumbs = BreadcrumbFor.Index();
    protected List<AuditLogSummary> _logs = [];
    protected bool _isLoading = true;
    protected string? _errorMessage;
    protected string _entityFilter = string.Empty;
    protected string _actionFilter = string.Empty;
    protected DateRange? _dateRange;
    protected int _currentPage = 1;
    protected int _totalPages = 1;
    protected int _totalCount;
    protected readonly HashSet<string> _expandedRows = [];

    protected override async Task OnParametersSetAsync()
    {
        _isLoading = true;
        _expandedRows.Clear();
        _entityFilter = Entity ?? string.Empty;
        _actionFilter = ActionFilter ?? string.Empty;
        _currentPage = Page is > 0 ? Page.Value : 1;

        DateTime? fromDate = DateTime.TryParse(From, out var parsedFrom) ? parsedFrom : null;
        DateTime? toDate = DateTime.TryParse(To, out var parsedTo) ? parsedTo : null;

        _dateRange = (fromDate.HasValue || toDate.HasValue)
            ? new DateRange(fromDate, toDate)
            : null;

        var result = await AuditLogService.GetLogsAsync(
            entityName: Entity,
            action: string.IsNullOrWhiteSpace(_actionFilter) ? null : _actionFilter,
            fromDate: fromDate,
            toDate: toDate,
            pageNumber: _currentPage);

        _isLoading = false;

        if (result.IsFailure)
        {
            _errorMessage = result.Error?.Message ?? "Audit logs could not be loaded.";
            _logs = [];
            return;
        }

        _errorMessage = null;
        var paged = result.Value!;
        _logs = paged.Items.ToList();
        _totalPages = paged.TotalPages;
        _totalCount = paged.TotalCount;
        _currentPage = paged.PageNumber;
    }

    protected void ApplyFilter()
        => NavigationManager.NavigateTo(RouteFor.WithFilters(
            _entityFilter,
            _actionFilter,
            _dateRange?.Start,
            _dateRange?.End));

    protected void ClearFilter()
    {
        _dateRange = null;
        _actionFilter = string.Empty;
        NavigationManager.NavigateTo(RouteFor.Index);
    }

    protected void OnPageChanged(int page)
        => NavigationManager.NavigateTo(RouteFor.WithFilters(
            _entityFilter,
            _actionFilter,
            _dateRange?.Start,
            _dateRange?.End,
            page));

    protected void ToggleExpand(string logId)
    {
        if (!_expandedRows.Add(logId))
            _expandedRows.Remove(logId);
    }

    protected async Task CopyToClipboardAsync(string text)
    {
        try { await JSRuntime.InvokeVoidAsync("navigator.clipboard.writeText", text); }
        catch { /* clipboard unavailable */ }
    }

    protected static Color ActionColor(string action) => action.ToUpperInvariant() switch
    {
        "INSERT" or "CREATE" => Color.Success,
        "UPDATE" or "MODIFY" => Color.Info,
        "DELETE" or "REMOVE" => Color.Error,
        _ => Color.Default,
    };

    protected static string AvatarColor(string? identifier)
    {
        if (string.IsNullOrWhiteSpace(identifier)) return "#1f4fd8";
        string[] colors = ["#1f4fd8", "#00695e", "#7c3aed", "#dc2626", "#d97706", "#0891b2", "#16a34a", "#db2777"];
        return colors[Math.Abs(identifier[0]) % colors.Length];
    }
}
