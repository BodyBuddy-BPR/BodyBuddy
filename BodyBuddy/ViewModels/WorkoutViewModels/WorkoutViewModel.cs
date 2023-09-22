using BodyBuddy.Models;
using BodyBuddy.Repositories;
using BodyBuddy.Views.WorkoutViews;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BodyBuddy.ViewModels.WorkoutViewModels
{
    public partial class WorkoutViewModel : BaseViewModel
    {
        private readonly IWorkoutRepository _workoutRepository;

        public ObservableCollection<Workout> Workouts { get; set; } = new ObservableCollection<Workout>();

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
        public async Task CreateWorkout()
        {

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
