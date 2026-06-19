using PlanShare.App.Constants;
using PlanShare.App.Views.Pages.Login.DoLogin;

namespace PlanShare.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .AddPages()
            .AddFonts();
        
        return builder.Build();
    }
    
    private static MauiAppBuilder AddPages(this MauiAppBuilder appBuilder)
    {
        Routing.RegisterRoute(route: "DoLoginPage", type: typeof(DoLoginPage));
        
        return appBuilder;
    }
    
    private static void AddFonts(this MauiAppBuilder appBuilder)
    {
        appBuilder.ConfigureFonts(configureDelegate: fonts =>
        {
            fonts.AddFont(filename: "Raleway-Black.ttf", alias: FontFamily.MAIN_FONT_BLACK);
            fonts.AddFont(filename: "Raleway-Regular.ttf", alias: FontFamily.MAIN_FONT_REGULAR);
            fonts.AddFont(filename: "Raleway-Thin.ttf", alias: FontFamily.MAIN_FONT_THIN);
            fonts.AddFont(filename: "WorkSans-Black.ttf", alias: FontFamily.SECONDARY_FONT_BLACK);
            fonts.AddFont(filename: "WorkSans-Regular.ttf", alias: FontFamily.SECONDARY_FONT_REGULAR);
        });
    }
}