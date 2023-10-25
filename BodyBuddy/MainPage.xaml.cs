using BodyBuddy.ViewModels;

namespace BodyBuddy;

public partial class MainPage : ContentPage
{
	private MainPageViewModel _viewModel;

	public MainPage(MainPageViewModel mainPageViewModel)
	{
		InitializeComponent();
		_viewModel = mainPageViewModel;
		BindingContext = mainPageViewModel;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await Task.Delay(100); // Add a short delay

        await _viewModel.GetDailyQuote();
    }
}

