using BodyBuddy.ViewModels.WorkoutViewModels;

namespace BodyBuddy.Views.WorkoutViews;

public partial class PreMadeWorkoutsPage : ContentPage
{
	private PreMadeWorkoutsViewModel _viewModel;

	public PreMadeWorkoutsPage(PreMadeWorkoutsViewModel preMadeWorkoutsViewModel)
	{
		InitializeComponent();
        _viewModel = preMadeWorkoutsViewModel;
        BindingContext = preMadeWorkoutsViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.GetPreMadeWorkoutPlans();
    }
}