using BodyBuddy.ViewModels.Profile;

namespace BodyBuddy.Views.StatisticsViews;

public partial class IntakeGraph : ContentPage
{
    private readonly ProfileGraphViewModel _viewModel;

    public IntakeGraph(ProfileGraphViewModel profileGraphViewModel)
    {
        InitializeComponent();

        _viewModel = profileGraphViewModel;
        BindingContext = profileGraphViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.GetAllIntakeData();
    }
}