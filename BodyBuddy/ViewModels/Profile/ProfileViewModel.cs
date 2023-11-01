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
using Syncfusion.Maui.GridCommon.Collections;
using System.Runtime.CompilerServices;


namespace BodyBuddy.ViewModels.Profile
{
    public partial class ProfileViewModel : BaseViewModel
    {
        private IStartupTestService _startupTestService;
        private IIntakeService _intakeService;
        private readonly IUserAuthenticationService _userAuthenticationService;

		private static int secondsInADay = 86400;
		private int currentDayOfWeek;
		private int currentAndSelectedDayDifference;

		#region ObservableProperties

		[ObservableProperty]
        private StartupTestDto _startupTestDto;         
        [ObservableProperty]
        private IntakeDto _userIntakeForDate; 
        [ObservableProperty]
		private string _bMI;
        [ObservableProperty]
		private int _currentSelectedDate;

		[ObservableProperty]
        public bool _isLoggedIn, _isMondaySelected, _isTuesdaySelected, _isWednesdaySelected, _isThursdaySelected, _isFridaySelected, _isSaturdaySelected, _isSundaySelected;

        #endregion
        

        public ProfileViewModel(IStartupTestService startupTestService, IIntakeService intakeService, IUserAuthenticationService userAuthenticationService)
        {
            _startupTestService = startupTestService;
            _intakeService = intakeService;
            _userAuthenticationService = userAuthenticationService;
        }

        public async Task Initialize()
		{
			StartupTestDto = await _startupTestService.GetStartupTestData();

			UserIntakeForDate = await _intakeService.GetIntakeForDateAsync((int)(DateTime.UtcNow.Date.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);

			CalculateUsersBMI();

			IsLoggedIn = _userAuthenticationService.IsUserLoggedIn();

			GetCurrentWeekDay();
		}

		private void CalculateUsersBMI()
		{
			double BMIDouble = (StartupTestDto.Weight / ((double)StartupTestDto.Height / 100 * (double)StartupTestDto.Height / 100));

			BMI = string.Format("{0:F1}", BMIDouble);
		}
		private async Task MakeToast(string displayText)
		{
			CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

			string text = displayText;
			ToastDuration duration = ToastDuration.Short;
			double fontSize = 14;

			var toast = Toast.Make(text, duration, fontSize);

			await toast.Show(cancellationTokenSource.Token);
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

		#region Weekday button methods

		private void GetCurrentWeekDay()
		{
			int dayOfWeek = (int)DateTime.UtcNow.DayOfWeek;

			currentDayOfWeek = dayOfWeek;
        private async Task MakeToast(string displayText)
        {
            CancellationTokenSource cancellationTokenSource = new();

			CurrentSelectedDate = currentDayOfWeek;

			SetSelectedWeekday();
		}
		private void SetSelectedWeekday()
		{
			ResetWeekdayBooleans();
			switch (CurrentSelectedDate)
			{
				case 1:
					IsMondaySelected = true; break;
				case 2:
					IsTuesdaySelected = true; break;
				case 3:
					IsWednesdaySelected = true; break;
				case 4:
					IsThursdaySelected = true; break;
				case 5:
					IsFridaySelected = true; break;
				case 6:
					IsSaturdaySelected = true; break;
				case 7:
					IsSundaySelected = true; break;
			}
		}
		private void ResetWeekdayBooleans()
		{
			IsMondaySelected = false;
			IsTuesdaySelected = false;
			IsWednesdaySelected = false;
			IsThursdaySelected = false;
			IsFridaySelected = false;
			IsSaturdaySelected = false;
			IsSundaySelected = false;
		}

		public async Task WeekdayButtonClicked(int buttonNumber)
        {
			CurrentSelectedDate = buttonNumber;
            SetSelectedWeekday();
            currentAndSelectedDayDifference = CurrentSelectedDate - currentDayOfWeek;
			int currentDateTimestamp = (int)(DateTime.UtcNow.Date.Subtract((new DateTime(1970, 1, 1))).TotalSeconds);
			UserIntakeForDate = await _intakeService.GetIntakeForDateAsync(currentDateTimestamp - (currentAndSelectedDayDifference * 86400));
		}
		#endregion
	}
}
