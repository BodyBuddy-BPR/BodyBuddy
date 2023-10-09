using BodyBuddy.Models;
using BodyBuddy.Repositories;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.ViewModels.IntakeViewmodels
{
	public partial class IntakeViewModel : BaseViewModel
	{
		private readonly IIntakeRepository _intakeRepository;

		[ObservableProperty]
		private Intake _intakeDetails;
		[ObservableProperty]
		private int _calorieEntryText;

		public IntakeViewModel(IIntakeRepository intakeRepository)
		{
			_intakeRepository = intakeRepository;
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

		// This method probably shouldnt call GetIntakeGoals, but its the only way I have gotten it to update IntakeDetails in realtime
		[RelayCommand]
		async Task AddWaterClicked()
		{
			IntakeDetails.WaterCurrent += 250;
			await _intakeRepository.SaveChangesAsync(IntakeDetails);
			await GetIntakeGoals();
		}

		// This method probably shouldnt call GetIntakeGoals, but its the only way I have gotten it to update IntakeDetails in realtime
		[RelayCommand]
		async Task AddKcalClicked(int calories)
		{
			IntakeDetails.CalorieCurrent += calories;
			await _intakeRepository.SaveChangesAsync(IntakeDetails);
			await GetIntakeGoals();
		}
	}
}
