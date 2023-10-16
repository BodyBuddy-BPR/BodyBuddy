using BodyBuddy.ViewModels.StartupTest;

namespace BodyBuddy.Views.StartupTest;

public partial class StartupTestPage : ContentPage
{
    private StartupTestViewModel _viewModel;
    public StartupTestPage(StartupTestViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }
}