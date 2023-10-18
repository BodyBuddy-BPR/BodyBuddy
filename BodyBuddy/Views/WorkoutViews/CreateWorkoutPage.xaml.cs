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

    private bool cameraEnabled = false;

    public CreateWorkoutPage(WorkoutViewModel workoutsViewModel)
    {
        InitializeComponent();
        _viewModel = workoutsViewModel;
        BindingContext = workoutsViewModel;

        cameraView.IsEnabled = false;
        cameraView.IsVisible = false;
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

    private async void ScanCodeBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            cameraView.IsEnabled = true;
            cameraView.IsVisible = true;

            // Assuming you have a CameraView named "cameraView" in your XAML
            cameraView.BarcodeDetected += HandleBarCodeDetected;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error setting up barcode detection: {ex.Message}");
        }
    }

    private void HandleBarCodeDetected(object sender, BarcodeEventArgs args)
    {
        if (!string.IsNullOrWhiteSpace(args.ToString()))
        {
            // Assuming you have a method to process the scanned QR code data
            _viewModel.ReadQrCodeData(args.Result.ToString());

            // If you only want to process the first detected barcode, you can unsubscribe
            cameraView.BarcodeDetected -= HandleBarCodeDetected;
        }
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