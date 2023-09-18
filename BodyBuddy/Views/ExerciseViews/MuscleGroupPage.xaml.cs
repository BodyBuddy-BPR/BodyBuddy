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
		await _viewModel.GetMusclegroups();
	}

}