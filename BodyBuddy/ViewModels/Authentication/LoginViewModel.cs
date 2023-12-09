using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using BodyBuddy.Authentication;
using BodyBuddy.Services;
using BodyBuddy.Views.StartupTest;

namespace BodyBuddy.ViewModels.Authentication
{
    [QueryProperty(nameof(SkipVisible), "SkipVisible")]
    public partial class LoginViewModel : BaseViewModel
    {
        private readonly IUserAuthenticationService _userAuthenticationService;
        private readonly ILoginDatabaseFlowService _loginDatabaseFlowService;
        private readonly IStartupTestService _startupTestService;

        [ObservableProperty]
        public bool isLogin = true;
        [ObservableProperty]
        public bool isSignUp = false;

        [ObservableProperty]
        public string loginEmail, loginPassword;

        [ObservableProperty]
        public string signUpEmail, signUpPassword, signUpErrorMessage;
        [ObservableProperty]
        public bool signUpErrorVisible = false;

        [ObservableProperty]
        public bool skipVisible = false;

        private readonly string _skipLoginKey = "SkipLogInKey";
        private readonly string _runStartupTest = "StartupTest";

        public LoginViewModel(IUserAuthenticationService userAuthenticationService, ILoginDatabaseFlowService loginDatabaseFlowService, IStartupTestService startupTestService)
        {
            _userAuthenticationService = userAuthenticationService;
            _loginDatabaseFlowService = loginDatabaseFlowService;
            _startupTestService = startupTestService;
        }

        public async Task Initialize()
        {
            // Does nothing currently
        }

        #region Sign In

        public async Task Login()
        {
            try
            {
                var success = await _userAuthenticationService.SignUserIn(LoginEmail, LoginPassword);
                var answer = false;

                if (success)
                {
                    var startupTest = await _startupTestService.GetStartupTestData();
                    if (startupTest.Id != 0)
                    {
                        answer = await Shell.Current.DisplayAlert("Startup test data", "Use current startup test data", "Yes", "No");
                    }

                    //Starting flow to swap DB to new data (Giv en popup her! --> Overwrite existing data, this may result in loss of Supabase information)
                    await _loginDatabaseFlowService.StartLoginDatabaseFlow(answer);

                    await MakeToast("successfully signed in.");

                    await Navigate();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Login Invalid", "Email or password is incorrect ", "OK");
            }
        }

        [RelayCommand]
        public async Task SkipLogin()
        {
            Preferences.Set(_skipLoginKey, true);
            await Navigate();
        }

        //[RelayCommand] // Currently this does not work
        //public async Task LoginUsingThirdParty(string passedProvider)
        //{
        //    if (string.IsNullOrEmpty(passedProvider)) return;

        //    // Convert the string to the corresponding Provider enum value
        //    if (Enum.TryParse<Provider>(passedProvider, out var provider))
        //    {
        //        //var signInUrl = await _supabase.Auth.SignIn(provider);
        //    }
        //    else
        //    {
        //        return;
        //    }
        //}

        #endregion

        #region Sign Up

        public async Task SignUp()
        {
            try
            {
                var success = await _userAuthenticationService.SignUserUp(SignUpEmail, SignUpPassword);

                if (success)
                {
                    await MakeToast("Congratulations! You have successfully signed up");

                    await Navigate();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error!", $"Unable to create user {ex.Message}", "OK");
            }
        }

        #endregion

        public async Task Navigate()
        {
            var runTest = Preferences.Get(_runStartupTest, true);
            if (runTest)
            {
                await Shell.Current.GoToAsync($"{nameof(StartupTestPage)}", true);
            }
            else
            {
                await Shell.Current.Navigation.PopAsync();
                await Shell.Current.GoToAsync($"//{nameof(MainPage)}", true);
            }
        }


        private async Task MakeToast(string displayText)
        {
            CancellationTokenSource cancellationTokenSource = new();

            string text = displayText;
            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;

            var toast = Toast.Make(text, duration, fontSize);

            await toast.Show(cancellationTokenSource.Token);
        }

    }
}
