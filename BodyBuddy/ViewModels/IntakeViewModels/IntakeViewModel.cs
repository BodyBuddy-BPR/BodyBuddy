using BodyBuddy.Models;
using BodyBuddy.Repositories;
using BodyBuddy.Views.Popups;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mopups.Interfaces;
using System.Diagnostics;

namespace BodyBuddy.ViewModels.IntakeViewmodels
{
	public partial class IntakeViewModel : BaseViewModel
	{
		private readonly IIntakeRepository _intakeRepository;
		IPopupNavigation _popupNavigation;

		[ObservableProperty]
		private Intake _intakeDetails;
		[ObservableProperty]
		private string _errorMessage;
		[ObservableProperty]
		private int _waterCurrent, _caloriesCurrent, _calorieEntryText, _newIntakeGoal, _calorieGoal, _waterGoal, _newCurrentIntake;
		[ObservableProperty]
		private double _waterIntakeProgress, _calorieIntakeProgress;

		public IntakeViewModel(IIntakeRepository intakeRepository, IPopupNavigation popupNavigation)
		{
			_intakeRepository = intakeRepository;
			_popupNavigation = popupNavigation;
		}

		public async Task Intilialize()
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

				var intake = await _intakeRepository.GetIntakeAsync();
				if (intake != null)
				{
					IntakeDetails = intake;
					CaloriesCurrent = IntakeDetails.CalorieCurrent;
					WaterCurrent = IntakeDetails.WaterCurrent;
					CalorieGoal = IntakeDetails.CalorieGoal;
					WaterGoal = IntakeDetails.WaterGoal;
					WaterIntakeProgress = IntakeDetails.WaterProgress;
					CalorieIntakeProgress = IntakeDetails.CalorieProgress;
				}
				else
				{
					await Shell.Current.DisplayAlert("Error!", $"Intake is null", "OK");

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
			WaterCurrent += 250;
			IntakeDetails.WaterCurrent = WaterCurrent;
			WaterIntakeProgress = (double)WaterCurrent / (double)WaterGoal;
			IntakeDetails.WaterProgress = WaterIntakeProgress;
			await _intakeRepository.SaveChangesAsync(IntakeDetails);
		}

		[RelayCommand]
		public async Task AddKcalClicked(int calories)
		{
			if(CaloriesCurrent + calories < 0)
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
				CaloriesCurrent += calories;
				IntakeDetails.CalorieCurrent = CaloriesCurrent;
				CalorieIntakeProgress = (double)CaloriesCurrent / (double)CalorieGoal;
				IntakeDetails.CalorieProgress = CalorieIntakeProgress;
				await _intakeRepository.SaveChangesAsync(IntakeDetails);
			}
		}

		public string CalorieIntakeProgressAsPercentage
		{
			get { return $"{CalorieIntakeProgress * 100:F1} %"; }
		}

		#region popup methods

		public async Task<bool> SaveNewIntakeGoal(string intakeType)
		{
			if (NewCurrentIntake < 0)
			{
				ErrorMessage = "Current intake cannot be negative.";
				return false;
			}
			else if (NewIntakeGoal == 0 || NewIntakeGoal < 0)
			{
				ErrorMessage = "New intake goal cannot be negative or zero.";
				return false;
			}
			else
			{
				if (intakeType == "Calorie")
				{
					IntakeDetails.CalorieGoal = NewIntakeGoal;
					CalorieGoal = IntakeDetails.CalorieGoal;
					IntakeDetails.CalorieCurrent = NewCurrentIntake;
					CaloriesCurrent = IntakeDetails.CalorieCurrent;
					IntakeDetails.CalorieProgress = (double)CaloriesCurrent / (double)NewIntakeGoal;
					CalorieIntakeProgress = IntakeDetails.CalorieProgress;
				}
				else
				{
					IntakeDetails.WaterGoal = NewIntakeGoal;
					WaterGoal = IntakeDetails.WaterGoal;
					IntakeDetails.WaterCurrent = NewCurrentIntake;
					WaterCurrent = IntakeDetails.WaterCurrent;
					IntakeDetails.WaterProgress = (double)WaterCurrent / (double)NewIntakeGoal;
					WaterIntakeProgress = IntakeDetails.WaterProgress;
				}

				await _intakeRepository.SaveChangesAsync(IntakeDetails);

				NewIntakeGoal = 0;
				NewCurrentIntake = 0;
				ErrorMessage = string.Empty;

				return true;
			}
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
				NewCurrentIntake = CaloriesCurrent;
				NewIntakeGoal = CalorieGoal;
			}
			else if (intakeType == "Water")
			{
				NewCurrentIntake = WaterCurrent;
				NewIntakeGoal = WaterGoal;
			}
			_popupNavigation.PushAsync(new EditIntakeGoalPopup(this, intakeType));
		}

		#endregion popup methods
	}
}
