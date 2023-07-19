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
    public partial class MyExercisesViewModel : BaseViewModel
    {
        private LocalDatabase _database;

        [ObservableProperty]
        private bool _isRefreshing;

        [ObservableProperty]
        private Exercise _selectedExercise;

        public ObservableCollection<Exercise> ExercisesList { get; set; } = new ObservableCollection<Exercise>();


        public MyExercisesViewModel(LocalDatabase localDatabase)
        {
            Title = string.Empty;

            _database = localDatabase;
            InitializeExerciseData();
        }

        [RelayCommand]
        public async Task GetMyExercises()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                var exercises = await _database.GetItemsAsync();

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

        #region Generate dummy data for testing
        // Populates the ObservableCollection Exercises with exercises
        private void InitializeExerciseData()
        {
            List<Exercise> exerciseData = new List<Exercise>
            {
                // Chest
                new Exercise { Name = "Bench Press", MuscleGroup = "Chest" },
                new Exercise { Name = "Dumbbell Chest Press", MuscleGroup = "Chest" },

                // Shoulders
                new Exercise { Name = "Overhead Press", MuscleGroup = "Shoulders" },
                new Exercise { Name = "Barbell Front Raise", MuscleGroup = "Shoulders" },

                // Biceps
                new Exercise { Name = "Barbell Curl", MuscleGroup = "Biceps" },
                new Exercise { Name = "Dumbbell Curl", MuscleGroup = "Biceps" },

                // Triceps
                new Exercise { Name = "Barbell Lying Triceps Extension", MuscleGroup = "Triceps" },
                new Exercise { Name = "Tricep Pushdown With Bar", MuscleGroup = "Triceps" },

                // Legs
                new Exercise { Name = "Squats", MuscleGroup = "Legs" },
                new Exercise { Name = "Leg Press", MuscleGroup = "Legs" },

                // Back
                new Exercise { Name = "Deadlift", MuscleGroup = "Back" },
                new Exercise { Name = "Pull-Up", MuscleGroup = "Back" },
            };

            foreach (Exercise exercise in exerciseData)
            {
                ExercisesList.Add(exercise);
            }
        }
        #endregion
    }
}
