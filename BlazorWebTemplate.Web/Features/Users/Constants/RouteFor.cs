namespace BlazorWebTemplate.Web.Features.Users.Constants;

public static class RouteFor
{
    public const string Index = "/users";

    public static string WithSearch(string search)
        => string.IsNullOrWhiteSpace(search) ? Index : $"{Index}?search={Uri.EscapeDataString(search)}";

    public static string WithSearchAndPage(string search, int page)
    {
        if (page <= 1 && string.IsNullOrWhiteSpace(search)) return Index;
        if (string.IsNullOrWhiteSpace(search)) return $"{Index}?page={page}";
        if (page <= 1) return WithSearch(search);
        return $"{Index}?search={Uri.EscapeDataString(search)}&page={page}";
    }
}
