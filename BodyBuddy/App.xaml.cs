using BodyBuddy.CustomControls;
using BodyBuddy.Database;
using Syncfusion.Licensing;

namespace BodyBuddy;

public partial class App : Application
{
    public App()
    {
        //Register Syncfusion license
        SyncfusionLicenseProvider.RegisterLicense(AppSettingKeys.SYNCFUNCTION_KEY);

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
