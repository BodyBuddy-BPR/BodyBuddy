using BodyBuddy.ViewModels.Profile;

namespace BodyBuddy.Views.Profile;

public partial class ProfilePage : ContentPage
{
    private ProfileViewModel _viewModel;
    public ProfilePage(ProfileViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.Initialize();
    }
}