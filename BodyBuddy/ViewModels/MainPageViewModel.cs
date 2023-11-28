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
using BodyBuddy.Models;
using BodyBuddy.Services.Implementations;

namespace BodyBuddy.ViewModels
{
    public partial class MainPageViewModel : BaseViewModel
    {
        private readonly IQuoteService _quoteService;
        private IUserAuthenticationService _userAuthService;
        private readonly IStepService _stepService;

        [ObservableProperty]
        private StepDto _userSteps;

        [ObservableProperty]
        private string _acceleration;

        [ObservableProperty]
        private string _magnitude;

        [ObservableProperty]
        private QuoteDto _quote;

        private readonly string _skipLoginKey = "SkipLogInKey";
        private const double StepThreshold = 2;
        private bool isStepInProgress;

        public MainPageViewModel(IStepService stepService, IQuoteService quoteService, IUserAuthenticationService userAuthService)
        {
            _stepService = stepService;
            _quoteService = quoteService;
            _userAuthService = userAuthService;
        }

        public async Task Initialize()
        {
            UserSteps = await _stepService.GetStepData();
            await GetDailyQuote();
            ToggleAccelerometer();
        }

        public void ToggleAccelerometer()
        {
            if (Accelerometer.Default.IsSupported)
            {
                if (!Accelerometer.Default.IsMonitoring)
                {
                    Accelerometer.Default.ReadingChanged += Accelerometer_ReadingChanged;
                    Accelerometer.Default.Start(SensorSpeed.UI);
                }
                //else
                //{
                //    Accelerometer.Default.Stop();
                //    Accelerometer.Default.ReadingChanged -= Accelerometer_ReadingChanged;
                //}
            }
        }

        private void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            var acceleration = e.Reading.Acceleration;

            // Calculate the magnitude of the acceleration vector
            var magnitude = Math.Sqrt(acceleration.X * acceleration.X + acceleration.Y * acceleration.Y + acceleration.Z * acceleration.Z);

            // Check if a step is in progress
            if (isStepInProgress)
            {
                if (magnitude < StepThreshold)
                {
                    // Step is complete
                    isStepInProgress = false;
                }
            }
            else
            {
                if (magnitude > StepThreshold)
                {
                    // Start of a step
                    isStepInProgress = true;

                    // Assuming that the user has their phone in their right or left pocket
                    // a step will only be counted when that leg takes a step.
                    // Therefore, we add two to the stepcount each time a step is registered.
                    UserSteps.Steps = UserSteps.Steps + 2;
                    _stepService.SaveStepData(UserSteps);
                }
            }
            Acceleration = $"Acceleration reading: {e.Reading}";
            Magnitude = $"Magnitude: {magnitude}";
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

                //var navigationParams = new Dictionary<string, object>
                //{
                //    { "SkipVisible", true }
                //};

                //var navigationPage = new NavigationPage(new LoginPage(new LoginViewModel(_userAuthService)))
                //{
                //    BarBackgroundColor = Color.Transparent,
                //    BarTextColor = Color.White
                //};

                //await Shell.Current.Navigation.PushModalAsync(navigationPage, true);

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
