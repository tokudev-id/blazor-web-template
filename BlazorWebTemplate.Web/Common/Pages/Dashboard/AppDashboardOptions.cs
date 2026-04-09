using System.ComponentModel.DataAnnotations;

namespace BlazorWebTemplate.Web.Common.Pages.Dashboard;

public sealed class AppDashboardOptions
{
    public const string SectionName = "AppDashboard";

    [Range(1, 60)]
    public int FreshnessMinutes { get; set; } = 2;
}
