using BodyBuddy.ViewModels.Profile;
using BodyBuddy.Views.Authentication;
using Mopups.Interfaces;

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
        await _viewModel.Initialize();
    }

    private async void Login_Clicked(object sender, EventArgs e)
    {
        //await _popupNavigation.PushAsync(new LoginPage());
        await Shell.Current.GoToAsync(nameof(LoginPage), true);
    }
}