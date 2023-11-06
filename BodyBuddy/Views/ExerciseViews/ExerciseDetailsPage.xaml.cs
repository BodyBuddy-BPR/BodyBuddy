using BodyBuddy.ViewModels.ExerciseViewModels;

namespace BodyBuddy.Views.ExerciseViews;

public partial class ExerciseDetailsPage : ContentPage
{
	private ExerciseDetailsViewModel _viewModel;
	public ExerciseDetailsPage(ExerciseDetailsViewModel exerciseDetailsViewModel)
	{
		InitializeComponent();
		_viewModel = exerciseDetailsViewModel;
		BindingContext = exerciseDetailsViewModel;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await Task.Delay(50); // Add a short delay

        await _viewModel.GetExerciseDetails();

        SetExerciseDetailsValueTexts();
    }

    private void SetExerciseDetailsValueTexts()
    {
        if (_viewModel.ExerciseDetails.Level == null) LevelValue.Text = "---";
        if (_viewModel.ExerciseDetails.Mechanic == null) MechanicValue.Text = "---";
        if (_viewModel.ExerciseDetails.Equipment == null) EquipmentValue.Text = "---";
        if (_viewModel.ExerciseDetails.Force == null) ForceValue.Text = "---";
    }
}