using Mopups.Services;
using BodyBuddy.ViewModels.WorkoutViewModels;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Alerts;
using System.Diagnostics;
using Camera.MAUI.ZXingHelper;

namespace BodyBuddy.Views.WorkoutViews;

public partial class CreateWorkoutPage
{
    private WorkoutViewModel _viewModel;

    public CreateWorkoutPage(WorkoutViewModel workoutsViewModel)
    {
        InitializeComponent();
        _viewModel = workoutsViewModel;
        BindingContext = workoutsViewModel;

        // Disabling views
        cameraPopupView.IsVisible = false;
        codeScannedPopupView.IsVisible = false;
    }

    private async void CreateBtn_Clicked(object sender, EventArgs e)
    {
        var valid = await _viewModel.CreateWorkout();

        if (valid)
        {
            await MopupService.Instance.PopAsync();

            _viewModel.WorkoutName = string.Empty;
            _viewModel.WorkoutDescription = string.Empty;
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

    private async void CreateFromCodeBtn_Clicked(object sender, EventArgs e)
    {
        var valid = await _viewModel.CreateWorkout();

        if (valid)
        {
            await _viewModel.AddExercisesToWorkout();

            await MopupService.Instance.PopAsync();

            _viewModel.WorkoutName = string.Empty;
            _viewModel.WorkoutDescription = string.Empty;
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

    private async void ScanCodeBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Disabling/Enabling the correct views of the popup
            normalPopupView.IsVisible = false; // Disable the entire normal view
            popupBorder.HeightRequest = 375; // Set a new height of the popup
            cameraPopupView.IsVisible = true; // Make Camera visible


            // Subscribing to HandleBarCodeDetected
            cameraView.BarcodeDetected += HandleBarCodeDetected;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error setting up barcode detection: {ex.Message}");
        }
    }

    private async void HandleBarCodeDetected(object sender, BarcodeEventArgs args)
    {
        // Process the scanned QR code data
        _viewModel.ReadQrCodeData(args.Result[0].Text);

        // Delay execution for a short period
        MainThread.BeginInvokeOnMainThread(() =>
        {
            // Disabling Camera view
            cameraPopupView.IsVisible = false;
            popupBorder.HeightRequest = 235; // Set a new height of the popup
            codeScannedPopupView.IsVisible = true;

        });
        cameraView.BarcodeDetected -= HandleBarCodeDetected;
    }


    private void cameraView_CamerasLoaded(object sender, EventArgs e)
    {
        if (cameraView.Cameras.Count > 0)
        {
            cameraView.Camera = cameraView.Cameras.First();

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await cameraView.StopCameraAsync();
                await cameraView.StartCameraAsync();
            });
        }
    }

}