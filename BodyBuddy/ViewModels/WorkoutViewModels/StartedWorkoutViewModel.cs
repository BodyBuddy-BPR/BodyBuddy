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
        private readonly IExerciseRecordsRepository _exerciseRecordsRepository;

        // Query field for the started workout
        [ObservableProperty]
        private Workout _workoutDetails;

        [ObservableProperty]
        private Exercise _displayedExercise;
        public ObservableCollection<ExerciseRecords> ExerciseRecords { get; set; } = new ObservableCollection<ExerciseRecords>();
        public ObservableCollection<Exercise> Exercises { get; set; } = new ObservableCollection<Exercise>();
              

        // Keep track of the index of the currently displayed exercise
        private int _currentExerciseIndex = 0;

        [ObservableProperty]
        public bool previousButtonIsEnabled = false;
        [ObservableProperty]
        public bool nextButtonIsEnabled = true;
        [ObservableProperty]
        public bool finishWorkoutButtonIsEnabled = false;


        public StartedWorkoutViewModel(IWorkoutExercisesRepository workoutExercisesRepository, IExerciseRecordsRepository exerciseRecordsRepository)
        {
            _workoutExercisesRepository = workoutExercisesRepository;
            _exerciseRecordsRepository = exerciseRecordsRepository;
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

                bool isPremade = Convert.ToBoolean(WorkoutDetails.PreMade); 
                var workoutPlan = await _workoutExercisesRepository.GetExercisesInWorkout(WorkoutDetails.Id, isPremade); // False for user made workouts

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
                await MakeSets();
            }
        }

        public async Task MakeSets()
        {
            if (DisplayedExercise == null || DisplayedExercise.Sets <= 0)
            {
                return; // No exercise selected or no sets defined
            }

            // Check if the exercise already has records
            if (DisplayedExercise.Records == null || DisplayedExercise.Records.Count == 0)
            {
                // Generate sets only if records are not already present
                DisplayedExercise.Records = new List<ExerciseRecords>();

                // Generate sets
                for (int i = 1; i <= DisplayedExercise.Sets; i++)
                {
                    var exerciseRecord = new ExerciseRecords
                    {
                        ExerciseId = DisplayedExercise.Id,
                        Set = i,
                        Reps = DisplayedExercise.Reps
                    };
                    DisplayedExercise.Records.Add(exerciseRecord);
                }
            }

            // Update ExerciseRecords with the records for the current exercise
            ExerciseRecords.Clear();
            foreach (var record in DisplayedExercise.Records)
            {
                ExerciseRecords.Add(record);
            }
        }

        [RelayCommand]
        public async Task FinishWorkout()
        {
            await SaveRecords();
            await GoBackAsync();
        }

        public async Task SaveRecords()
        {
            // Save the exercise records
            foreach (var exercise in Exercises)
            {
                foreach (var record in exercise.Records)
                {
                    await _exerciseRecordsRepository.SaveExerciseRecords(record);
                }
            }

        }

        #region Cycle Exercises Buttons

        [RelayCommand]
        public async Task NextExercise()
        {
            if (Exercises.Count == 0 || _currentExerciseIndex >= Exercises.Count - 1)
            {
                FinishWorkoutButtonIsEnabled = true;
                return; // No exercises available or already at the last exercise
            }

            // Move to the next exercise
            _currentExerciseIndex++;

            // Update the displayed exercise
            DisplayedExercise = Exercises[_currentExerciseIndex];

            PreviousButtonIsEnabled = true;

            // Check if we are now at the last exercise after moving
            if (_currentExerciseIndex >= Exercises.Count - 1)
            {
                FinishWorkoutButtonIsEnabled = true;
                NextButtonIsEnabled = false;
            }
            else
            {
                FinishWorkoutButtonIsEnabled = false;
            }

            await MakeSets();
        }

        [RelayCommand]
        public async Task PreviousExercise()
        {
            if (Exercises.Count == 0 || _currentExerciseIndex == 0)
            {
                PreviousButtonIsEnabled = false;
                return; // No exercises available or already at the first exercise
            }

            // Move to the previous exercise
            _currentExerciseIndex--;

            // Update the displayed exercise
            DisplayedExercise = Exercises[_currentExerciseIndex];

            FinishWorkoutButtonIsEnabled = false;
            NextButtonIsEnabled = true;

            // Check if there is a previous exercise and update IsEnabled accordingly
            if (_currentExerciseIndex > 0)
            {
                PreviousButtonIsEnabled = true;
            }
            else
            {
                PreviousButtonIsEnabled = false; // Disable the "Previous" button when on the first exercise
            }

            await MakeSets();
        }

        #endregion

    }
}
