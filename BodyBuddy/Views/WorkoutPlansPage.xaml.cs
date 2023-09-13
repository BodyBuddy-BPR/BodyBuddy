using BodyBuddy.ViewModels;

namespace BodyBuddy.Views;

public partial class WorkoutPlansPage : ContentPage
{
    private WorkoutPlansViewModel _viewModel;
    public WorkoutPlansPage(WorkoutPlansViewModel workoutPlansViewModel)
    {
        InitializeComponent();

        _viewModel = workoutPlansViewModel;
        BindingContext = workoutPlansViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.GetMyWorkoutPlans();
    }

    private void Workout_Created(object sender, EventArgs e)
    {
        NewWorkoutEntry.IsEnabled = false;
        NewWorkoutEntry.IsEnabled = true;
    }

}