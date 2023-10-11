using BodyBuddy.CustomControls;
using BodyBuddy.Database;
using Syncfusion.Licensing;

namespace BodyBuddy;

public partial class App : Application
{
    public App()
    {
        //Register Syncfusion license
#if GITHUB_BUILD
    string syncfusionKey = Environment.GetEnvironmentVariable("SYNCFUNCTION_KEY");
#else
        string syncfusionKey = AppSettingKeys.SYNCFUNCTION_KEY;
#endif
        SyncfusionLicenseProvider.RegisterLicense(syncfusionKey);

        InitializeComponent();

        MainPage = new AppShell();



        // Custom Entry
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(CustomEntry), (handler, view) =>
        {
            #if __ANDROID__
            handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#elif __IOS__
                handler.PlatformView.BackgroundColor = UIKit.UIColor color = UIColor.Transparent;
                handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
        });
    }
}
