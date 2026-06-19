using PlanShare.App.Constants;

namespace PlanShare.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(configureDelegate: fonts =>
            {
                fonts.AddFont(filename: "Raleway-Black.ttf", alias: FontFamily.MAIN_FONT_BLACK);
                fonts.AddFont(filename: "Raleway-Regular.ttf", alias: FontFamily.MAIN_FONT_REGULAR);
                fonts.AddFont(filename: "Raleway-Thin.ttf", alias: FontFamily.MAIN_FONT_THIN);
                fonts.AddFont(filename: "WorkSans-Black.ttf", alias: FontFamily.SECONDARY_FONT_BLACK);
                fonts.AddFont(filename: "WorkSans-Regular.ttf", alias: FontFamily.SECONDARY_FONT_REGULAR);
            });
        
        return builder.Build();
    }
}