using Microsoft.Maui.Handlers;
using PlanShare.App.Constants;
using PlanShare.App.Navigation;
using PlanShare.App.Views.Pages.Login.DoLogin;
using PlanShare.App.Views.Pages.User.Register;

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

        builder.ConfigureMauiHandlers(handlers =>
        {
            EntryHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
            {
                #if ANDROID
                    handler.PlatformView.Background = null;
                #endif
            });
        });

        return builder.Build();
    }
    
    private static MauiAppBuilder AddPages(this MauiAppBuilder appBuilder)
    {
        Routing.RegisterRoute(route: RoutePages.LOGIN_PAGE, type: typeof(DoLoginPage));
        Routing.RegisterRoute(route: RoutePages.USER_REGISTER_PAGE, type: typeof(RegisterUserPage));
        
        return appBuilder;
    }
    
    private static void AddFonts(this MauiAppBuilder appBuilder)
    {
        appBuilder.ConfigureFonts(configureDelegate: fonts =>
        {
            fonts.AddFont(filename: "Raleway-Black.ttf", alias: FontFamily.MAIN_FONT_BLACK);
            fonts.AddFont(filename: "Raleway-Regular.ttf", alias: FontFamily.MAIN_FONT_REGULAR);
            fonts.AddFont(filename: "Raleway-Light.ttf", alias: FontFamily.MAIN_FONT_LIGHT);
            fonts.AddFont(filename: "WorkSans-Black.ttf", alias: FontFamily.SECONDARY_FONT_BLACK);
            fonts.AddFont(filename: "WorkSans-Regular.ttf", alias: FontFamily.SECONDARY_FONT_REGULAR);
        });
    }
}