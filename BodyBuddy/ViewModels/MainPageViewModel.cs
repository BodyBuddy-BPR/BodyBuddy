using BodyBuddy.Dtos;
using BodyBuddy.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.ViewModels.Authentication;
using BodyBuddy.Views.Authentication;
using BodyBuddy.Authentication;

namespace BodyBuddy.ViewModels
{
    public partial class MainPageViewModel : BaseViewModel
    {
        private readonly IQuoteService _quoteService;
        private IUserAuthenticationService _userAuthService;

        [ObservableProperty]
        public QuoteDto _quote;

        private readonly string _skipLoginKey = "SkipLogInKey";

        public MainPageViewModel(IQuoteService quoteService, IUserAuthenticationService userAuthService)
        {
            _quoteService = quoteService;
            _userAuthService = userAuthService;
        }

        public async Task Initialize()
        {
            await GetDailyQuote();
        }

        public async Task AttemptToLogin()
        {

            if (_userAuthService.IsUserLoggedIn()) return;

            var isLoginSkipped = Preferences.Get(_skipLoginKey, false);

            // Attempt auto-login
            var autoLoginSuccess = await _userAuthService.TryAutoSignIn();

            if (!autoLoginSuccess && !isLoginSkipped)
            {
                await Shell.Current.GoToAsync($"{nameof(LoginPage)}", true, new Dictionary<string, object>
                {
                    { "SkipVisible", true }
                });
                //await Shell.Current.Navigation.PushModalAsync(new LoginPage(new LoginViewModel(null, null)));

            }
        }

        public async Task GetDailyQuote()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                Quote = await _quoteService.GetDailyQuote();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error!", $"Unable to get daily quote {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
