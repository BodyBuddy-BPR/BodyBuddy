using BodyBuddy.Helpers;
using BodyBuddy.ViewModels.WorkoutViewModels;
using Mopups.Interfaces;
using System.Text.RegularExpressions;

namespace BodyBuddy.Views.WorkoutViews;

public partial class WorkoutsPage : ContentPage
{
    private WorkoutViewModel _viewModel;
    IPopupNavigation _popupNavigation;

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
        string title = Shell.Current.CurrentItem?.CurrentItem?.CurrentItem?.Title;
        if (title == Strings.PremadeWorkOuts)
            _viewModel.IsPreMadeWorkout = true;
        else
            _viewModel.IsPreMadeWorkout = false;
        _viewModel.Title = title;

        await _viewModel.GetWorkoutPlans();
    }

    private void ClickToShowPopup_Clicked(object sender, EventArgs e)
    {
        //popup.Show();
        _popupNavigation.PushAsync(new CreateWorkoutPage(_viewModel));
    }


}