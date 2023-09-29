using BodyBuddy.Models;
using BodyBuddy.Repositories;
using BodyBuddy.Views.ExerciseViews;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BodyBuddy.ViewModels.ExerciseViewModels
{
    [QueryProperty(nameof(QueryDetails), "Exercise")]
    public partial class ExercisesViewModel : BaseViewModel
    {
        #region Injections
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IWorkoutRepository _workoutRepository;
        #endregion

        [ObservableProperty]
        private Exercise _queryDetails; //Category and Musclegroup selected in the previous pages

        [ObservableProperty]
        private Workout _selectedWorkout;

        public ObservableCollection<Exercise> ExercisesList { get; set; } = new ObservableCollection<Exercise>();
        public ObservableCollection<Workout> WorkoutsList { get; set; } = new ObservableCollection<Workout>();

        public ExercisesViewModel(IExerciseRepository exerciseRepository, IWorkoutRepository workoutRepository)
        {
            Title = string.Empty;

            _exerciseRepository = exerciseRepository;
            _workoutRepository = workoutRepository;
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

        [RelayCommand]
        public async Task GetWorkouts()
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

        private void SetSelectedWorkout()
        {
            if (CachedData.SharedWorkout != null && WorkoutsList.Any(x => x.Id == CachedData.SharedWorkout.Id))
            {
                SelectedWorkout = CachedData.SharedWorkout;
            }
            else
            {
                //SelectedWorkout = null;
                SelectedWorkout = new Workout { Name = "Select a workout" };
                //return;
            }
        }

        [RelayCommand]
        async Task AddExerciseToWorkout(Exercise exercise)
        {
            if(SelectedWorkout.Id != 0)
            {
                await _workoutRepository.AddExerciseToWorkout(SelectedWorkout.Id, exercise.Id);
                await Shell.Current.DisplaySnackbar($"{exercise.Name} added to {SelectedWorkout.Name}");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error!", "Select a workout", "OK");
                return;
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
