using BodyBuddy.ViewModels;

namespace BodyBuddy.Views.ExerciseViews;

public partial class ExercisesPage : ContentPage
{
    private ExercisesViewModel _viewModel;
    public ExercisesPage(ExercisesViewModel exercisesViewModel)
    {
        InitializeComponent();
        _viewModel = exercisesViewModel;
        BindingContext = exercisesViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.GetExercises();
        await _viewModel.GetWorkouts();

        // Set SelectedWorkout based on _workoutPlan
        if (_viewModel.WorkoutsList.Any(x => x.Id == _viewModel.WorkoutPlan.Id))
        {
            _viewModel.SelectedWorkout = _viewModel.WorkoutPlan;
        }
    }


}