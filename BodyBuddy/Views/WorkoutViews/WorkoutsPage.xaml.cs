using BodyBuddy.Helpers;
using BodyBuddy.ViewModels.WorkoutViewModels;
using Mopups.Interfaces;
using System.Text.RegularExpressions;

namespace BodyBuddy.Views.WorkoutViews;

public partial class WorkoutsPage : ContentPage
{
    private WorkoutViewModel _viewModel;
    private IPopupNavigation _popupNavigation;

    public WorkoutsPage(WorkoutViewModel workoutsViewModel, IPopupNavigation popupNavigation)
    {
        InitializeComponent();
        _viewModel = workoutsViewModel;
        BindingContext = workoutsViewModel;

        _popupNavigation = popupNavigation;
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await Task.Delay(100); // Add a short delay

        PageDetector();

        await _viewModel.GetWorkoutPlans();
    }


    // Button to show CreateWorkoutPage Popup
    private void ClickToShowPopup_Clicked(object sender, EventArgs e)
    {
        //popup.Show();
        _popupNavigation.PushAsync(new CreateWorkoutPage(_viewModel));
    }


    // This method finds out if you need the premade workouts or the users own
    private void PageDetector()
    {
        string title = Shell.Current.CurrentItem?.CurrentItem?.CurrentItem?.Title;

        if (title == Strings.PremadeWorkOuts)
        {
            _viewModel.IsPreMadeWorkout = true;
        }
        else
        {
            _viewModel.IsPreMadeWorkout = false;
        }

        _viewModel.Title = title;
    }
}