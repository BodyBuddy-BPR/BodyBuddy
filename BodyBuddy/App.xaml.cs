namespace BodyBuddy;

public partial class App : Application
{
	public App()
	{
        //Register Syncfusion license
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NHaF5cXmVCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdgWXZccnRWR2RfV0V/XUs=");

        InitializeComponent();

		MainPage = new AppShell();
	}
}
