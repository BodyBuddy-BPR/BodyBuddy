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

        // This method is used to read qr code data and create usable objects from it
        public void ReadQrCodeData(string qrCodeData)
        {
            // Split the data into separate parts based on the delimiter ';'
            string[] parts = qrCodeData.Split(';');

            // Extract workout details
            string workoutIdPart = parts.FirstOrDefault(p => p.StartsWith("WorkoutId:", StringComparison.OrdinalIgnoreCase));
            int workoutId = 0;

            if (workoutIdPart != null)
            {
                string workoutIdString = workoutIdPart.Split(':')[1];
                int.TryParse(workoutIdString, out workoutId);
            }

            // Extract exercise details
            List<Exercise> exercises = new List<Exercise>();

            foreach (var part in parts)
            {
                if (part.StartsWith("ExerciseId:", StringComparison.OrdinalIgnoreCase))
                {
                    string[] exerciseParts = part.Split(',');

                    int exerciseId;
                    int.TryParse(exerciseParts[0].Split(':')[1], out exerciseId);

                    int sets;
                    int.TryParse(exerciseParts[1].Split(':')[1], out sets);

                    int reps;
                    int.TryParse(exerciseParts[2].Split(':')[1], out reps);

                    exercises.Add(new Exercise
                    {
                        Id = exerciseId,
                        Sets = sets,
                        Reps = reps
                        // Add other properties as needed
                    });
                }
            }

            // Now you have the workoutId and a list of exercises
            // You can use this information to create a Workout object with associated exercises
            Workout workout = new Workout
            {
                Id = workoutId,
                // Set other properties as needed
            };

            // Do something with the workout and exercises
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
