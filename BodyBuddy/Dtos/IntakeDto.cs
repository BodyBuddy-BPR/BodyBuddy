using CommunityToolkit.Mvvm.ComponentModel;

namespace BodyBuddy.Dtos
{
    public partial class IntakeDto : ObservableObject
    {
        [ObservableProperty] private int _id;

        [ObservableProperty] private DateTime _date;

        [ObservableProperty] private int _calorieGoal;

        [ObservableProperty] private int _waterGoal;

        [ObservableProperty] private int _calorieCurrent;

        [ObservableProperty] private int _waterCurrent;
    }
}
