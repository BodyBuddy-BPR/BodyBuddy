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
	}
}
