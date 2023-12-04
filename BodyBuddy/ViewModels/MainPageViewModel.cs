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
using BodyBuddy.Views.WorkoutViews;

namespace BodyBuddy.ViewModels
{
    public partial class MainPageViewModel : BaseViewModel
    {
        private readonly IQuoteService _quoteService;
        private readonly IStartupTestService _startupTestService;
        private IUserAuthenticationService _userAuthService;
        private readonly IStepService _stepService;
        private readonly IPopupNavigation _popupNavigation;
        private readonly IWorkoutService _workoutService;

        private StartupTestDto startupTestDto;

        [ObservableProperty]
        private StepDto _userSteps;

        [ObservableProperty]
        public double _stepProgress;

        [ObservableProperty]
        private QuoteDto _quote;

        [ObservableProperty]
        private string _errorMessage;

        [ObservableProperty]
        private int _newStepGoal;

        [ObservableProperty]
        private ObservableCollection<string> _targetAreas;

        [ObservableProperty]
        private List<WorkoutDto> _workoutsToShow;

        [ObservableProperty]
        private List<WorkoutDto> _allWorkouts;

        private readonly string _skipLoginKey = "SkipLogInKey";
        private const double StepThreshold = 1.5;
        private bool isStepInProgress;

        public MainPageViewModel(IStepService stepService, IQuoteService quoteService, IUserAuthenticationService userAuthService, IPopupNavigation popupNavigation,
            IStartupTestService startupTestService, IWorkoutService workoutService)
        {
            _stepService = stepService;
            _quoteService = quoteService;
            _userAuthService = userAuthService;
            _popupNavigation = popupNavigation;
            _startupTestService = startupTestService;
            _workoutService = workoutService;
        }

        public async Task Initialize()
        {
            UserSteps = await _stepService.GetStepData();
            StepProgress = UserSteps.Steps == 0 ? 0 : (double)UserSteps.Steps / UserSteps.StepGoal;
            await GetDailyQuote();
            await SetWorkoutsToShow();
            await TurnOnAccelerometer();
        }

        public async Task SetWorkoutsToShow()
        {
            startupTestDto = await _startupTestService.GetStartupTestData();

            if (!string.IsNullOrEmpty(startupTestDto.TargetAreas))
            {
                var targetAreaStrings = startupTestDto.TargetAreas.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                TargetAreas = new ObservableCollection<string>(targetAreaStrings);
            }

            AllWorkouts = await _workoutService.GetWorkoutPlans(true);

            if (TargetAreas != null && TargetAreas.Count == 1 && TargetAreas[0].Equals("Abs", StringComparison.OrdinalIgnoreCase))
            {
                TargetAreas[0] = "Ab";
            }
            else if (TargetAreas != null && TargetAreas.Count > 0)
            {
                WorkoutsToShow = AllWorkouts
                    .Where(x => x.Name.ToLowerInvariant().Contains(TargetAreas.FirstOrDefault()?.ToLowerInvariant()))
                    .ToList();
            }
            else
            {
                TargetAreas = new ObservableCollection<string>
                {
                    "Upperbody",
                    "Lowerbody",
                    "Abs",
                    "Back"
                };

                WorkoutsToShow = AllWorkouts
                    .Where(x => x.Name.ToLowerInvariant().Contains(TargetAreas.FirstOrDefault()?.ToLowerInvariant()))
                    .ToList();
            }
        }

        #region Accelerometer

        public async Task TurnOnAccelerometer()
        {
            if (Accelerometer.Default.IsSupported)
            {
                if (!Accelerometer.Default.IsMonitoring)
                {
                    Accelerometer.Default.ReadingChanged += Accelerometer_ReadingChanged;
                    Accelerometer.Default.Start(SensorSpeed.UI);
                }
            }
        }

        private async void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
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
                    await _stepService.SaveStepData(UserSteps);
                    StepProgress = (double)UserSteps.Steps / (double)UserSteps.StepGoal;
                }
            }
        }

        #endregion

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
            StepProgress = (double)UserSteps.Steps / (double)UserSteps.StepGoal;

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

        public void UpdateWorkoutsToShow(object dataItem)
        {
            string searchTerm = dataItem?.ToString()?.ToLowerInvariant();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                // Remove the 's' if the search term is "abs"
                if (searchTerm == "abs")
                {
                    searchTerm = searchTerm.TrimEnd('s');
                }

                WorkoutsToShow = AllWorkouts
                    .Where(x => x.Name.ToLowerInvariant().Contains(searchTerm))
                    .ToList();
            }
            else
            {
                WorkoutsToShow = AllWorkouts.ToList();
            }
        }


        #region Navigation

        [RelayCommand]
        public async Task GoToWorkoutDetailsFromMainPage(WorkoutDto workout)
        {
            if (workout == null)
                return;

            await Shell.Current.GoToAsync(nameof(WorkoutDetailsPage), true, new Dictionary<string, object>
            {
                { "Workout", workout }
            });
        }
        #endregion
    }
}
