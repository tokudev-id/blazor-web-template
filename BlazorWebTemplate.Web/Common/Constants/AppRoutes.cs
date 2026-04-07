namespace BlazorWebTemplate.Web.Common.Constants;

public static class AppRoutes
{
    public const string Root = "/";
    public const string Dashboard = "/dashboard";
    public const string Login = "/login";
    public const string Error = "/error";
    public const string Posts = "/posts";
    public const string NewPost = "/posts/new";
    public const string Categories = "/categories";
    public const string Users = "/users";
    public const string AccountLogin = "/account/login";
    public const string AccountLogout = "/account/logout";

    public static string PostDetails(int id) => $"{Posts}/{id}";

    public static string PostEdit(int id) => $"{Posts}/{id}/edit";

    public static string PostDelete(int id) => $"{Posts}/{id}/delete";

    public static string PostsByTag(string tag) => $"{Posts}?tag={Uri.EscapeDataString(tag)}";

    public static string CategoriesSearch(string search)
        => string.IsNullOrWhiteSpace(search) ? Categories : $"{Categories}?search={Uri.EscapeDataString(search)}";

    public static string UsersSearch(string search)
        => string.IsNullOrWhiteSpace(search) ? Users : $"{Users}?search={Uri.EscapeDataString(search)}";

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
