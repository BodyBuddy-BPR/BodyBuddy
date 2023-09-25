using BodyBuddy.ViewModels.WorkoutViewModels;

namespace BodyBuddy.Views.WorkoutViews;

public partial class WorkoutsPage : ContentPage
{
	private WorkoutViewModel _viewModel;

	public WorkoutsPage(WorkoutViewModel workoutsViewModel)
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


    private void ClickToShowPopup_Clicked(object sender, EventArgs e)
    {
        popup.Show();
    }
}