using MudBlazor;

namespace BlazorWebTemplate.Web.Theme;

public static class EditorialMudTheme
{
    public static MudTheme Value { get; } = new()
    {
        PaletteLight = new PaletteLight
        {
            Background = "#F5F9FF",
            Surface = "#FFFFFF",
            AppbarBackground = "#FFFFFF",
            DrawerBackground = "#FBFDFF",
            Primary = "#1992F5",
            Secondary = "#3B82F6",
            Tertiary = "#BFE3FF",
            Info = "#0EA5E9",
            Success = "#22C55E",
            Warning = "#F59E0B",
            Error = "#EF4444",
            TextPrimary = "#132238",
            TextSecondary = "#64748B",
            Divider = "#E4ECF7",
            LinesDefault = "#E4ECF7",
            TableLines = "#EDF3FA",
            ActionDefault = "#1992F5",
            ActionDisabled = "#B9C7DA"
        },
        LayoutProperties = new LayoutProperties
        {
            DefaultBorderRadius = "18px"
        },
        Typography = new Typography
        {
            Default = new DefaultTypography
            {
                FontFamily = ["Manrope", "sans-serif"]
            },
            H1 = new H1Typography
            {
                FontFamily = ["Sora", "sans-serif"],
                FontWeight = "800",
                FontSize = "3rem",
                LineHeight = "1.02"
            },
            H2 = new H2Typography
            {
                FontFamily = ["Sora", "sans-serif"],
                FontWeight = "800",
                FontSize = "2.15rem",
                LineHeight = "1.08"
            },
            H3 = new H3Typography
            {
                FontFamily = ["Sora", "sans-serif"],
                FontWeight = "700",
                FontSize = "1.55rem",
                LineHeight = "1.15"
            },
            H4 = new H4Typography
            {
                FontFamily = ["Sora", "sans-serif"],
                FontWeight = "700",
                FontSize = "1.15rem",
                LineHeight = "1.25"
            },
            Subtitle1 = new Subtitle1Typography
            {
                FontWeight = "700",
                FontSize = "1rem"
            },
            Body1 = new Body1Typography
            {
                FontSize = "0.98rem",
                LineHeight = "1.75"
            },
            Button = new ButtonTypography
            {
                FontWeight = "800",
                LetterSpacing = ".01em",
                TextTransform = "none"
            },
            Overline = new OverlineTypography
            {
                FontWeight = "800",
                LetterSpacing = ".16em",
                FontSize = ".7rem"
            }
        },
        Shadows = new Shadow()
    };
}
