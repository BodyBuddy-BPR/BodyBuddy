using BodyBuddy.ViewModels.IntakeViewmodels;

namespace BodyBuddy.Views.StatisticsViews;

public partial class IntakePage : ContentPage
{
	private IntakeViewModel _viewModel;
	public IntakePage(IntakeViewModel intakeViewModel)
	{
		InitializeComponent();
		_viewModel = intakeViewModel;
		BindingContext = intakeViewModel;

	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await _viewModel.Intilialize();
	}
}