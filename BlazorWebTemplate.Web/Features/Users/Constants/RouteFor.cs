namespace BlazorWebTemplate.Web.Features.Users.Constants;

public static class RouteFor
{
    public const string Index = "/users";

    public static string WithSearch(string search)
        => string.IsNullOrWhiteSpace(search) ? Index : $"{Index}?search={Uri.EscapeDataString(search)}";
}
