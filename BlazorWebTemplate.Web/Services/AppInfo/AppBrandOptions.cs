using System.ComponentModel.DataAnnotations;

namespace BlazorWebTemplate.Web.Services.AppInfo;

public sealed class AppBrandOptions
{
    public const string SectionName = "Branding";

    [Required]
    [StringLength(80, MinimumLength = 2)]
    public string ProductName { get; set; } = "Unictive Control Center";

    [Required]
    [StringLength(120, MinimumLength = 6)]
    public string ProductTagline { get; set; } = "Enterprise content operations";

    [Required]
    [StringLength(180, MinimumLength = 12)]
    public string ProductDescription { get; set; } = "A reusable admin shell for content, workflow, and team operations.";

    [Required]
    [StringLength(80, MinimumLength = 2)]
    public string CompanyName { get; set; } = "Unictive";

    [Required]
    [EmailAddress]
    public string SupportEmail { get; set; } = "support@unictive.example";

    [Required]
    [RegularExpression("^#([0-9a-fA-F]{6})$", ErrorMessage = "PrimaryColor must be a 6-digit hex value, for example #1F4FD8.")]
    public string PrimaryColor { get; set; } = "#1F4FD8";

    [Required]
    [RegularExpression("^#([0-9a-fA-F]{6})$", ErrorMessage = "SecondaryColor must be a 6-digit hex value.")]
    public string SecondaryColor { get; set; } = "#0F172A";

    [Required]
    [RegularExpression("^#([0-9a-fA-F]{6})$", ErrorMessage = "AccentColor must be a 6-digit hex value.")]
    public string AccentColor { get; set; } = "#14B8A6";
}
