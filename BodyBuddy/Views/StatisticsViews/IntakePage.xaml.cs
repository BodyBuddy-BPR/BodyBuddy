using BodyBuddy.Controls.Common;
using BodyBuddy.ViewModels.IntakeViewModels;

namespace BodyBuddy.Views.StatisticsViews;

public partial class IntakePage : ContentPage
{
	private readonly IntakeViewModel _viewModel;
	public IntakePage(IntakeViewModel intakeViewModel)
	{
		InitializeComponent();
		_viewModel = intakeViewModel;
		BindingContext = intakeViewModel;

        // Adding Common Toolbar items
        CommonToolBarItems.AddCommonToolbarItems(this);
    }

	protected override async void OnAppearing()
	{
		base.OnAppearing();
        await Task.Delay(100);

        await _viewModel.Initialize();
	}
}