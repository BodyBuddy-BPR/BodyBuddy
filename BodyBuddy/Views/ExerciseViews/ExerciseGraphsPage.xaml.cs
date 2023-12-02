using BodyBuddy.ViewModels.ExerciseViewModels;

namespace BodyBuddy.Views.ExerciseViews;

public partial class ExerciseGraphsPage : ContentPage
{
    private readonly ExerciseGraphsViewModel _viewModel;
	public ExerciseGraphsPage(ExerciseGraphsViewModel exerciseGraphsViewModel)
	{
		InitializeComponent();
        BindingContext = exerciseGraphsViewModel;
        _viewModel = exerciseGraphsViewModel;
    }

    protected override async void OnAppearing()
    {
        await _viewModel.GetWorkoutDataForExercise();
    }
}