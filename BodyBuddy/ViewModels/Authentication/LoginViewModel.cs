using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Supabase;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Supabase.Gotrue.Constants;

namespace BodyBuddy.ViewModels.Authentication
{
    public partial class LoginViewModel : BaseViewModel
    {
        private Client _supabase;

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

        public LoginViewModel(Client client)
        {
            _supabase = client;
        }

        #region Sign In

        public async Task Login()
        {
            //var session = await _supabase.Auth.SignUp(LoginEmail, LoginPassword);
            try
            {
                var session = await _supabase.Auth.SignIn(LoginEmail, LoginPassword);
                await MakeToast("Succesfully signed in.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error!", $"Unable to login {ex.Message}", "OK");
            }
        }


        [RelayCommand] // Currently this does not work
        public async Task LoginUsingThirdParty(string passedProvider)
        {
            if (string.IsNullOrEmpty(passedProvider)) return;

            // Convert the string to the corresponding Provider enum value
            if (Enum.TryParse<Provider>(passedProvider, out var provider))
            {
                //var signInUrl = await _supabase.Auth.SignIn(provider);
            }
            else
            {
                return;
            }
        }

        #endregion

        #region Sign Up

        public async Task SignUp()
        {
            try
            {
                var session = await _supabase.Auth.SignUp(SignUpEmail, SignUpPassword);
                await MakeToast("Congratulations! You have succesfully signed up. Now sign in");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error!", $"Unable to create user {ex.Message}", "OK");
            }
        }

        #endregion

        private async Task MakeToast(string displayText)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            string text = displayText;
            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;

            var toast = Toast.Make(text, duration, fontSize);

            await toast.Show(cancellationTokenSource.Token);
        }

    }
}
