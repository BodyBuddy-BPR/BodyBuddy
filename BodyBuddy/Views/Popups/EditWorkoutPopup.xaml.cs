using BodyBuddy.ViewModels.WorkoutViewModels;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Mopups.Services;

namespace BodyBuddy.Views.Popups;

public partial class EditWorkoutPopup
{
    private readonly WorkoutDetailsViewModel _viewModel;

    public EditWorkoutPopup(WorkoutDetailsViewModel workoutDetailsViewModel)
	{
		InitializeComponent();
        _viewModel = workoutDetailsViewModel;
        BindingContext = workoutDetailsViewModel;
    }

    private async void SaveBtn_Clicked(object sender, EventArgs e)
    {
        var valid = await _viewModel.SaveWorkout();

        if (valid)
        {
            await MopupService.Instance.PopAsync();
        }
        else
        {
            CancellationTokenSource cancellationTokenSource = new();

            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;

            var toast = Toast.Make(_viewModel.ErrorMessage, duration, fontSize);
            await toast.Show(cancellationTokenSource.Token);
        }
    }
}