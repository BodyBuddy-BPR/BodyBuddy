using BodyBuddy.ViewModels.IntakeViewModels;

namespace BodyBuddy.Views.StatisticsViews;

public partial class IntakeGraph : ContentPage
{
    private readonly IntakeGraphViewModel _viewModel;

    public IntakeGraph(IntakeGraphViewModel intakeGraphViewModel)
    {
        InitializeComponent();

        _viewModel = intakeGraphViewModel;
        BindingContext = intakeGraphViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.GetAllIntakeData();
    }
}