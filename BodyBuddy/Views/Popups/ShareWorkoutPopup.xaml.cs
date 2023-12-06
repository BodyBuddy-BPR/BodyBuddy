using BodyBuddy.ViewModels.WorkoutViewModels;

namespace BodyBuddy.Views.Popups;

public partial class ShareWorkoutPopup
{
    private WorkoutDetailsViewModel _viewModel;

    public ShareWorkoutPopup(WorkoutDetailsViewModel workoutDetailsViewModel)
    {
        InitializeComponent();
        _viewModel = workoutDetailsViewModel;
        BindingContext = workoutDetailsViewModel;

        GenerateQrCode();
    }

    private void GenerateQrCode()
    {
        string qrCodeData = _viewModel.GenerateQrCodeData();

        QRcode.Barcode = qrCodeData;
    }

}