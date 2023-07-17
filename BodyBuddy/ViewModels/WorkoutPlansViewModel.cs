using BodyBuddy.Database;
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
        private LocalDatabase _database;

        [ObservableProperty]
        private string _workoutName;

        public ObservableCollection<WorkoutPlan> WorkoutPlans { get; set; } = new ObservableCollection<WorkoutPlan>();

        public WorkoutPlansViewModel(LocalDatabase localDatabase)
        {
            Title = string.Empty;

            _database = localDatabase;
        }

        [RelayCommand]
        public async Task GetMyWorkoutPlans()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                var workoutPlans = await _database.GetWorkoutPlansAsync();

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
            WorkoutPlan workoutPlan = new()
            {
                Name = WorkoutName,
            };

            await _database.SaveWorkoutPlanAsync(workoutPlan);

            await GoBackAsync();
        }
    }
}
