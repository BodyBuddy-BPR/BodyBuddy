using Mopups.Services;
using BodyBuddy.ViewModels.WorkoutViewModels;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Alerts;

namespace BodyBuddy.Views.WorkoutViews;

public partial class CreateWorkoutPage
{
    private WorkoutViewModel _viewModel;

    public CreateWorkoutPage(WorkoutViewModel workoutsViewModel)
    {
        InitializeComponent();
        _viewModel = workoutsViewModel;
        BindingContext = workoutsViewModel;
    }

    private async void CreateBtn_Clicked(object sender, EventArgs e)
    {
        var valid = await _viewModel.CreateWorkout();

        if (valid)
        {
            await MopupService.Instance.PopAsync();
        }
        else
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;

            var toast = Toast.Make(_viewModel.ErrorMessage, duration, fontSize);
            await toast.Show(cancellationTokenSource.Token);
        }
    }
    
}