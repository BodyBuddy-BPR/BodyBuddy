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
using CommunityToolkit.Mvvm.Input;
using Mopups.Interfaces;
using BodyBuddy.Views.Popups;
using System.Collections.ObjectModel;

namespace BodyBuddy.ViewModels
{
    public partial class MainPageViewModel : BaseViewModel
    {
        private readonly IQuoteService _quoteService;
        private readonly IUserAuthenticationService _userAuthService;
        private readonly IStepService _stepService;
        private readonly IPopupNavigation _popupNavigation;

        [ObservableProperty]
        private StepDto _userSteps;

        [ObservableProperty] private ObservableCollection<UserTotalSteps> _friendsSteps = new();

        [ObservableProperty]
        public int _stepProgress;

        [ObservableProperty]
        private QuoteDto _quote;

        [ObservableProperty]
        private string _errorMessage;

        [ObservableProperty]
        private int _newStepGoal;

        private readonly string _skipLoginKey = "SkipLogInKey";
        private const double StepThreshold = 1.5;
        private bool isStepInProgress;

        public MainPageViewModel(IStepService stepService, IQuoteService quoteService, IUserAuthenticationService userAuthService, IPopupNavigation popupNavigation)
        {
            _stepService = stepService;
            _quoteService = quoteService;
            _userAuthService = userAuthService;
            _popupNavigation = popupNavigation;
        }

        public async Task Initialize()
        {
            UserSteps = await _stepService.GetStepDataTodayAsync();
            StepProgress = UserSteps.Steps == 0 ? 0 : UserSteps.Steps / UserSteps.StepGoal;
            await GetDailyQuote();
            TurnOnAccelerometer();
        }

        public async Task GetFriendsSteps()
        {
            FriendsSteps = new ObservableCollection<UserTotalSteps>(await _stepService.GetStepsForPeriodFriends());
            FriendsSteps.Add(new UserTotalSteps(){TotalSteps = UserSteps.Steps, User = new UserModel() {Email = "You"}});

            var sortedSteps = FriendsSteps.OrderByDescending(u => u.TotalSteps).ToList();
            FriendsSteps = new ObservableCollection<UserTotalSteps>(sortedSteps);
        }

        public void TurnOnAccelerometer()
        {
            if (!Accelerometer.Default.IsSupported) return;
            if (Accelerometer.Default.IsMonitoring) return;
            Accelerometer.Default.ReadingChanged += Accelerometer_ReadingChanged;
            Accelerometer.Default.Start(SensorSpeed.UI);
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

                    UserSteps.Steps++;
                    _stepService.SaveStepData(UserSteps);
                    StepProgress = UserSteps.Steps / UserSteps.StepGoal;
                }
            }
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

        public async Task<bool> SaveNewStepGoalValue()
        {

            if (NewStepGoal <= 0)
            {
                ErrorMessage = "New step goal cannot be negative or zero.";
                return false;
            }

            UserSteps.StepGoal = NewStepGoal;
            StepProgress = UserSteps.Steps / UserSteps.StepGoal;

            await _stepService.SaveStepData(UserSteps);

            ErrorMessage = string.Empty;

            return true;
        }

        public void ClickToShowPopup_Clicked()
        {
            _popupNavigation.PushAsync(new EditStepGoalPopup(this));
        }

        [RelayCommand]
        public void DeclineEditStepGoal()
        {
            ErrorMessage = string.Empty;
        }
    }
}
