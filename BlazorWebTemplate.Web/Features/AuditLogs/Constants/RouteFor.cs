namespace BlazorWebTemplate.Web.Features.AuditLogs.Constants;

public static class RouteFor
{
    public const string Index = "/audit-logs";

    public static string WithFilters(
        string? entityName = null,
        string? action = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int page = 1)
    {
        var parts = new List<string>();
        if (!string.IsNullOrWhiteSpace(entityName))
            parts.Add($"entity={Uri.EscapeDataString(entityName)}");
        if (!string.IsNullOrWhiteSpace(action))
            parts.Add($"action={Uri.EscapeDataString(action)}");
        if (fromDate.HasValue)
            parts.Add($"from={fromDate.Value:yyyy-MM-dd}");
        if (toDate.HasValue)
            parts.Add($"to={toDate.Value:yyyy-MM-dd}");
        if (page > 1)
            parts.Add($"page={page}");
        return parts.Count == 0 ? Index : $"{Index}?{string.Join('&', parts)}";
    }
}
