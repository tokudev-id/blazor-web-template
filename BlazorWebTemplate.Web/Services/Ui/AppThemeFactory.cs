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
                Secondary = "#565E74",
                Tertiary = brand.AccentColor,
                Background = "#F7F9FB",
                Surface = "#FFFFFF",
                AppbarBackground = "#FFFFFF",
                DrawerBackground = "#FFFFFF",
                DrawerText = "#1A1B23",
                TextPrimary = "#1A1B23",
                TextSecondary = "#747686",
                Success = "#0F9F76",
                Warning = "#B7791F",
                Error = "#BA1A1A",
                Info = "#1F4FD8",
                Divider = "#E0E3E5",
                LinesDefault = "#E0E3E5",
                TableLines = "#EEF1F5",
                ActionDefault = brand.PrimaryColor,
                ActionDisabled = "#A6B5C6"
            },
            Typography = new Typography
            {
                Default = new DefaultTypography
                {
                    FontFamily = ["Inter", "sans-serif"]
                },
                H1 = new H1Typography
                {
                    FontFamily = ["Inter", "sans-serif"],
                    FontWeight = "800",
                    FontSize = "3.2rem",
                    LineHeight = "1"
                },
                H2 = new H2Typography
                {
                    FontFamily = ["Inter", "sans-serif"],
                    FontWeight = "800",
                    FontSize = "2.4rem",
                    LineHeight = "1.05"
                },
                H3 = new H3Typography
                {
                    FontFamily = ["Inter", "sans-serif"],
                    FontWeight = "750",
                    FontSize = "1.7rem",
                    LineHeight = "1.12"
                },
                H4 = new H4Typography
                {
                    FontFamily = ["Inter", "sans-serif"],
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
                DefaultBorderRadius = "8px"
            },
            Shadows = new Shadow()
        };
}
