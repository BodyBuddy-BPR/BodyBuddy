using BodyBuddy.Models;
using BodyBuddy.Repositories;
using BodyBuddy.Views.ExerciseViews;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using BodyBuddy.Dtos;

namespace BodyBuddy.ViewModels.ExerciseViewModels
{
    [QueryProperty(nameof(QueryDetails), "Exercise")]
    public partial class ExercisesViewModel : BaseViewModel
    {
        #region Injections

        private readonly IExerciseRepository _exerciseRepository;
        private readonly IWorkoutRepository _workoutRepository;
        private readonly IWorkoutExercisesRepository _workoutExercisesRepository;

        #endregion

        #region ObservableProperties

        [ObservableProperty]
        private Exercise _queryDetails; //Category and Musclegroup selected in the previous pages

        [ObservableProperty]
        private WorkoutModel _selectedWorkout;

        #endregion

        public ObservableCollection<Exercise> ExercisesList { get; set; } = new ObservableCollection<Exercise>();
        public ObservableCollection<WorkoutModel> WorkoutsList { get; set; } = new ObservableCollection<WorkoutModel>();

        public ExercisesViewModel(IExerciseRepository exerciseRepository, IWorkoutRepository workoutRepository, IWorkoutExercisesRepository workoutExercisesRepository)
        {
            Title = string.Empty;

            _exerciseRepository = exerciseRepository;
            _workoutRepository = workoutRepository;
            _workoutExercisesRepository = workoutExercisesRepository;
        }

        public async Task Initialize()
        {
            await GetExercises();
            await GetWorkouts();
        }

        #region Gets

        private async Task GetExercises()
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

                if (exercises != null)
                {
                    foreach (var exercise in exercises)
                    {
                        //await Task.Delay(50);
                        ExercisesList.Add(exercise);
                    }
                }
                else
                {
                    // Log or handle the case where exercises is null
                    await Shell.Current.DisplayAlert("Error!", $"Exercises is null", "OK");

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

        private async Task GetWorkouts()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                var workouts = await _workoutRepository.GetWorkoutPlansAsync(0);

                if (WorkoutsList.Count != 0)
                {
                    WorkoutsList.Clear();
                }

                foreach (var workout in workouts)
                {
                    WorkoutsList.Add(workout);
                }

                SetSelectedWorkout();
                
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

        #endregion

        private void SetSelectedWorkout()
        {
            if (CachedData.SharedWorkout != null && WorkoutsList.Any(x => x.Id == CachedData.SharedWorkout.Id))
            {
                SelectedWorkout = CachedData.SharedWorkout;
            }
            else
            {
                //SelectedWorkout = null;
                SelectedWorkout = new WorkoutModel { Name = "Select a workout" };
                //return;
            }
        }

        [RelayCommand]
        async Task AddExerciseToWorkout(Exercise exercise)
        {
            if(SelectedWorkout.Id != 0)
            {
                await _workoutExercisesRepository.AddExerciseToWorkout(SelectedWorkout.Id, exercise);
                await Shell.Current.DisplaySnackbar($"{exercise.Name} added to {SelectedWorkout.Name}");
            }
            else
            {
                await Shell.Current.DisplayAlert("No Workout", "Try selecting a Workout first", "OK");
                return;
            }
        }

        #region Navigation

        // Navigation to exercise details
        [RelayCommand]
        async Task GoToExerciseDetails(Exercise exercise)
        {
            if (exercise is null)
                return;

            await Task.Delay(100); // Add a short delay
            await Shell.Current.GoToAsync(nameof(ExerciseDetailsPage), true, new Dictionary<string, object>
            {
                {"Exercise", exercise }
            });
        }

        #endregion
    }
}
