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

        popup.Closing += Popup_Closing;
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

    private void Popup_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        if (!_viewModel.CanClose)
        {
            e.Cancel = true;
            return;
        }
        e.Cancel = false;
    }

    //This method only gets called the first time the popup is attempted closed, and I dont know why :-(
    //private void PopupLayout_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    //{
    //	if (_viewModel.ErrorMessage != string.Empty)
    //	{
    //		e.Cancel = true;
    //	}
    //}
}