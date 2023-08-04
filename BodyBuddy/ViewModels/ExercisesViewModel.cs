using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BodyBuddy.ViewModels
{
    public partial class ExercisesViewModel : BaseViewModel
    {
        private readonly IExerciseRepository _exerciseRepository;
        private IConnectivity _connectivity;

        public ObservableCollection<Exercise> ExercisesList { get; set; } = new ObservableCollection<Exercise>();

        public ObservableCollection<string> FilterChips { get; set; } = new();

        public ExercisesViewModel(IExerciseRepository exerciseRepository, IConnectivity connectivity)
        {
            Title = string.Empty;

            _exerciseRepository = exerciseRepository;
            _connectivity = connectivity; 
            GenerateFilterChips();

        }

        [RelayCommand]
        public async Task GetExercises(string muscleGroup)
        {

            if (IsBusy) return;

            try
            {
                IsBusy = true;

                var exercises = await _exerciseRepository.GetExercisesAsync(muscleGroup);

                if (ExercisesList.Count != 0)
                {
                    ExercisesList.Clear();
                }

                foreach (var exercise in exercises)
                {
                    ExercisesList.Add(exercise);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error!", $"Unable to get exercises {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        async Task GoToExerciseDetails(Exercise exercise)
        {
            if (exercise is null) 
                return;

            await Shell.Current.GoToAsync(nameof(ExerciseDetailsPage), true, new Dictionary<string, object>
            {
                {"Exercise", exercise }
            });
        }

        private void GenerateFilterChips()
        {
            FilterChips.Add("abdominals");
            FilterChips.Add("hamstrings");
            FilterChips.Add("adductors");
            FilterChips.Add("quadriceps");
            FilterChips.Add("biceps");
            FilterChips.Add("glutes");
            FilterChips.Add("calves");
            FilterChips.Add("shoulders");
            FilterChips.Add("chest");
            FilterChips.Add("middle back");
            FilterChips.Add("lower back");
            FilterChips.Add("lats");
            FilterChips.Add("triceps");
            FilterChips.Add("traps");
            FilterChips.Add("forearms");
            FilterChips.Add("neck");
            FilterChips.Add("abductors");

            //List<Exercise> muscleGroups = new List<Exercise>
            //{

            //    new Exercise { PrimaryMuscles = "abdominals"},
            //    new Exercise { PrimaryMuscles = "hamstrings"},
            //    new Exercise { PrimaryMuscles = "adductors"},
            //    new Exercise { PrimaryMuscles = "quadriceps"},
            //    new Exercise { PrimaryMuscles = "biceps"},
            //    new Exercise { PrimaryMuscles = "glutes"},
            //    new Exercise { PrimaryMuscles = "calves"},
            //    new Exercise { PrimaryMuscles = "shoulders"},
            //    new Exercise { PrimaryMuscles = "chest"},
            //    new Exercise { PrimaryMuscles = "middle back"},
            //    new Exercise { PrimaryMuscles = "lower back"},
            //    new Exercise { PrimaryMuscles = "lats"},
            //    new Exercise { PrimaryMuscles = "triceps"},
            //    new Exercise { PrimaryMuscles = "traps"},
            //    new Exercise { PrimaryMuscles = "forearms"},
            //    new Exercise { PrimaryMuscles = "neck"},
            //    new Exercise { PrimaryMuscles = "abductors"},
            //};

            //foreach (Exercise muscleGroup in muscleGroups)
            //{
            //    FilterChips.Add(muscleGroup);
            //}
        }
    }
}
