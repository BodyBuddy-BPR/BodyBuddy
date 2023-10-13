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

        public ObservableCollection<Workout> Workouts { get; set; } = new ObservableCollection<Workout>();

        [ObservableProperty]
        private bool _isPreMadeWorkout;

        [ObservableProperty]
        public string workoutName, workoutDescription;

        [ObservableProperty]
        public string errorMessage;

        public WorkoutViewModel(IWorkoutRepository workoutRepository)
        {
            Title = string.Empty;

            _workoutRepository = workoutRepository;
        }


        [RelayCommand]
        public async Task GetWorkoutPlans()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                List<Workout> workoutPlans;
                // Getting User Workouts
                // TODO: --> Make a DTO in a service class, so this one takes true/false rather than 1 and 0
                // Then map from DTO to DB method and back (change bool to ints and back)
                if (IsPreMadeWorkout)
                    workoutPlans = await _workoutRepository.GetWorkoutPlansAsync(1); // 0 for user made workouts
                else
                    workoutPlans = await _workoutRepository.GetWorkoutPlansAsync(0); // 0 for user made workouts

                if (Workouts.Count != 0)
                {
                    Workouts.Clear();
                }
                foreach (var workoutPlan in workoutPlans)
                {
                    Workouts.Add(workoutPlan);
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
                Workouts.Remove(workout);
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
                Workouts.Add(workout);

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
