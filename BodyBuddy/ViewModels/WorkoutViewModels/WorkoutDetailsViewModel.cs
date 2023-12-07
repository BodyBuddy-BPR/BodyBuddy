using BodyBuddy.Views.ExerciseViews;
using BodyBuddy.Views.WorkoutViews;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mopups.Services;
using System.Diagnostics;
using BodyBuddy.Dtos;
using BodyBuddy.Services;
using BodyBuddy.Helpers;

namespace BodyBuddy.ViewModels.WorkoutViewModels
{
    [QueryProperty(nameof(WorkoutDetails), "Workout")]
    public partial class WorkoutDetailsViewModel : BaseViewModel
    {
        private readonly IWorkoutService _workoutService;
        private readonly IWorkoutExercisesService _workoutExercisesService;

        #region ObservableProperties

        // Query field
        [ObservableProperty] private WorkoutDto _workoutDetails;
        private WorkoutDto _previousWorkoutDetails;

        // Exercises to show
        [ObservableProperty] private List<ExerciseDto> _exercises = new();

        // Workout Popup fields
        [ObservableProperty]
        private string _popupName, _popupDescription, _errorMessage;

        // Exercise Popup fields
        [ObservableProperty]
        private int _editSets, _editReps, _editWorkoutExerciseId;

        // Visibility
        [ObservableProperty]
        private bool _smallButtonsIsEnabled; // This is the small Add Exercise & Start Workout buttons
        [ObservableProperty]
        private bool _largeButtonModifyIsEnabled, _smallButtonModifyIsEnabled; // For the AddExercisesCommand 

        #endregion


        public WorkoutDetailsViewModel(IWorkoutService workoutService, IWorkoutExercisesService workoutExercisesService)
        {
            _workoutService = workoutService;
            _workoutExercisesService = workoutExercisesService;
        }

        public async Task Initialize()
        {
            if (string.IsNullOrWhiteSpace(WorkoutDetails.Description))
            {
                WorkoutDetails.Description = "Try giving this workout a description";
            }

            await Task.Run(GetExercisesFromWorkout);

            //Setting up Visibility of small and big buttons
            if (Exercises.Count > 0)
            {
                LargeButtonModifyIsEnabled = (false);
                SmallButtonModifyIsEnabled = (!WorkoutDetails.PreMade);
                SmallButtonsIsEnabled = true;
            }
            else
            {
                LargeButtonModifyIsEnabled = (!WorkoutDetails.PreMade);
                SmallButtonModifyIsEnabled = (false);
                SmallButtonsIsEnabled = false;
            }
        }

        public async Task GetExercisesFromWorkout()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                if (!WorkoutDetails.Equals(_previousWorkoutDetails))
                {
                    Exercises = await _workoutExercisesService.GetExercisesInWorkout(WorkoutDetails.Id);
                    _previousWorkoutDetails = WorkoutDetails;
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
        async Task DeleteFromWorkout(ExerciseDto exercise)
        {
            if (exercise == null) return;

            bool result = await Shell.Current.DisplayAlert("Delete", $"Are you sure you want to remove {exercise.Name} from this workout?", "OK", "Cancel");

            if (result)
            {
                await _workoutExercisesService.DeleteExerciseFromWorkout(WorkoutDetails.Id, exercise.Id);
                Exercises.Remove(exercise);

                if (Exercises.Count == 0)
                {
                    SmallButtonModifyIsEnabled = false;
                    LargeButtonModifyIsEnabled = !WorkoutDetails.PreMade;
                }
            }
        }

        #region Workout Popup

        public async Task<bool> SaveWorkout()
        {
            var exists = await _workoutService.DoesWorkoutAlreadyExist(PopupName);

            if (string.IsNullOrWhiteSpace(PopupName))
            {

                ErrorMessage = "Workout name cannot be empty.";
                return false;
            }

            if (exists && PopupName != WorkoutDetails.Name)
            {
                ErrorMessage = $"A workoutplan with the name \"{PopupName}\" already exists.";
                return false;
            }

            WorkoutDto workout = new() { Id = WorkoutDetails.Id, Name = PopupName, Description = PopupDescription, PreMade = false };
            await _workoutService.SaveWorkoutData(workout);

            WorkoutDetails.Name = PopupName;
            WorkoutDetails.Description = PopupDescription;

            return true;
        }

        [RelayCommand]
        public void DeclineEditWorkout()
        {
            ErrorMessage = string.Empty;
        }

        [RelayCommand]
        async Task DeleteWorkout(WorkoutDto workout)
        {
            if (workout == null) return;

            bool result = await Shell.Current.DisplayAlert("Delete", $"Are you sure you want to delete {workout.Name}?", "OK", "Cancel");

            if (result)
            {
                await _workoutService.DeleteWorkout(workout);
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

                var existingExercise = Exercises.FirstOrDefault(e => e.WorkoutExerciseId == EditWorkoutExerciseId);
                if (existingExercise != null)
                {
                    existingExercise.Sets = EditSets;
                    existingExercise.Reps = EditReps;
                    existingExercise.WorkoutId = WorkoutDetails.Id;

                    await _workoutExercisesService.EditExerciseInWorkout(existingExercise);
                }
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


        #region QR Code Generation 

        public string GenerateQrCodeData()
        {
            return QrCodeGenerator.GenerateWorkoutCode(WorkoutDetails, Exercises.ToList());
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
            await Task.Delay(100); // Add a short delay
            await Shell.Current.GoToAsync(nameof(StartedWorkoutPage), true, new Dictionary<string, object>
            {
                { "Workout", WorkoutDetails }
            });
        }

        [RelayCommand]
        async Task ToExerciseDetails(ExerciseDto exercise)
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
