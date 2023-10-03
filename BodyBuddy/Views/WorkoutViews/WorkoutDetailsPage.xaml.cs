using BodyBuddy.Models;
using BodyBuddy.ViewModels.WorkoutViewModels;
using BodyBuddy.Views.Popups;
using Mopups.Interfaces;

namespace BodyBuddy.Views.WorkoutViews;

public partial class WorkoutDetailsPage : ContentPage
{
	private WorkoutDetailsViewModel _viewModel;
    IPopupNavigation _popupNavigation;


    public WorkoutDetailsPage(WorkoutDetailsViewModel workoutDetailsViewModel, IPopupNavigation popupNavigation)
	{
		InitializeComponent();

		_viewModel = workoutDetailsViewModel;
		BindingContext = workoutDetailsViewModel;

        _popupNavigation = popupNavigation;
    }

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await _viewModel.Initialize();
	}

    private void EditBtn_Clicked(object sender, EventArgs e)
    {
		_viewModel.PopupName = _viewModel.WorkoutName;
		_viewModel.PopupDescription = _viewModel.WorkoutDescription;

        //popup.Show();
        _popupNavigation.PushAsync(new EditWorkoutPopup(_viewModel));
    }

    private void SetsAndRepsBtn_Clicked(object sender, EventArgs e)
    {
        if (sender is ImageButton button && button.CommandParameter is Exercise exercise)
        {
            _viewModel.ExerciseToEdit = exercise;
            _popupNavigation.PushAsync(new EditSetsAndRepsPopup(_viewModel));
            // Now 'exercise' holds the Exercise object associated with the clicked button.
            // You can use it as needed.
            // For example, you can pass it to another page, show a popup, etc.
        }
    }
}