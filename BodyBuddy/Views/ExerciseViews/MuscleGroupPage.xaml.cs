using BodyBuddy.ViewModels.ExerciseViewModels;

namespace BodyBuddy.Views.ExerciseViews;

public partial class MuscleGroupPage : ContentPage
{
	private MuscleGroupViewModel _viewModel;
	public MuscleGroupPage(MuscleGroupViewModel muscleGroupViewModel) 
	{
		InitializeComponent();
		_viewModel = muscleGroupViewModel;
		BindingContext = muscleGroupViewModel;
    }

	protected override async void OnAppearing()
	{
        base.OnAppearing();
        
        await Task.Delay(100); // Add a short delay

        await _viewModel.GetMuscleGroups();
    }
}