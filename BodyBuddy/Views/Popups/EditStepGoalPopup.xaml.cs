using BodyBuddy.ViewModels;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Mopups.Services;

namespace BodyBuddy.Views.Popups;

public partial class EditStepGoalPopup
{
    private readonly MainPageViewModel _viewModel;

    public EditStepGoalPopup(MainPageViewModel mainpageViewModel)
	{
		InitializeComponent();
        _viewModel = mainpageViewModel;
        BindingContext = mainpageViewModel;
    }

	private async void SaveBtn_Clicked(object sender, EventArgs e)
	{
		var valid = await _viewModel.SaveNewStepGoalValue();

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

	private async void CancelBtn_Clicked(object sender, EventArgs e)
	{
			await MopupService.Instance.PopAsync();
	}

}