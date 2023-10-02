using BodyBuddy.ViewModels.WorkoutViewModels;

namespace BodyBuddy.Views.WorkoutViews;

public partial class PreMadeWorkoutsPage : ContentPage
{
    private WorkoutViewModel _viewModel;

    public PreMadeWorkoutsPage(WorkoutViewModel workoutsViewModel)
	{
		InitializeComponent();
        _viewModel = workoutsViewModel;
        BindingContext = workoutsViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.GetWorkoutPlans();
    }
}