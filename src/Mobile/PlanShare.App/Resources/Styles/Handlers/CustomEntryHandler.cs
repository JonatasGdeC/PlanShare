using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using PlanShare.App.Extensions;

namespace PlanShare.App.Resources.Styles.Handlers;

public static class CustomEntryHandler
{
    public static void Customize()
    {
        EntryHandler.Mapper.AppendToMapping(key: "PlanShareEntry", method: (handler, entry) =>
        {
            handler.PlatformView.Background = null;
            Color cursorColor = Application.Current!.GetPrimaryColor();
            Color lineColor = Application.Current!.GetLineColor();

#if ANDROID
            handler.PlatformView.TextCursorDrawable?.SetTint(tintColor: cursorColor.ToPlatform());
            handler.PlatformView.Background?.SetTint(tintColor: lineColor.ToPlatform());
#elif IOS || MACCATALYST
            handler.PlatformView.Layer.BorderColor = lineColor.ToCGColor();
            handler.PlatformView.Layer.BorderWidth = 0.7f;
            handler.PlatformView.Layer.CornerRadius = 5;
            handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
            handler.PlatformView.TintColor = cursorColor.ToPlatform();
#endif
        });
    }
}