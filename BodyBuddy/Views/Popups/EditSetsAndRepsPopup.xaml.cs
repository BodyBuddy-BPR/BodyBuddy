using BodyBuddy.ViewModels.WorkoutViewModels;
using CommunityToolkit.Maui.Core;
using Mopups.Services;

namespace BodyBuddy.Views.Popups;

public partial class EditSetsAndRepsPopup
{
    private WorkoutDetailsViewModel _viewModel;

    public EditSetsAndRepsPopup(WorkoutDetailsViewModel workoutDetailsViewModel)
	{
		InitializeComponent();
        _viewModel = workoutDetailsViewModel;
        BindingContext = workoutDetailsViewModel;
    }



    // Buttons that increase or decrease the count of Sets and Reps
    private void MinusSetsBtn_Clicked(object sender, EventArgs e)
    {
        if (_viewModel.ExerciseToEdit.Sets > 0)
        {
            _viewModel.ExerciseToEdit.Sets--;
            SetsLabel.Text = _viewModel.ExerciseToEdit.Sets.ToString();
        }
    }
    private void PlusSetsBtn_Clicked(object sender, EventArgs e)
    {
        _viewModel.ExerciseToEdit.Sets++;
        SetsLabel.Text = _viewModel.ExerciseToEdit.Sets.ToString();
    }
    private void MinusRepsBtn_Clicked(object sender, EventArgs e)
    {
        if (_viewModel.ExerciseToEdit.Reps > 0)
        {
            _viewModel.ExerciseToEdit.Reps--;
            RepsLabel.Text = _viewModel.ExerciseToEdit.Reps.ToString();
        }
    }
    private void PlusRepsBtn_Clicked(object sender, EventArgs e)
    {
        _viewModel.ExerciseToEdit.Reps++;
        RepsLabel.Text = _viewModel.ExerciseToEdit.Reps.ToString();
    }
}