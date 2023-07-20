namespace BodyBuddy.Views;

public partial class WorkoutPlanDetailsPage : ContentPage
{
	private WorkoutPlanDetailsViewModel _viewModel;
	public WorkoutPlanDetailsPage(WorkoutPlanDetailsViewModel workoutPlanDetailsViewModel)
	{
		InitializeComponent();
		_viewModel = workoutPlanDetailsViewModel;
		BindingContext = workoutPlanDetailsViewModel;
	}
}