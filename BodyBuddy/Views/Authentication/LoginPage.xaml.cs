using BodyBuddy.ViewModels.Authentication;
using Mopups.Interfaces;

namespace BodyBuddy.Views.Authentication;

public partial class LoginPage
{
    private LoginViewModel _viewModel;
    public LoginPage(LoginViewModel loginViewModel)
	{
		InitializeComponent();
        _viewModel = loginViewModel;
        BindingContext = loginViewModel;
	}

    private void SignUp_Clicked(object sender, EventArgs e)
    {
        _viewModel.IsLogin = false;
        _viewModel.IsSignUp = true;
    }

    private void Login_Clicked(object sender, EventArgs e)
    {
        _viewModel.IsLogin = true;
        _viewModel.IsSignUp = false;
    }
}