using CommunityToolkit.Mvvm.ComponentModel;

namespace BodyBuddy.Dtos
{
    public partial class ExerciseDto : ObservableObject
    {
        public int Id { get; set; }
        public int WorkoutExerciseId { get; set; }

        [ObservableProperty] private string _name;

        [ObservableProperty] private string _force;

        [ObservableProperty] private string _level;

        [ObservableProperty] private string _mechanic;

        [ObservableProperty] private string _equipment;

        [ObservableProperty] private string _primaryMuscles;

        [ObservableProperty] private string _secondaryMuscles;

        [ObservableProperty] private string _instructions;

        [ObservableProperty] private string _category;

        [ObservableProperty] private string _images;

        //Based on PrimaryMuscles
        [ObservableProperty] private string _targetArea;

        [ObservableProperty] private int _sets;

        [ObservableProperty] private int _reps;
        public List<ExerciseRecordsDto> Records { get; set; } = new();
    }

}
