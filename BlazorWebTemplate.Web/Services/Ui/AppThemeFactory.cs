using MudBlazor;
using BlazorWebTemplate.Web.Services.AppInfo;

namespace BlazorWebTemplate.Web.Services.Ui;

public static class AppThemeFactory
{
    public static MudTheme Create(AppBrandOptions brand)
        => new()
        {
            PaletteLight = new PaletteLight
            {
                Primary = brand.PrimaryColor,
                Secondary = brand.SecondaryColor,
                Tertiary = brand.AccentColor,
                Background = "#F3F6FB",
                Surface = "#FFFFFF",
                AppbarBackground = "#0F172A",
                DrawerBackground = "#0B1220",
                DrawerText = "#DCE7F6",
                TextPrimary = "#132033",
                TextSecondary = "#526077",
                Success = "#0F9F76",
                Warning = "#B7791F",
                Error = "#C2413B",
                Info = "#2563EB",
                Divider = "#D8E1ED",
                LinesDefault = "#D8E1ED",
                TableLines = "#E7EDF5",
                ActionDefault = brand.PrimaryColor,
                ActionDisabled = "#A6B5C6"
            },
            Typography = new Typography
            {
                Default = new DefaultTypography
                {
                    FontFamily = ["Public Sans", "sans-serif"]
                },
                H1 = new H1Typography
                {
                    FontFamily = ["Plus Jakarta Sans", "sans-serif"],
                    FontWeight = "800",
                    FontSize = "3.2rem",
                    LineHeight = "1"
                },
                H2 = new H2Typography
                {
                    FontFamily = ["Plus Jakarta Sans", "sans-serif"],
                    FontWeight = "800",
                    FontSize = "2.4rem",
                    LineHeight = "1.05"
                },
                H3 = new H3Typography
                {
                    FontFamily = ["Plus Jakarta Sans", "sans-serif"],
                    FontWeight = "750",
                    FontSize = "1.7rem",
                    LineHeight = "1.12"
                },
                H4 = new H4Typography
                {
                    FontFamily = ["Plus Jakarta Sans", "sans-serif"],
                    FontWeight = "700",
                    FontSize = "1.2rem",
                    LineHeight = "1.25"
                },
                Subtitle1 = new Subtitle1Typography
                {
                    FontWeight = "700",
                    FontSize = "1rem"
                },
                Body1 = new Body1Typography
                {
                    FontSize = "1rem",
                    LineHeight = "1.65"
                },
                Button = new ButtonTypography
                {
                    FontWeight = "700",
                    LetterSpacing = ".01em",
                    TextTransform = "none"
                },
                Overline = new OverlineTypography
                {
                    FontWeight = "800",
                    LetterSpacing = ".16em",
                    FontSize = ".72rem"
                }
            },
            LayoutProperties = new LayoutProperties
            {
                DefaultBorderRadius = "16px"
            },
            Shadows = new Shadow()
        };
}
