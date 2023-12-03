using BodyBuddy.Controls.Common;
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

        // Adding Common Toolbar items
        CommonToolBarItems.AddCommonToolbarItems(this);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await Task.Delay(200); // Add a short delay

        await _viewModel.AttemptToLogin();
        await _viewModel.Initialize();
        await _viewModel.GetFriendsSteps();
    }

    private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        _viewModel.ClickToShowPopup_Clicked();
    }
}

