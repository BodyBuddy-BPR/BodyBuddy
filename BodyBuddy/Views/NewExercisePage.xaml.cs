namespace BodyBuddy.Views;

public partial class NewExercisePage : ContentPage
{
	private NewExerciseViewModel _viewModel;
	public NewExercisePage(NewExerciseViewModel newExerciseViewModel)
	{
		InitializeComponent();

		_viewModel = newExerciseViewModel;
		BindingContext = newExerciseViewModel;
	}
}