using BodyBuddy.Dtos;
using BodyBuddy.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.Authentication;
using BodyBuddy.Views.Authentication;
using CommunityToolkit.Mvvm.Input;
using Supabase;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;


namespace BodyBuddy.ViewModels.Profile
{
    public partial class ProfileViewModel : BaseViewModel
    {
        private IStartupTestService _startupTestService;

        private IUserAuthenticationService _userAuthenticationService;

        #region ObservableProperties

        [ObservableProperty]
        private StartupTestDto _startupTestDto;

        [ObservableProperty]
        public bool isLoggedIn;

        #endregion
        

        public ProfileViewModel(IStartupTestService startupTestService, IUserAuthenticationService userAuthenticationService)
        {
            _startupTestService = startupTestService;
            _userAuthenticationService = userAuthenticationService;
        }

        public async Task Initialize()
        {
            StartupTestDto = await _startupTestService.GetStartupTestData();

            IsLoggedIn = _userAuthenticationService.IsUserLoggedIn();
        }

        #region Login / Logout

        public async Task LogIn()
        {
            await Shell.Current.GoToAsync($"{nameof(LoginPage)}", true, new Dictionary<string, object>
            {
                { "SkipVisible", false }
            });
        }

        public async Task LogOut()
        {
            var loggedOut = await _userAuthenticationService.SignUserOut();
            if (loggedOut)
            {
                await Shell.Current.GoToAsync($"//{nameof(MainPage)}", true);
                await MakeToast("You have logged out");
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
