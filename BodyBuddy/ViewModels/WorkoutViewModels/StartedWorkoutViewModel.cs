using BodyBuddy.Models;
using BodyBuddy.Repositories;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.ViewModels.WorkoutViewModels
{
    [QueryProperty(nameof(WorkoutDetails), "Workout")]
    public partial class StartedWorkoutViewModel : BaseViewModel
    {
        private readonly IWorkoutExercisesRepository _workoutExercisesRepository;

        // Query field for the started workout
        [ObservableProperty]
        private Workout _workoutDetails;

        [ObservableProperty]
        private Exercise _displayedExercise;
        public ObservableCollection<Exercise> Exercises { get; set; } = new ObservableCollection<Exercise>();

        // Keep track of the index of the currently displayed exercise
        private int _currentExerciseIndex = 0;

        public StartedWorkoutViewModel(IWorkoutExercisesRepository workoutExercisesRepository)
        {
            _workoutExercisesRepository = workoutExercisesRepository;
        }

        public async Task Initialize()
        {
            await GetExercisesFromWorkout();
        }

        public async Task GetExercisesFromWorkout()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;


                var workoutPlan = await _workoutExercisesRepository.GetExercisesInWorkout(WorkoutDetails.Id, false); // False for user made workouts

                if (Exercises.Count != 0)
                {
                    Exercises.Clear();
                }

                foreach (var exercise in workoutPlan)
                {
                    Exercises.Add(exercise);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error!", $"Unable to get workout plans {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;

                DisplayedExercise = Exercises.FirstOrDefault();
            }
        }

        [RelayCommand]
        public void NextExercise()
        {
            if (Exercises.Count == 0)
            {
                return; // No exercises available
            }

            // Move to the next exercise
            _currentExerciseIndex++;

            // If we reached the end, loop back to the first exercise
            if (_currentExerciseIndex >= Exercises.Count)
            {
                _currentExerciseIndex = 0;
            }

            // Update the displayed exercise
            DisplayedExercise = Exercises[_currentExerciseIndex];
        }

        [RelayCommand]
        public void PreviousExercise()
        {
            if (Exercises.Count == 0)
            {
                return; // No exercises available
            }

            // Move to the previous exercise
            _currentExerciseIndex--;

            // If we reached the beginning, loop to the last exercise
            if (_currentExerciseIndex < 0)
            {
                _currentExerciseIndex = Exercises.Count - 1;
            }

            // Update the displayed exercise
            DisplayedExercise = Exercises[_currentExerciseIndex];
        }
    }
}
