using BodyBuddy.ViewModels.StartupTest;

namespace BodyBuddy.Views.StartupTest;

public partial class StartupTestPage : ContentPage
{
    private StartupTestViewModel _viewModel;
    public StartupTestPage(StartupTestViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
		_viewModel = viewModel;
	}


	private void OnEditorCompleted(object sender, EventArgs e)
	{
		NameEditor.Unfocus();
	}

	private void TargetAreaChangedEvent(object sender, SelectionChangedEventArgs e)
	{
		_viewModel.TargetAreaChangedEvent(e.CurrentSelection.ToList());
	}	
	
	private void CheckBox_StateChanged(object sender, Syncfusion.Maui.Buttons.StateChangedEventArgs e)
	{
        bool[] values = new bool[4] { UpperBody.IsChecked.Value, LowerBody.IsChecked.Value, Abs.IsChecked.Value, Back.IsChecked.Value };
        _viewModel.CheckBoxStateChange(values);
    }

}