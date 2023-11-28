using CommunityToolkit.Mvvm.ComponentModel;

namespace BodyBuddy.Dtos
{
    public partial class StepDto : ObservableObject
    {
        public int Id { get; set; }

        [ObservableProperty] private int _steps;

        [ObservableProperty] private DateTime _date;
    }
}
