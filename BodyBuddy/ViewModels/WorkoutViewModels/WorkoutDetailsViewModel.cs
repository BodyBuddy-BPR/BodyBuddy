using BodyBuddy.Models;
using BodyBuddy.Repositories;
using BodyBuddy.Views.ExerciseViews;
using BodyBuddy.Views.WorkoutViews;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mopups.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BodyBuddy.ViewModels.WorkoutViewModels
{
    [QueryProperty(nameof(WorkoutDetails), "Workout")]
    public partial class WorkoutDetailsViewModel : BaseViewModel
    {
        private readonly IWorkoutRepository _workoutRepository;
        private readonly IWorkoutExercisesRepository _workoutExercisesRepository;

        #region ObservableProperties

        // Query field
        [ObservableProperty]
        private Workout _workoutDetails;

        // IsPremade (used to hide edit and deletions)
        [ObservableProperty]
        private bool _isPremade;

        // Displayed Fields
        [ObservableProperty]
        public string workoutName, workoutDescription;

        // Workout Popup fields
        [ObservableProperty]
        public string popupName, popupDescription, errorMessage;

        // Exercise Popup fields
        [ObservableProperty]
        public int sets, reps;

        [ObservableProperty]
        public Exercise exerciseToEdit;

        [ObservableProperty]
        private bool smallButtonsIsEnabled; // This is the small Add Exercise & Start Workout buttons
        [ObservableProperty]
        private bool largeButtonModifyIsEnabled, smallButtonModifyIsEnabled; // For the AddExercisesCommand 



        #endregion

        public ObservableCollection<Exercise> Exercises { get; set; } = new ObservableCollection<Exercise>();

        public WorkoutDetailsViewModel(IWorkoutRepository workoutRepository, IWorkoutExercisesRepository workoutExercisesRepository)
        {
            _workoutRepository = workoutRepository;
            _workoutExercisesRepository = workoutExercisesRepository;
        }


        public async Task Initialize()
        {
            WorkoutName = WorkoutDetails.Name;
            if (string.IsNullOrWhiteSpace(WorkoutDetails.Description))
            {
                WorkoutDetails.Description = "Try giving this workout a description";
            }
            WorkoutDescription = WorkoutDetails.Description;

            await GetExercisesFromWorkout();

            IsPremade = true;

            //Setting up Visibility of small and big buttons
            if (Exercises.Count > 0)
            {
                LargeButtonModifyIsEnabled = (false && !IsPremade);
                SmallButtonModifyIsEnabled = (!IsPremade);
                SmallButtonsIsEnabled = true;
            }
            else
            {
                LargeButtonModifyIsEnabled = (!IsPremade);
                SmallButtonModifyIsEnabled = (false && !IsPremade);
                SmallButtonsIsEnabled = false;
            }
        }


        public async Task GetExercisesFromWorkout()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;


                var workoutPlan = await _workoutExercisesRepository.GetExercisesInWorkout(WorkoutDetails.Id);

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
            }
        }


        [RelayCommand]
        async Task DeleteFromWorkout(Exercise exercise)
        {
            bool result = await Shell.Current.DisplayAlert("Delete", $"Are you sure you want to remove {exercise.Name} from this workout?", "OK", "Cancel");

            if (result)
            {
                if (exercise == null) return;
                await _workoutExercisesRepository.DeleteExerciseFromWorkout(WorkoutDetails.Id, exercise.Id);
                Exercises.Remove(exercise);

                if (Exercises.Count == 0)
                {
                    SmallButtonModifyIsEnabled = false;
                    LargeButtonModifyIsEnabled = true && !IsPremade;
                }
            }
        }


        #region Workout Popup

        public async Task<bool> SaveWorkout()
        {
            var exists = await _workoutRepository.DoesWorkoutAlreadyExist(PopupName);

            if (string.IsNullOrWhiteSpace(PopupName))
            {

                ErrorMessage = "Workout name cannot be empty.";
                return false;
            }
            else if (exists && PopupName != WorkoutName)
            {
                ErrorMessage = $"A workoutplan with the name \"{PopupName}\" already exists.";
                return false;
            }
            else
            {
                Workout workout = new() { Id = WorkoutDetails.Id, Name = PopupName, Description = PopupDescription, PreMade = 0 };
                await _workoutRepository.PostWorkoutPlanAsync(workout);

                WorkoutName = PopupName;
                WorkoutDescription = PopupDescription;

                return true;
            }
        }

        [RelayCommand]
        public void DeclineEditWorkout()
        {
            ErrorMessage = string.Empty;
        }

        [RelayCommand]
        async Task DeleteWorkout(Workout workout)
        {
            bool result = await Shell.Current.DisplayAlert("Delete", $"Are you sure you want to delete {workout.Name}?", "OK", "Cancel");

            if (result)
            {
                if (workout == null) return;
                await _workoutRepository.DeleteWorkout(workout);
                await GoBackAsync();
            }
        }

        #endregion


        #region SetsAndReps Popup

        [RelayCommand]
        public async Task SaveSetsAndReps()
        {
            if (IsBusy) return;

            await MopupService.Instance.PopAsync();

            try
            {
                IsBusy = true;

                var existingExercise = Exercises.FirstOrDefault(e => e.WorkoutId == ExerciseToEdit.WorkoutId);

                if (existingExercise != null)
                {
                    int index = Exercises.IndexOf(existingExercise);

                    // Remove the existing exercise from the list
                    Exercises.Remove(existingExercise);

                    // Modify the exercise
                    existingExercise.Sets = ExerciseToEdit.Sets;
                    existingExercise.Reps = ExerciseToEdit.Reps;

                    // Add the modified exercise back at the correct index
                    Exercises.Insert(index, existingExercise);
                }

                // Edit the exercise in the repository
                await _workoutExercisesRepository.EditExerciseInWorkout(WorkoutDetails.Id, ExerciseToEdit);

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error!", $"Unable to edit the exercise {ex.Message}", "OK");
                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion


        #region Navigation

        [RelayCommand]
        public async Task AddExercises()
        {
            await Task.Delay(100); // Add a short delay
            CachedData.SharedWorkout = WorkoutDetails;
            await Shell.Current.GoToAsync($"{nameof(CategoryPage)}");
        }

        [RelayCommand]
        async Task StartWorkout()
        {
            if (WorkoutDetails == null) return;

            await Task.Delay(100); // Add a short delay
            await Shell.Current.GoToAsync(nameof(StartedWorkoutPage), true, new Dictionary<string, object>
            {
                { "Workout", WorkoutDetails }
            });
        }

        #endregion
    }
}
