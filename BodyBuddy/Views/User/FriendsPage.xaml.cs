using BodyBuddy.ViewModels.User;

namespace BodyBuddy.Views.User;

public partial class FriendsPage : ContentPage
{
    private readonly FriendsViewModel _viewModel;
	public FriendsPage(FriendsViewModel friendsViewModel)
	{
		InitializeComponent();
		_viewModel = friendsViewModel;
        BindingContext = friendsViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.Initialize();
    }
}