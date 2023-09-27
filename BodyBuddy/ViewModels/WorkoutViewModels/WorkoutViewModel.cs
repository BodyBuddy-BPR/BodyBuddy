using BodyBuddy.Models;
using BodyBuddy.Repositories;
using BodyBuddy.Views.WorkoutViews;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Security.Policy;
using System.Windows.Input;

namespace BodyBuddy.ViewModels.WorkoutViewModels
{
    public partial class WorkoutViewModel : BaseViewModel
    {
        private readonly IWorkoutRepository _workoutRepository;

        public ObservableCollection<Workout> Workouts { get; set; } = new ObservableCollection<Workout>();

        [ObservableProperty]
        public string workoutName;

        [ObservableProperty]
        bool isRefreshing;


        [ObservableProperty]
        public string errorMessage;
        [ObservableProperty]
        public bool isErrorVisible = false;

        [ObservableProperty]
        public bool canClose;

        public WorkoutViewModel(IWorkoutRepository workoutRepository)
        {
            Title = string.Empty;

            _workoutRepository = workoutRepository;
            GetWorkoutPlans();
        }

        [RelayCommand]
        public async Task GetWorkoutPlans()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                var workoutPlans = await _workoutRepository.GetWorkoutPlansAsync(0); // 0 for user made workouts

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
                IsRefreshing = false;
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



        [RelayCommand]
        async Task CreateWorkout()
        {
            var exists = await _workoutRepository.DoesWorkoutAlreadyExist(WorkoutName);

            if (string.IsNullOrWhiteSpace(WorkoutName))
            {
                CanClose = false;
                IsErrorVisible = true;

                ErrorMessage = "Workout name cannot be empty.";
            }
            else if (exists)
            {
                CanClose = false;

                IsErrorVisible = true;
                ErrorMessage = $"A workoutplan with the name \"{WorkoutName}\" already exists.";

            }
            else
            {
                Workout workout = new() { Name = WorkoutName, PreMade = 0 };
                await _workoutRepository.PostWorkoutPlanAsync(workout);
                Workouts.Add(workout);

                WorkoutName = string.Empty;
                IsErrorVisible = false;

                CanClose = true;

            }
        }

        [RelayCommand]
        public void DeclineCreateWorkout()
        {
            WorkoutName = string.Empty;
            ErrorMessage = string.Empty;
            IsErrorVisible = false;
            CanClose = true;

        }

        [RelayCommand]
        public async Task GoToWorkoutDetails(Workout workout)
        {
            if (workout == null)
            {
                return;
            }

            await Shell.Current.GoToAsync(nameof(WorkoutDetailsPage), true, new Dictionary<string, object>
            {
                { "Workout", workout }
            });
        }
    }
}
