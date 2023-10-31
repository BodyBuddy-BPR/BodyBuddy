using BodyBuddy.ViewModels.WorkoutViewModels;

namespace BodyBuddy.Views.Popups;

public partial class EditSetsAndRepsPopup
{
    private readonly WorkoutDetailsViewModel _viewModel;

    public EditSetsAndRepsPopup(WorkoutDetailsViewModel workoutDetailsViewModel)
	{
		InitializeComponent();
        _viewModel = workoutDetailsViewModel;
        BindingContext = workoutDetailsViewModel;
    }



    // Buttons that increase or decrease the count of Sets and Reps
    private void MinusSetsBtn_Clicked(object sender, EventArgs e)
    {
        if (_viewModel.EditSets > 0)
        {
            _viewModel.EditSets--;
            SetsLabel.Text = _viewModel.EditSets.ToString();
        }
    }
    private void PlusSetsBtn_Clicked(object sender, EventArgs e)
    {
        _viewModel.EditSets++;
        SetsLabel.Text = _viewModel.EditSets.ToString();
    }
    private void MinusRepsBtn_Clicked(object sender, EventArgs e)
    {
        if (_viewModel.EditReps > 0)
        {
            _viewModel.EditReps--;
            RepsLabel.Text = _viewModel.EditReps.ToString();
        }
    }
    private void PlusRepsBtn_Clicked(object sender, EventArgs e)
    {
        _viewModel.EditReps++;
        RepsLabel.Text = _viewModel.EditReps.ToString();
    }
}