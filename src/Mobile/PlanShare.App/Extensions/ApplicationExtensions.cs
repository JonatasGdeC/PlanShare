namespace PlanShare.App.Extensions;

public static class ApplicationExtensions
{
    private static bool IsLightMode(this Application application) => application.RequestedTheme == AppTheme.Light;

    public static Color GetPrimaryColor(this Application application)
    {
        bool isLightMode = application.IsLightMode();

        string key = isLightMode ? "PRIMARY_COLOR_LIGHT" : "PRIMARY_COLOR_DARK";

        return (Color)application.Resources[index: key];
    }

    public static Color GetSecondaryColor(this Application application)
    {
        bool isLightMode = application.IsLightMode();

        string key = isLightMode ? "SECONDARY_COLOR_LIGHT" : "SECONDARY_COLOR_DARK";

        return (Color)application.Resources[index: key];
    }

    public static Color GetLineColor(this Application application)
    {
        bool isLightMode = application.IsLightMode();

        string key = isLightMode ? "LINES_COLOR_LIGHT" : "LINES_COLOR_DARK";

        return (Color)application.Resources[index: key];
    }

    public static Color GetSkeletonViewColor(this Application application)
    {
        bool isLightMode = application.IsLightMode();

        string key = isLightMode ? "SKELETON_LOADING_COLOR_LIGHT" : "SKELETON_LOADING_COLOR_DARK";

        return (Color)application.Resources[index: key];
    }

    public static Color GetHighlightColor(this Application application)
    {
        bool isLightMode = application.IsLightMode();

        string key = isLightMode ? "HIGHLIGHT_COLOR_LIGHT" : "HIGHLIGHT_COLOR_DARK";

        return (Color)application.Resources[index: key];
    }

    public static Color GetDangerColor(this Application application)
    {
        bool isLightMode = application.IsLightMode();

        string key = isLightMode ? "DANGER_ACTION_COLOR_LIGHT" : "DANGER_ACTION_COLOR_DARK";

        return (Color)application.Resources[index: key];
    }
}