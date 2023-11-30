using BodyBuddy.ViewModels.Profile;
using Syncfusion.Maui.Charts;
using System.Globalization;

namespace BodyBuddy.Views.Profile;

public partial class ProfileGraph : ContentPage
{
    private ProfileGraphViewModel _viewModel;

    public ProfileGraph(ProfileGraphViewModel profileGraphViewModel)
	{
        _viewModel = profileGraphViewModel;
        BindingContext = profileGraphViewModel;
        InitializeComponent();
	}

    //private void OnLabelCreated(object sender, ChartAxisLabelEventArgs e)
    //{
    //    // Assuming e.Label is a string representing the date in a specific format
    //    if (DateTime.TryParseExact(e.Label, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
    //    {
    //        // Format the date as needed. Example: "dd" for the day.
    //        e.Label = dateTime.ToString("dd");
    //    }
    //    else
    //    {
    //        // Handle parsing failure, perhaps set a default value or log an error
    //        Console.WriteLine("Failed to parse date: " + e.Label);
    //    }
    //}
}