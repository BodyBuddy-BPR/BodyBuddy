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

        [ObservableProperty] private int _calorieGoal;

        [ObservableProperty] private int _waterGoal;

        [ObservableProperty] private int _calorieCurrent;

        [ObservableProperty] private int _waterCurrent;

        [ObservableProperty] private DateTime _date;
    }
}
