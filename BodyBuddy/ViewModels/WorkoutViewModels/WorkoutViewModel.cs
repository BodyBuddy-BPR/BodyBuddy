using BodyBuddy.Models;
using BodyBuddy.Repositories;
using BodyBuddy.Views.WorkoutViews;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BodyBuddy.ViewModels.WorkoutViewModels
{
    public partial class WorkoutViewModel : BaseViewModel
    {
        private readonly IWorkoutRepository _workoutRepository;

        public ObservableCollection<Workout> MyWorkouts { get; set; } = new ObservableCollection<Workout>();
        public ObservableCollection<Workout> PreMadeWorkouts { get; set; } = new ObservableCollection<Workout>();

        [ObservableProperty]
        private bool isPremade;

        [ObservableProperty]
        public string workoutName, workoutDescription;

        [ObservableProperty]
        public string errorMessage;

        public WorkoutViewModel(IWorkoutRepository workoutRepository)
        {
            Title = string.Empty;

            _workoutRepository = workoutRepository;
            IsPremade = false;

        }


        [RelayCommand]
        public async Task GetWorkoutPlans()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                // Getting Premade Workouts
                var preMadeWorkoutPlans = await _workoutRepository.GetWorkoutPlansAsync(1); // 1 for premade workouts

                if (PreMadeWorkouts.Count != 0)
                {
                    PreMadeWorkouts.Clear();
                }
                foreach (var workoutPlan in preMadeWorkoutPlans)
                {
                    PreMadeWorkouts.Add(workoutPlan);
                }

                // Getting User Workouts
                var workoutPlans = await _workoutRepository.GetWorkoutPlansAsync(0); // 0 for user made workouts

                if (MyWorkouts.Count != 0)
                {
                    MyWorkouts.Clear();
                }
                foreach (var workoutPlan in workoutPlans)
                {
                    MyWorkouts.Add(workoutPlan);
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
        async Task DeleteWorkout(Workout workout)
        {
            bool result = await Shell.Current.DisplayAlert("Delete", $"Are you sure you want to delete {workout.Name}?", "OK", "Cancel");

            if (result)
            {
                if (workout == null) return;
                await _workoutRepository.DeleteWorkout(workout);
                MyWorkouts.Remove(workout);
            }
        }

        #region Create Workout Popup

        public async Task<bool> CreateWorkout()
        {
            var exists = await _workoutRepository.DoesWorkoutAlreadyExist(WorkoutName);

            if (string.IsNullOrWhiteSpace(WorkoutName))
            {

                ErrorMessage = "Workout name cannot be empty.";
                return false;
            }
            else if (exists)
            {
                ErrorMessage = $"A workoutplan with the name \"{WorkoutName}\" already exists.";
                return false;
            }
            else
            {
                Workout workout = new() { Name = WorkoutName, Description = WorkoutDescription, PreMade = 0 };
                await _workoutRepository.PostWorkoutPlanAsync(workout);
                MyWorkouts.Add(workout);

                WorkoutName = string.Empty;
                WorkoutDescription = string.Empty;
                return true;
            }
        }

        [RelayCommand]
        public void DeclineCreateWorkout()
        {
            WorkoutName = string.Empty;
            WorkoutDescription = string.Empty;
            ErrorMessage = string.Empty;
        }

        #endregion 

        #region Navigation

        [RelayCommand]
        public async Task GoToWorkoutDetails(Workout workout)
        {
            if (workout == null)
            {
                return;
            }

            await Task.Delay(100);
            await Shell.Current.GoToAsync(nameof(WorkoutDetailsPage), true, new Dictionary<string, object>
            {
                { "Workout", workout }
            });
        }
        #endregion
    }
}
