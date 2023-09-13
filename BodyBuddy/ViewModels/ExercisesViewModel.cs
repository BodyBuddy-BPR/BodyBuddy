using BodyBuddy.Models;
using BodyBuddy.Repositories;
using BodyBuddy.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BodyBuddy.ViewModels
{
    [QueryProperty(nameof(Exercise), "Exercise")]
    public partial class ExercisesViewModel : BaseViewModel
    {
        private readonly IExerciseRepository _exerciseRepository;
        private IConnectivity _connectivity;

        [ObservableProperty]
        private Exercise exercise;

        public ObservableCollection<Exercise> ExercisesList { get; set; } = new ObservableCollection<Exercise>();

        //public ObservableCollection<string> FilterChips { get; set; } = new();

        public ExercisesViewModel(IExerciseRepository exerciseRepository, IConnectivity connectivity)
        {
            Title = string.Empty;

            _exerciseRepository = exerciseRepository;
            _connectivity = connectivity; 

        }

        [RelayCommand]
        public async Task GetExercises()
        {

            if (IsBusy) return;

            try
            {
                IsBusy = true;

                var exercises = await _exerciseRepository.GetExercisesAsync(Exercise.Category, Exercise.PrimaryMuscles);

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

        //Should probably be moved, as this is pretty dum
        //private void GenerateFilterChips()
        //{
        //    FilterChips.Add("abdominals");
        //    FilterChips.Add("hamstrings");
        //    FilterChips.Add("adductors");
        //    FilterChips.Add("quadriceps");
        //    FilterChips.Add("biceps");
        //    FilterChips.Add("glutes");
        //    FilterChips.Add("calves");
        //    FilterChips.Add("shoulders");
        //    FilterChips.Add("chest");
        //    FilterChips.Add("middle back");
        //    FilterChips.Add("lower back");
        //    FilterChips.Add("lats");
        //    FilterChips.Add("triceps");
        //    FilterChips.Add("traps");
        //    FilterChips.Add("forearms");
        //    FilterChips.Add("neck");
        //    FilterChips.Add("abductors");
        //}
    }
}
