using CommunityToolkit.Mvvm.ComponentModel;

namespace BodyBuddy.Dtos
{
    public partial class WorkoutDto : ObservableObject
    {
        public int Id { get; set; }
        [ObservableProperty] private string _name;
        [ObservableProperty] private string _description;
        public bool PreMade { get; set; }
    }
}
