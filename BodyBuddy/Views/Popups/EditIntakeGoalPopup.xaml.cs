using BodyBuddy.ViewModels.IntakeViewModels;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Mopups.Services;

namespace BodyBuddy.Views.Popups;

public partial class EditIntakeGoalPopup
{
    private IntakeViewModel _viewModel;
    private string _intakeType;

    public EditIntakeGoalPopup(IntakeViewModel intakeViewModel, string intakeType)
	{
		InitializeComponent();
        _viewModel = intakeViewModel;
		_intakeType = intakeType;
        BindingContext = intakeViewModel;
    }

	private async void SaveBtn_Clicked(object sender, EventArgs e)
	{
		var valid = await _viewModel.SaveNewIntakeValues(_intakeType);

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

	private async void CancelBtn_Clicked(object sender, EventArgs e)
	{
			await MopupService.Instance.PopAsync();
	}

}