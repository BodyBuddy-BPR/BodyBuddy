using BodyBuddy.CustomControls;
using Syncfusion.Licensing;

namespace BodyBuddy;

public partial class App : Application
{
    public App()
    {
        //Register Syncfusion license
        SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NHaF5cXmVCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdgWXZcdXRSRmVcVkNzV0s=");

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
