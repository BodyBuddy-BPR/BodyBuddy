using BodyBuddy.ViewModels.WorkoutViewModels;
using Mopups.Interfaces;
using Mopups.Services;

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
        await _viewModel.GetWorkoutPlans();
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        await _viewModel.GetWorkoutPlans();
    }

    private void ClickToShowPopup_Clicked(object sender, EventArgs e)
    {
        //popup.Show();
        _popupNavigation.PushAsync(new CreateWorkoutPage(_viewModel));
    }

}