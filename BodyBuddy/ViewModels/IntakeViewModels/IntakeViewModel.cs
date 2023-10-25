using BodyBuddy.Models;
using BodyBuddy.Repositories;
using BodyBuddy.Views.Popups;
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
		private IntakeModel _intakeDetails;
		[ObservableProperty]
		private string _errorMessage;
		[ObservableProperty]
		private int _waterCurrent, _caloriesCurrent, _calorieEntryText, _newIntakeGoal, _calorieGoal, _waterGoal;

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
			await _intakeRepository.SaveChangesAsync(IntakeDetails);
		}

		[RelayCommand]
		public async Task AddKcalClicked(int calories)
		{
			CaloriesCurrent += calories;
			IntakeDetails.CalorieCurrent = CaloriesCurrent;
			await _intakeRepository.SaveChangesAsync(IntakeDetails);
		}

		#region popup methods

		public async Task<bool> SaveNewIntakeGoal(string intakeType)
		{
			if (NewIntakeGoal == 0)
			{
				ErrorMessage = "New intake goal cannot be empty.";
				return false;
			}
			else
			{
				if(intakeType == "Calorie")
				{
					IntakeDetails.CalorieGoal = NewIntakeGoal;
					CalorieGoal = IntakeDetails.CalorieGoal;
				}
				else
				{
					IntakeDetails.WaterGoal = NewIntakeGoal;
					WaterGoal = IntakeDetails.WaterGoal;
				}

				await _intakeRepository.SaveChangesAsync(IntakeDetails);

				NewIntakeGoal = 0;
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
			_popupNavigation.PushAsync(new EditIntakeGoalPopup(this, intakeType));
		}

		#endregion popup methods
	}
}
