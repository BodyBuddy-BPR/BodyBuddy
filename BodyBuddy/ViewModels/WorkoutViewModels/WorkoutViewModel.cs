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
        public string workoutName;

        public WorkoutViewModel(IWorkoutRepository workoutRepository)
        {
            Title = string.Empty;

            _workoutRepository = workoutRepository;
        }

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
            }
        }

        [RelayCommand]
		async Task DeleteWorkout(Workout workout)
		{
			bool result = await Shell.Current.DisplayAlert("Delete", $"Are you sure you want to delete {workout.Name}?", "OK", "Cancel");

            if(result)
            {
				if (workout == null) return;
				await _workoutRepository.DeleteWorkout(workout);
				Workouts.Remove(workout);
			}
		}

        [RelayCommand]
        async Task CreateWorkout()
        {
            if (string.IsNullOrWhiteSpace(WorkoutName)) return;
            Workout workout = new Workout {  Name = WorkoutName, PreMade = 0 };
            await _workoutRepository.PostWorkoutPlanAsync(workout);
            Workouts.Add(workout);
            WorkoutName = string.Empty;
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
