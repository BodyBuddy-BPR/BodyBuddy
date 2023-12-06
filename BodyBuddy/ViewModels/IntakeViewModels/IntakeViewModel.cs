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
        #region properties
        private readonly IIntakeService _intakeService;
        private readonly IPopupNavigation _popupNavigation;

        [ObservableProperty]
        private string _errorMessage;
        [ObservableProperty]
        private int _calorieEntryText, _newIntakeGoal, _newCurrentIntake;

        //Setting WaterProgress and CalorieProgress
        public double WaterProgress => IntakeDto.WaterGoal != 0 ? (double)IntakeDto.WaterCurrent / IntakeDto.WaterGoal : 0;
        [ObservableProperty]
        private bool _isConfettiAnimationEnabled = false;
        private readonly string _waterGoalReachedToday = "WaterGoalReachedToday";
        public double CalorieProgress => IntakeDto.CalorieGoal != 0 ? (double)IntakeDto.CalorieCurrent / IntakeDto.CalorieGoal : 0;

        private IntakeDto _intakeDto;
        public IntakeDto IntakeDto
        {
            get => _intakeDto;
            set
            {
                if (_intakeDto != null)
                {
                    _intakeDto.PropertyChanged -= IntakeDtoPropertyChanged;
                }

                if (!SetProperty(ref _intakeDto, value))
                    return;

                if (_intakeDto != null)
                {
                    //Subscribing properties changed
                    _intakeDto.PropertyChanged += IntakeDtoPropertyChanged;
                }

                //Updating both properties when IntakeDto is set
                OnPropertyChanged(nameof(CalorieProgress));
                OnPropertyChanged(nameof(WaterProgress));
            }
        }
        #endregion

        public IntakeViewModel(IIntakeService intakeService, IPopupNavigation popupNavigation)
        {
            _intakeService = intakeService;
            _popupNavigation = popupNavigation;

            IntakeDto = new IntakeDto();
            IntakeDto.PropertyChanged += IntakeDtoPropertyChanged;
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

                var intake = await _intakeService.GetIntakeTodayAsync();
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
            // Increase water current
            IntakeDto.WaterCurrent += 250;

            if (IntakeDto.WaterCurrent >= IntakeDto.WaterGoal && IntakeDto.WaterCurrent <= IntakeDto.WaterGoal + 250)
            {
                IsConfettiAnimationEnabled = true;
            }

            // Save changes to IntakeDto
            await _intakeService.SaveChangesAsync(IntakeDto);
        }


        [RelayCommand]
        public async Task AddKcalClicked(int calories)
        {
            if (IntakeDto.CalorieCurrent + calories < 0)
            {
                ErrorMessage = "Cannot reduce current calorie intake below zero.";

                CancellationTokenSource cancellationTokenSource = new();

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

        #region private methods



        private void IntakeDtoPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //If the following properties are changed, it lets the UI know that WaterProgress or CalorieProgress is changed
            switch (e.PropertyName)
            {
                case nameof(IntakeDto.WaterCurrent) or nameof(IntakeDto.WaterGoal):
                    OnPropertyChanged(nameof(WaterProgress));
                    break;
                case nameof(IntakeDto.CalorieCurrent) or nameof(IntakeDto.CalorieGoal):
                    OnPropertyChanged(nameof(CalorieProgress));
                    break;
            }
        }

        #endregion

    }
}
