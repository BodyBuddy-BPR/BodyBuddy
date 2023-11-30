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

    // Method to let the user tap the text next to a checkbox, and it will still be selected.
    private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        var layout = sender as Layout;
        var check = layout.FirstOrDefault(c => c.GetType() == typeof(CheckBox));

        if (check != null)
        {
            ((CheckBox)check).IsChecked = !((CheckBox)check).IsChecked;
        }
    }

}