using BodyBuddy.ViewModels.WorkoutViewModels;

namespace BodyBuddy.Views.WorkoutViews;

public partial class PreMadeWorkoutDetailsPage : ContentPage
{
    private PreMadeWorkoutDetailsViewModel _viewModel;

    public PreMadeWorkoutDetailsPage(PreMadeWorkoutDetailsViewModel preMadeWorkoutDetailsViewModel)
    {
        InitializeComponent();

        _viewModel = preMadeWorkoutDetailsViewModel;
        BindingContext = preMadeWorkoutDetailsViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.GetExercisesFromWorkout();
    }
}