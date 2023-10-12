using BodyBuddy.ViewModels.WorkoutViewModels;

namespace BodyBuddy.Views.WorkoutViews;

public partial class PreMadeWorkoutDetailsPage : ContentPage
{
    private WorkoutDetailsViewModel _viewModel;

    public PreMadeWorkoutDetailsPage(WorkoutDetailsViewModel workoutDetailsViewModel)
    {
        InitializeComponent();

        _viewModel = workoutDetailsViewModel;
        BindingContext = workoutDetailsViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.Initialize();
    }
}