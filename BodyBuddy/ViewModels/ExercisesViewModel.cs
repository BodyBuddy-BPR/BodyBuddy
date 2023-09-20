using BodyBuddy.Models;
using BodyBuddy.Repositories;
using BodyBuddy.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BodyBuddy.ViewModels
{
    [QueryProperty(nameof(QueryDetails), "Exercise")]
    public partial class ExercisesViewModel : BaseViewModel
    {
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IWorkoutRepository _workoutRepository;

        [ObservableProperty]
        private Exercise _queryDetails; //Category and Musclegroup selected in the previous pages

        [ObservableProperty]
        private Workout _workoutPlan; //If navigated to from a workout, this is that workout

        private Workout _selectedWorkout;
        public Workout SelectedWorkout
        {
            get { return _selectedWorkout; }
            set
            {
                if (_selectedWorkout != value)
                {
                    _selectedWorkout = value;
                    OnPropertyChanged(); // Notify that the property has changed
                }
            }
        }

        public ObservableCollection<Exercise> ExercisesList { get; set; } = new ObservableCollection<Exercise>();
        public ObservableCollection<Workout> WorkoutsList { get; set; } = new ObservableCollection<Workout>();

        public ExercisesViewModel(IExerciseRepository exerciseRepository, IWorkoutRepository workoutRepository)
        {
            Title = string.Empty;

            _exerciseRepository = exerciseRepository;
            _workoutRepository = workoutRepository;

            // Access the cached data
            WorkoutPlan = CachedData.SharedWorkout;
        }

        [RelayCommand]
        public async Task GetExercises()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                var exercises = await _exerciseRepository.GetExercisesAsync(QueryDetails.Category, QueryDetails.PrimaryMuscles);

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
        public async Task GetWorkouts()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                var workouts = await _workoutRepository.GetWorkoutPlansAsync();

                if (WorkoutsList.Count != 0)
                {
                    WorkoutsList.Clear();
                }

                foreach (var workout in workouts)
                {
                    WorkoutsList.Add(workout);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error!", $"Unable to get workouts {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        // Navigation to exercise details
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
    }
}
