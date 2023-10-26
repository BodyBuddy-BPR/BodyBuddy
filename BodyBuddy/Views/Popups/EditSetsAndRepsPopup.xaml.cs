using BodyBuddy.ViewModels.WorkoutViewModels;

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
        if (_viewModel.SetsToEdit > 1)
        {
            _viewModel.SetsToEdit--;
            SetsLabel.Text = _viewModel.SetsToEdit.ToString();
        }
    }
    private void PlusSetsBtn_Clicked(object sender, EventArgs e)
    {
        _viewModel.SetsToEdit++;
        SetsLabel.Text = _viewModel.SetsToEdit.ToString();
    }
    private void MinusRepsBtn_Clicked(object sender, EventArgs e)
    {
        if (_viewModel.RepsToEdit > 1)
        {
            _viewModel.RepsToEdit--;
            RepsLabel.Text = _viewModel.RepsToEdit.ToString();
        }
    }
    private void PlusRepsBtn_Clicked(object sender, EventArgs e)
    {
        _viewModel.RepsToEdit++;
        RepsLabel.Text = _viewModel.RepsToEdit.ToString();
    }
}