using Microsoft.Extensions.Logging;

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
                fonts.AddFont(filename: "OpenSans-Regular.ttf", alias: "OpenSansRegular");
                fonts.AddFont(filename: "OpenSans-Semibold.ttf", alias: "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}