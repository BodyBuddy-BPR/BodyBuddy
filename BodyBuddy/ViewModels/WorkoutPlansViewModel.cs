using BodyBuddy.Database;
using BodyBuddy.Models;
using BodyBuddy.Repositories;
using BodyBuddy.Views;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.ViewModels
{
    public partial class WorkoutPlansViewModel : BaseViewModel
    {
        private readonly IWorkoutPlanRepository _workoutPlanRepository;

        [ObservableProperty]
        private string _workoutName;

        public ObservableCollection<Workout> WorkoutPlans { get; set; } = new ObservableCollection<Workout>();

        public WorkoutPlansViewModel(IWorkoutPlanRepository workoutPlanRepository)
        {
            Title = string.Empty;

            _workoutPlanRepository = workoutPlanRepository;
        }

        [RelayCommand]
        public async Task GetMyWorkoutPlans()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                var workoutPlans = await _workoutPlanRepository.GetWorkoutPlansAsync();

                if (WorkoutPlans.Count != 0)
                {
                    WorkoutPlans.Clear();
                }

                foreach (var plan in workoutPlans)
                {
                    WorkoutPlans.Add(plan);
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
        public async Task SaveWorkoutPlan()
        {
            if (string.IsNullOrWhiteSpace(WorkoutName)) return;

            Workout workoutPlan = new()
            {
                Name = WorkoutName
            };

            WorkoutPlans.Add(workoutPlan);
            await _workoutPlanRepository.SaveWorkoutPlanAsync(workoutPlan);

            WorkoutName = string.Empty;
        }

        [RelayCommand]
        async Task GoToWorkoutplanDetails(WorkoutPlan workoutPlan)
        {
            if (workoutPlan == null)
                return;

            await Shell.Current.GoToAsync(nameof(WorkoutPlanDetailsPage), true, new Dictionary<string, object>
            {
            {"WorkoutPlan", workoutPlan }
        });
        }

    }
}
