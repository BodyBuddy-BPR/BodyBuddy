using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.Dtos
{
    public partial class IntakeDto : ObservableObject
    {
        [ObservableProperty] private int _id;

        [ObservableProperty] private DateTime _date;

        private int _calorieGoal, _waterGoal, _calorieCurrent, _waterCurrent;

        public double WaterProgress => _waterGoal != 0 ? (double)_waterCurrent / _waterGoal : 0;
        public double CalorieProgress => _calorieGoal != 0 ? (double)_calorieCurrent / _calorieGoal : 0;

        public int CalorieCurrent
        {
            get => _calorieCurrent;
            set => SetAndNotify(ref _calorieCurrent, value, nameof(CalorieProgress), nameof(CalorieCurrent));
        }

        public int WaterCurrent
        {
            get => _waterCurrent;
            set => SetAndNotify(ref _waterCurrent, value, nameof(WaterProgress), nameof(WaterCurrent));
        }

        public int WaterGoal
        {
            get => _waterGoal;
            set => SetAndNotify(ref _waterGoal, value, nameof(WaterProgress), nameof(WaterGoal));
        }

        public int CalorieGoal
        {
            get => _calorieGoal;
            set => SetAndNotify(ref _calorieGoal, value, nameof(CalorieProgress), nameof(CalorieGoal));
        }

        private void SetAndNotify<T>(ref T field, T value, params string[] additionalPropertiesToNotify)
        {
            if (!SetProperty(ref field, value)) return;

            //Notifying each property identified
            foreach (var propertyName in additionalPropertiesToNotify)
            {
                OnPropertyChanged(propertyName);
            }
        }
    }
}
