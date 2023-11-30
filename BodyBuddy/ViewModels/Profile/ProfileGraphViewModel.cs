using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public ProfileGraphViewModel(IIntakeService intakeService)
        {
            _intakeService = intakeService;
            GenerateIntakeData();
        }

        private void GenerateIntakeData()
        {
            IntakeList.Add(new IntakeDto()
            {
                CalorieCurrent = 3333,
                CalorieGoal = 3500,
                WaterCurrent = 2500,
                WaterGoal = 2500,
                Date = new DateTime(2023, 11, 30)
            });
            IntakeList.Add(new IntakeDto()
            {
                CalorieCurrent = 3333,
                CalorieGoal = 3500,
                WaterCurrent = 2500,
                WaterGoal = 2500,
                Date = new DateTime(2023, 11, 29)
            });
            IntakeList.Add(new IntakeDto()
            {
                CalorieCurrent = 3333, 
                CalorieGoal = 3500, 
                WaterCurrent = 2500, 
                WaterGoal = 2500, 
                Date = new DateTime(2023, 11, 26)
            });                        
            IntakeList.Add(new IntakeDto()
            {
                CalorieCurrent = 2500,
                CalorieGoal = 3500,
                WaterCurrent = 1500, 
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

        public async Task GetAllIntakeData()
        {
            IntakeList = new ObservableCollection<IntakeDto>(await _intakeService.GetAllIntakeDataAsync());
        }


    }
}
