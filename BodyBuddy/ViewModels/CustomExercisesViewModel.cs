using BodyBuddy.Database;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BodyBuddy.ViewModels
{
    public partial class CustomExercisesViewModel : BaseViewModel
    {
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IWorkoutPlanRepository _workoutPlanRepository;

        [ObservableProperty]
        private bool _isRefreshing;

        [ObservableProperty]
        private Exercise _selectedExercise;

        [ObservableProperty]
        private WorkoutPlan _selectedWorkoutPlan;

        public ObservableCollection<CustomExercise> ExercisesList { get; set; } = new ObservableCollection<CustomExercise>();

        public CustomExercisesViewModel(IExerciseRepository exerciseRepository, IWorkoutPlanRepository workoutPlanRepository)
        {
            Title = string.Empty;

            _exerciseRepository = exerciseRepository;
            _workoutPlanRepository = workoutPlanRepository;

        }

        [RelayCommand]
        public async Task GetMyExercises()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                //var exercises = await _database.GetItemsAsync();    
                var exercises = await _exerciseRepository.GetCustomExercisesAsync();

                if (ExercisesList.Count != 0)
                {
                    ExercisesList.Clear();
                }

                foreach (var exercise in exercises)
                {
                    ExercisesList.Add(exercise);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error!", $"Unable to get exercises {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }
        }

        public async Task GetWorkoutplans()
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



        public ObservableCollection<WorkoutPlan> SelectedWorkoutPlans { get; set; } = new ObservableCollection<WorkoutPlan>();

        public void SelectedItemsChanged(object plan)
        {
            SelectedWorkoutPlans.Add((WorkoutPlan)plan);
        }

        [RelayCommand]
        public async Task AddToWorkout()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                foreach (var plan in SelectedWorkoutPlans)
                {
                    //await _database.AddExerciseToWorkoutPlan
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
        public async Task NewExercise()
        {
            await Shell.Current.GoToAsync($"{nameof(NewExercisePage)}");
        }

     
    }
}
