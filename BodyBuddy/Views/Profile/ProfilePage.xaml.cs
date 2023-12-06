using BodyBuddy.ViewModels.Profile;

namespace BodyBuddy.Views.Profile;

public partial class ProfilePage : ContentPage
{
    private ProfileViewModel _viewModel;

    public ProfilePage(ProfileViewModel profileViewModel)
    {
        InitializeComponent();
        _viewModel = profileViewModel;
        BindingContext = profileViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await Task.Delay(400);

        await _viewModel.Initialize();

        // Changes the button text depending on if the user is logged in
        LoginBtn.Text = _viewModel.IsLoggedIn switch
        {
            true => "Sign out",
            false => "Sign In"
        };
    }

    private async void LoginOrOut_Clicked(object sender, EventArgs e)
    {
        if (!_viewModel.IsLoggedIn)
        {
            await _viewModel.LogIn();
        }
        else if (_viewModel.IsLoggedIn)
        {
            await _viewModel.LogOut();
        }
    }

    private void WeekdayButtonClicked(object sender, EventArgs e)
    {
		Button clickedButton = (Button)sender;
		_viewModel.WeekdayButtonClicked(int.Parse(clickedButton.CommandParameter.ToString()));
    }

}