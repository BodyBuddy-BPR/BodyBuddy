using BodyBuddy.ViewModels.IntakeViewmodels;
using CommunityToolkit.Mvvm.Input;

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
		await _viewModel.GetIntakeGoals();
	}
}