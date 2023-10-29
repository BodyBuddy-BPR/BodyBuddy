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

namespace BodyBuddy.ViewModels.IntakeViewModels
{
	public partial class IntakeViewModel : BaseViewModel
	{
		private readonly IIntakeService _intakeService;
        private readonly IPopupNavigation _popupNavigation;

		[ObservableProperty]
		private IntakeDto _intakeDetails;
		[ObservableProperty]
		private string _errorMessage;
		[ObservableProperty]
		private int _calorieEntryText, _newIntakeGoal, _newCurrentIntake;

        [ObservableProperty]
		private double _waterIntakeProgress, _calorieIntakeProgress;

		public IntakeViewModel(IIntakeService intakeService, IPopupNavigation popupNavigation)
		{
			_intakeService = intakeService;
			_popupNavigation = popupNavigation;
		}

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
					IntakeDetails = intake;

					WaterIntakeProgress = (double)intake.WaterCurrent / (double)intake.WaterGoal;
					CalorieIntakeProgress = (double)intake.CalorieCurrent / (double)intake.CalorieGoal;
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
			IntakeDetails.WaterCurrent += 250;
			
            WaterIntakeProgress = (double)IntakeDetails.WaterCurrent / (double)IntakeDetails.WaterGoal;
			await _intakeService.SaveChangesAsync(IntakeDetails);
		}

		[RelayCommand]
		public async Task AddKcalClicked(int calories)
		{
			if(IntakeDetails.CalorieCurrent + calories < 0)
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
				IntakeDetails.CalorieCurrent += calories;

				CalorieIntakeProgress = (double)IntakeDetails.CalorieCurrent / (double)IntakeDetails.CalorieGoal;
				await _intakeService.SaveChangesAsync(IntakeDetails);
			}
		}

		public string CalorieIntakeProgressAsPercentage
		{
			get { return $"{CalorieIntakeProgress * 100:F1} %"; }
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
                IntakeDetails.CalorieGoal = NewIntakeGoal;
                IntakeDetails.CalorieCurrent = NewCurrentIntake;

                CalorieIntakeProgress = (double)IntakeDetails.CalorieCurrent / (double)NewIntakeGoal;
            }
            else
            {
                IntakeDetails.WaterGoal = NewIntakeGoal;
                IntakeDetails.WaterCurrent = NewCurrentIntake;

                WaterIntakeProgress = (double)IntakeDetails.WaterCurrent / (double)NewIntakeGoal;
            }

            await _intakeService.SaveChangesAsync(IntakeDetails);

            NewIntakeGoal = 0;
            NewCurrentIntake = 0;
            ErrorMessage = string.Empty;

            return true;
        }

		[RelayCommand]
		public void DeclineEditIntake()
		{
			ErrorMessage = string.Empty;
			NewIntakeGoal = 0;
		}

		[RelayCommand]
		private void ClickToShowPopup_Clicked(string intakeType)
		{
			if (intakeType == "Calorie")
			{
				NewCurrentIntake = IntakeDetails.CalorieCurrent;
				NewIntakeGoal = IntakeDetails.CalorieGoal;
			}
			else if (intakeType == "Water")
			{
				NewCurrentIntake = IntakeDetails.WaterCurrent;
				NewIntakeGoal = IntakeDetails.WaterGoal;
			}
			_popupNavigation.PushAsync(new EditIntakeGoalPopup(this, intakeType));
		}

		#endregion popup methods
	}
}
