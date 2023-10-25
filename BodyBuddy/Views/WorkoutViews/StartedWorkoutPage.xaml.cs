using BodyBuddy.ViewModels.WorkoutViewModels;

namespace BodyBuddy.Views.WorkoutViews;

public partial class StartedWorkoutPage : ContentPage
{
	private StartedWorkoutViewModel _viewModel;

	public StartedWorkoutPage(StartedWorkoutViewModel startedWorkoutViewModel)
	{
		InitializeComponent();
		_viewModel = startedWorkoutViewModel;
		BindingContext = startedWorkoutViewModel;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await Task.Delay(50); // Add a short delay

        await _viewModel.Initialize();
    }
}