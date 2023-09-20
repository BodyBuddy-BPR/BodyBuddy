using BodyBuddy.ViewModels.WorkoutViewModels;

namespace BodyBuddy.Views.WorkoutViews;

public partial class WorkoutDetailsPage : ContentPage
{
	private WorkoutDetailsViewModel _viewModel;
	public WorkoutDetailsPage(WorkoutDetailsViewModel workoutDetailsViewModel)
	{
		InitializeComponent();

		_viewModel = workoutDetailsViewModel;
		BindingContext = workoutDetailsViewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await _viewModel.GetExercisesFromWorkout();
	}
}