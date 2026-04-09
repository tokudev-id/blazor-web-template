namespace BlazorWebTemplate.Web.Common.Constants;

public static class CommonRouteFor
{
    public const string Root = "/";
    public const string Dashboard = "/dashboard";
    public const string Login = "/login";
    public const string Error = "/error";
    public const string AccountLogin = "/account/login";
    public const string AccountLogout = "/account/logout";

    public static string LoginWithReturnUrl(string? returnUrl, string? error = null)
    {
        var query = new List<string>();

        if (!string.IsNullOrWhiteSpace(returnUrl))
        {
            query.Add($"returnUrl={Uri.EscapeDataString(returnUrl)}");
        }

        if (!string.IsNullOrWhiteSpace(error))
        {
            query.Add($"error={Uri.EscapeDataString(error)}");
        }

        return query.Count == 0 ? Login : $"{Login}?{string.Join("&", query)}";
    }
}
