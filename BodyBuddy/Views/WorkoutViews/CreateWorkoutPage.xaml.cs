using Mopups.Services;
using BodyBuddy.ViewModels.WorkoutViewModels;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Alerts;
using System.Diagnostics;
using Camera.MAUI.ZXingHelper;

namespace BodyBuddy.Views.WorkoutViews;

public partial class CreateWorkoutPage
{
    private readonly WorkoutViewModel _viewModel;

    private bool isCameraStarted = false;

    public CreateWorkoutPage(WorkoutViewModel workoutsViewModel)
    {
        InitializeComponent();
        _viewModel = workoutsViewModel;
        BindingContext = workoutsViewModel;
    }

    // Normal Create Workout btn
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
            CancellationTokenSource cancellationTokenSource = new();

            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;

            var toast = Toast.Make(_viewModel.ErrorMessage, duration, fontSize);
            await toast.Show(cancellationTokenSource.Token);
        }
    }


    // When a QR Code has been scanned, this is the Create Workout btn
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
            CancellationTokenSource cancellationTokenSource = new();

            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;

            var toast = Toast.Make(_viewModel.ErrorMessage, duration, fontSize);
            await toast.Show(cancellationTokenSource.Token);
        }
    }

    // Opens the Camera Popup
    private async void OpenCameraBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Delay the initialization of the camera until the user clicks the button
            await Task.Delay(200);

            // Disabling/Enabling the correct views of the popup    
            normalPopupView.IsVisible = false; // Disable the entire normal view
            popupBorder.HeightRequest = 375; // Set a new height of the popup

            cameraPopupView.IsEnabled = true;
            cameraPopupView.IsVisible = true; // Make Camera visible

            // Initialize the camera after a short delay
            await Task.Delay(200);

            // Check if the camera is not already started
            if (!isCameraStarted)
            {
                // Subscribing to HandleBarCodeDetected
                cameraView.BarcodeDetected += HandleBarCodeDetected;

                // Initialize the camera
                await cameraView.StartCameraAsync();

                isCameraStarted = true;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error setting up barcode detection: {ex.Message}");
        }
    }

    // The event that happens when a QR Code is detected
    private async void HandleBarCodeDetected(object sender, BarcodeEventArgs args)
    {
        // Process the scanned QR code data
        _viewModel.ReadQrCodeData(args.Result[0].Text);

        MainThread.BeginInvokeOnMainThread(() =>
        {
            // Disabling Camera view
            cameraPopupView.IsVisible = false;
            cameraPopupView.IsEnabled = false;

            popupBorder.HeightRequest = 235; // Set a new height of the popup
            codeScannedPopupView.IsVisible = true;

        });
        cameraView.BarcodeDetected -= HandleBarCodeDetected;
    }



    // Loads the specidic camera on the phone to use
    private void cameraView_CamerasLoaded(object sender, EventArgs e)
    {
        if (cameraView.Cameras.Count > 0)
        {
            cameraView.Camera = cameraView.Cameras.First();
        }
    }
}