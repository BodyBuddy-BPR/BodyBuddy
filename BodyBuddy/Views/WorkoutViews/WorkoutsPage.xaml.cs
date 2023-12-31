using BodyBuddy.Helpers;
using BodyBuddy.ViewModels.WorkoutViewModels;
using Mopups.Interfaces;
using BodyBuddy.Controls.Common;

namespace BodyBuddy.Views.WorkoutViews;

public partial class WorkoutsPage : ContentPage
{
    private WorkoutViewModel _viewModel;
    private IPopupNavigation _popupNavigation;
    private bool _isFirstTime = true;


    public WorkoutsPage(WorkoutViewModel workoutsViewModel, IPopupNavigation popupNavigation)
    {
        InitializeComponent();
        _viewModel = workoutsViewModel;
        BindingContext = workoutsViewModel;

        _popupNavigation = popupNavigation;

        // Adding Common Toolbar items
        CommonToolBarItems.AddCommonToolbarItems(this);
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await Task.Delay(300); // Add a short delay
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

        _viewModel.IsPreMadeWorkout = title == Strings.PremadeWorkOuts;

        _viewModel.Title = title;
    }
}