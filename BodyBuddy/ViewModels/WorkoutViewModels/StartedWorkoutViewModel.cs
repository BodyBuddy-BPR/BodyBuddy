using BodyBuddy.Views.ExerciseViews;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using BodyBuddy.Dtos;
using BodyBuddy.Services;

namespace BodyBuddy.ViewModels.WorkoutViewModels
{
    [QueryProperty(nameof(WorkoutDetails), "Workout")]
    public partial class StartedWorkoutViewModel : BaseViewModel
    {
        private readonly IWorkoutExercisesService _workoutExercisesService;
        private readonly IExerciseRecordsService _exerciseRecordsService;

        // Query field for the started workout
        [ObservableProperty]
        private WorkoutDto _workoutDetails;

        [ObservableProperty]
        private ExerciseDto _displayedExercise;
        public ObservableCollection<ExerciseRecordsDto> ExerciseRecords { get; set; } = new();
        [ObservableProperty] private List<ExerciseDto> _exercises = new();


        // Keep track of the index of the currently displayed exercise
        private int _currentExerciseIndex = 0;

        [ObservableProperty]
        public bool previousButtonIsEnabled = false;
        [ObservableProperty]
        public bool nextButtonIsEnabled = true;
        [ObservableProperty]
        public bool finishWorkoutButtonIsEnabled = false;

        [ObservableProperty]
        public bool _isWorkoutFinished = false;

        public StartedWorkoutViewModel(IWorkoutExercisesService workoutExercisesService, IExerciseRecordsService exerciseRecordsService)
        {
            _workoutExercisesService = workoutExercisesService;
            _exerciseRecordsService = exerciseRecordsService;
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

                Exercises = await _workoutExercisesService.GetExercisesInWorkout(WorkoutDetails.Id);
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
                DisplayedExercise.Records = new List<ExerciseRecordsDto>();

                // Generate sets
                for (int i = 1; i <= DisplayedExercise.Sets; i++)
                {
                    var exerciseRecord = new ExerciseRecordsDto()
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
            IsWorkoutFinished = true;

        }

        [RelayCommand]
        public async Task GoBackToWorkoutDetails()
        {
            await GoBackAsync();
            IsWorkoutFinished = false;
        }

        public async Task SaveRecords()
        {
            // Save the exercise records
            foreach (var exercise in Exercises)
            {
                foreach (var record in exercise.Records)
                {
                    if (record.Weight > 0)
                        await _exerciseRecordsService.SaveExerciseRecords(record);
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


        #region Navigation 

        [RelayCommand]
        async Task ToExercise(ExerciseDto exercise)
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
