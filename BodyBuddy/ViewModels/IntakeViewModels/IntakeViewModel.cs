using BodyBuddy.Models;
using BodyBuddy.Repositories;
using BodyBuddy.Views.Popups;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mopups.Interfaces;
using System.Diagnostics;
using BodyBuddy.Dtos;
using BodyBuddy.Services;
using System.ComponentModel;

namespace BodyBuddy.ViewModels.IntakeViewModels
{
	public partial class IntakeViewModel : BaseViewModel
	{
		private readonly IIntakeService _intakeService;
        private readonly IPopupNavigation _popupNavigation;

		[ObservableProperty]
		private IntakeDto _intakeDto;
		[ObservableProperty]
		private string _errorMessage;
		[ObservableProperty]
		private int _calorieEntryText, _newIntakeGoal, _newCurrentIntake;

        //public double WaterProgress => IntakeDto.WaterGoal != 0 ? (double)IntakeDto.WaterCurrent / IntakeDto.WaterGoal : 0;
        //public double CalorieProgress => IntakeDto.CalorieGoal != 0 ? (double)IntakeDto.CalorieCurrent / IntakeDto.CalorieGoal : 0;

        public IntakeViewModel(IIntakeService intakeService, IPopupNavigation popupNavigation)
		{
			_intakeService = intakeService;
			_popupNavigation = popupNavigation;

            //IntakeDto.PropertyChanged += IntakeDtoPropertyChanged;
        }

        //private void IntakeDtoPropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    switch (e.PropertyName)
        //    {
        //        case nameof(IntakeDto.WaterCurrent) or nameof(IntakeDto.WaterGoal):
        //            OnPropertyChanged(nameof(WaterProgress));
        //            break;
        //        case nameof(IntakeDto.CalorieCurrent) or nameof(IntakeDto.CalorieGoal):
        //            OnPropertyChanged(nameof(CalorieProgress));
        //            break;
        //    }
        //}

        public async Task Initialize()
		{
			await GetIntakeGoals();
		}

		[RelayCommand]
		public async Task GetIntakeGoals()
		{
			if (IsBusy) return;

			try
			{
				IsBusy = true;

				var intake = await _intakeService.GetIntakeAsync();
				if (intake != null)
				{
					IntakeDto = intake;
                }
                else
				{
					await Shell.Current.DisplayAlert("Error!", "Intake is null", "OK");

				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
				await Shell.Current.DisplayAlert("Error!", $"Unable to get intake {ex.Message}", "OK");
			}
			finally
			{
				IsBusy = false;
			}
		}

		[RelayCommand]
		public async Task AddWaterClicked()
		{
            IntakeDto.WaterCurrent += 250;
			
			await _intakeService.SaveChangesAsync(IntakeDto);
		}

		[RelayCommand]
		public async Task AddKcalClicked(int calories)
		{
			if(IntakeDto.CalorieCurrent + calories < 0)
			{
				ErrorMessage = "Cannot reduce current calorie intake below zero.";

				CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

				ToastDuration duration = ToastDuration.Short;
				double fontSize = 14;

				var toast = Toast.Make(ErrorMessage, duration, fontSize);
				await toast.Show(cancellationTokenSource.Token);
			}
			else
			{
                IntakeDto.CalorieCurrent += calories;

				await _intakeService.SaveChangesAsync(IntakeDto);
			}
		}

        #region popup methods

		public async Task<bool> SaveNewIntakeValues(string intakeType)
        {
            if (NewCurrentIntake < 0)
			{
				ErrorMessage = "Current intake cannot be negative.";
				return false;
			}

            if (NewIntakeGoal <= 0)
            {
                ErrorMessage = "New intake goal cannot be negative or zero.";
                return false;
            }

            if (intakeType == "Calorie")
            {
                IntakeDto.CalorieGoal = NewIntakeGoal;
                IntakeDto.CalorieCurrent = NewCurrentIntake;
            }
            else
            {
                IntakeDto.WaterGoal = NewIntakeGoal;
                IntakeDto.WaterCurrent = NewCurrentIntake;
            }

            await _intakeService.SaveChangesAsync(IntakeDto);

            ErrorMessage = string.Empty;

            return true;
        }

		[RelayCommand]
		public void DeclineEditIntake()
		{
			ErrorMessage = string.Empty;
		}

		[RelayCommand]
		private void ClickToShowPopup_Clicked(string intakeType)
        {
            switch (intakeType)
            {
                case "Calorie":
                    NewCurrentIntake = IntakeDto.CalorieCurrent;
                    NewIntakeGoal = IntakeDto.CalorieGoal;
                    break;
                case "Water":
                    NewCurrentIntake = IntakeDto.WaterCurrent;
                    NewIntakeGoal = IntakeDto.WaterGoal;
                    break;
            }

            _popupNavigation.PushAsync(new EditIntakeGoalPopup(this, intakeType));
        }

		#endregion popup methods
	}
}
