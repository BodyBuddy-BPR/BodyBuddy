using System.Collections.ObjectModel;
using BodyBuddy.Dtos;
using BodyBuddy.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BodyBuddy.ViewModels.Profile
{
    public partial class ProfileGraphViewModel : BaseViewModel
    {
        private readonly IIntakeService _intakeService;

        [ObservableProperty]
        private ObservableCollection<IntakeDto> _intakeList = new();

        private int _intakeCount, _calorieGoalsCompleted, _waterGoalsCompleted;
        [ObservableProperty] string _calorieGoalsCompletedString, _waterGoalsCompletedString;

        public ProfileGraphViewModel(IIntakeService intakeService)
        {
            _intakeService = intakeService;
            //GenerateIntakeData();
        }

        public async Task GetAllIntakeData()
        {
            IntakeList = new ObservableCollection<IntakeDto>(await _intakeService.GetAllIntakeDataAsync());
            GoalsCompleted();
        }

        #region privates
        private void GoalsCompleted()
        {
            _intakeCount = IntakeList.Count;
            foreach (IntakeDto intake in IntakeList)
            {
                if (intake.CalorieCurrent >= intake.CalorieGoal)
                    _calorieGoalsCompleted++;
                if (intake.WaterCurrent >= intake.WaterGoal)
                    _waterGoalsCompleted++;
            }

            SetStringText();
        }
        private void SetStringText()
        {
            CalorieGoalsCompletedString = $"{_calorieGoalsCompleted} / {_intakeCount} days";
            WaterGoalsCompletedString = $"{_waterGoalsCompleted} / {_intakeCount} days";
        }
        #endregion


        #region TestData
        private void GenerateIntakeData()
        {
            IntakeList.Add(new IntakeDto()
            {
                CalorieCurrent = 2500,
                CalorieGoal = 3000,
                WaterCurrent = 3000,
                WaterGoal = 2500,
                Date = new DateTime(2023, 11, 30)
            });
            IntakeList.Add(new IntakeDto()
            {
                CalorieCurrent = 3333,
                CalorieGoal = 3000,
                WaterCurrent = 2750,
                WaterGoal = 2500,
                Date = new DateTime(2023, 11, 29)
            });
            IntakeList.Add(new IntakeDto()
            {
                CalorieCurrent = 3333,
                CalorieGoal = 3000,
                WaterCurrent = 2750,
                WaterGoal = 2500,
                Date = new DateTime(2023, 11, 26)
            });
            IntakeList.Add(new IntakeDto()
            {
                CalorieCurrent = 2500,
                CalorieGoal = 3500,
                WaterCurrent = 2500,
                WaterGoal = 2500,
                Date = new DateTime(2023, 11, 25)
            });
            IntakeList.Add(new IntakeDto()
            {
                CalorieCurrent = 2000,
                CalorieGoal = 3500,
                WaterCurrent = 1250,
                WaterGoal = 2500,
                Date = new DateTime(2023, 11, 24)
            });
        }
        #endregion
    }
}
