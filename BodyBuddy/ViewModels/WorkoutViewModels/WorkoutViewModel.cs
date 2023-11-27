using BodyBuddy.Models;
using BodyBuddy.Repositories;
using BodyBuddy.Views.WorkoutViews;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using BodyBuddy.Dtos;
using BodyBuddy.Helpers;
using BodyBuddy.Services;
using ZXing.QrCode;
using Microsoft.VisualBasic;

namespace BodyBuddy.ViewModels.WorkoutViewModels
{
    public partial class WorkoutViewModel : BaseViewModel
    {
        private readonly IWorkoutService _workoutService;
        private readonly IWorkoutExercisesService _workoutExercisesService;
        private readonly IStartupTestService _startupTestService;

        [ObservableProperty] private ObservableCollection<WorkoutDto> _workoutList = new();
        private StartupTestDto startupTestDto;
        private List<ExerciseDto> Exercises { get; set; } = new(); // Used for adding exercises from scanned workouts

        [ObservableProperty]
        private bool _isPreMadeWorkout = true;

        [ObservableProperty]
        public string workoutName, workoutDescription;

        [ObservableProperty]
        public string errorMessage;

        public WorkoutViewModel(IWorkoutService workoutService, IWorkoutExercisesService workoutExercisesService, IStartupTestService startupTestService)
        {
            Title = string.Empty;

            _workoutService = workoutService;
            _workoutExercisesService = workoutExercisesService;
            _startupTestService = startupTestService;
        }


        [RelayCommand]
        public async Task GetWorkoutPlans()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                startupTestDto = await _startupTestService.GetStartupTestData();
                string[] targetAreas = startupTestDto.TargetAreas.Split(new string[] { ", " }, StringSplitOptions.None);

                var tempWorkoutList = new ObservableCollection<WorkoutDto>(await _workoutService.GetWorkoutPlans(IsPreMadeWorkout));

                foreach (string area in targetAreas)
                {
                    IEnumerable<WorkoutDto> matchingWorkouts = tempWorkoutList.Where(workout =>
                        workout.Name.IndexOf(area, StringComparison.OrdinalIgnoreCase) >= 0);

                    foreach (var matchingWorkout in matchingWorkouts)
                    {
                        WorkoutList.Add(matchingWorkout);
                    }
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
        async Task DeleteWorkout(WorkoutDto workout)
        {
            if (workout == null) return;

            bool result = await Shell.Current.DisplayAlert("Delete", $"Are you sure you want to delete {workout.Name}?", "OK", "Cancel");

            if (result)
            {
                bool deleted = await _workoutService.DeleteWorkout(workout);
                if (deleted)
                {
                    WorkoutList.Remove(workout);
                }
            }
        }


        #region Create Workout Popup

        public async Task<bool> CreateWorkout()
        {
            var valid = await ValidWorkout(WorkoutName);

            if (!valid) return false;

            try
            {
                WorkoutDto workout = new() { Name = WorkoutName, Description = WorkoutDescription, PreMade = false };
                await _workoutService.SaveWorkoutData(workout);
                WorkoutList.Add(workout);

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error!", $"Unable to get create workout{ex.Message}", "OK");
            }
            return true;
        }

        public async Task<bool> ValidWorkout(string name)
        {
            var exists = await _workoutService.DoesWorkoutAlreadyExist(name);

            if (string.IsNullOrWhiteSpace(name))
            {
                ErrorMessage = "Workout name cannot be empty.";
                return false;
            }
            else if (exists)
            {
                ErrorMessage = $"A workoutplan with the name \"{name}\" already exists.";
                return false;
            }
            return true;
        }

        [RelayCommand]
        public void DeclineCreateWorkout()
        {
            WorkoutName = string.Empty;
            WorkoutDescription = string.Empty;
            ErrorMessage = string.Empty;
        }

        // This method is used to read qr code data and create usable objects from it
        public void ReadQrCodeData(string qrCodeData)
        {
            var exercisesInWorkout = QrCodeGenerator.ReadWorkoutCode(qrCodeData, SetPropertyValues);
            Exercises.AddRange(exercisesInWorkout);
        }
        private void SetPropertyValues(string key, string value)
        {
            switch (key)
            {
                case QrCodeConstants.WorkoutName:
                    WorkoutName = value;
                    break;
                case QrCodeConstants.WorkoutDescription:
                    WorkoutDescription = value;
                    break;
            }
        }

        public async Task AddExercisesToWorkout()
        {
            try
            {
                var workout = await _workoutService.GetSpecificWorkoutAsync(WorkoutName);

                foreach (var exercise in Exercises)
                {
                    await _workoutExercisesService.AddExerciseToWorkout(workout.Id, exercise.Id);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error!", $"Unable to get add exercises to the workout{ex.Message}", "OK");
            }
            finally
            {
                Exercises.Clear();
            }
        }

        #endregion 


        #region Navigation

        [RelayCommand]
        public async Task GoToWorkoutDetails(WorkoutDto workout)
        {
            if (workout == null)
                return;

            await Shell.Current.GoToAsync(nameof(WorkoutDetailsPage), true, new Dictionary<string, object>
            {
                { "Workout", workout }
            });
        }
        #endregion
    }
}
