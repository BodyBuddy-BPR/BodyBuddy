using BodyBuddy.ViewModels.StartupTest;

namespace BodyBuddy.Views.StartupTest;

public partial class StartupTestPage : ContentPage
{
    public StartupTestPage(StartupTestViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}