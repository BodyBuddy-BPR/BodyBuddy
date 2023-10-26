using BodyBuddy.ViewModels.Authentication;


namespace BodyBuddy.Views.Authentication;

public partial class LoginPage : ContentPage
{
    private LoginViewModel _viewModel;
    public LoginPage(LoginViewModel loginViewModel)
    {
        InitializeComponent();
        _viewModel = loginViewModel;
        BindingContext = loginViewModel;

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.Initialize();
    }

    #region Sign In

    private async void SignIn_Clicked(object sender, EventArgs e)
    {
        await _viewModel.Login();
    }

    private void GoToSignUp_Clicked(object sender, EventArgs e)
    {
        _viewModel.IsLogin = false;
        _viewModel.IsSignUp = true;
    }

    #endregion

    #region Sign Up

    private async void SignUp_Clicked(object sender, EventArgs e)
    {
        if (!EmailValid.IsValid)
        {
            EmailError.Text = "Email is not valid";
            EmailError.IsVisible = true;

            // Start the timer to hide the error label after 3 seconds
            StartErrorLabelTimer(EmailError);
        }
        if (!PasswordValid.IsValid)
        {
            PasswordError.Text = PasswordValid.Errors.FirstOrDefault().ToString();
            PasswordError.IsVisible = true;

            // Start the timer to hide the error label after 3 seconds
            StartErrorLabelTimer(PasswordError);
        }
        else if (PasswordValid.IsValid && EmailValid.IsValid)
        {
            EmailError.IsVisible = false;
            PasswordError.IsVisible = false;

            await _viewModel.SignUp();
        }

    }

    private void GoToLogin_Clicked(object sender, EventArgs e)
    {
        _viewModel.IsLogin = true;
        _viewModel.IsSignUp = false;
    }

    #endregion

    private async void StartErrorLabelTimer(Label errorLabel)
    {
        // Wait for 3 seconds
        await Task.Delay(3000);

        // Hide the error label if it is still visible
        if (errorLabel.IsVisible)
        {
            errorLabel.IsVisible = false;
        }
    }



}