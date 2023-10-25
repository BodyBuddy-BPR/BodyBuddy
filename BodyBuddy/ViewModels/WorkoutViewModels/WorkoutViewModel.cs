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
        private readonly IWorkoutExercisesRepository _workoutExercisesRepository;

        public ObservableCollection<WorkoutModel> Workouts { get; set; } = new ObservableCollection<WorkoutModel>();

        [ObservableProperty]
        private bool _isPreMadeWorkout;

        [ObservableProperty]
        public string workoutName, workoutDescription;

        [ObservableProperty]
        public string errorMessage;

        public WorkoutViewModel(IWorkoutRepository workoutRepository, IWorkoutExercisesRepository workoutExercisesRepository)
        {
            Title = string.Empty;

            _workoutRepository = workoutRepository;
            _workoutExercisesRepository = workoutExercisesRepository;
        }


        [RelayCommand]
        public async Task GetWorkoutPlans()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                List<WorkoutModel> workoutPlans;
                // Getting User Workouts
                // TODO: --> Make a DTO in a service class, so this one takes true/false rather than 1 and 0
                // Then map from DTO to DB method and back (change bool to ints and back)
                if (IsPreMadeWorkout)
                    workoutPlans = await _workoutRepository.GetWorkoutPlansAsync(1); // 0 for premade workouts
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
        async Task DeleteWorkout(WorkoutModel workout)
        {
            bool result = await Shell.Current.DisplayAlert("Delete", $"Are you sure you want to delete {workout.Name}?", "OK", "Cancel");

            if (result)
            {
                if (workout == null) return;
                bool deleted = await _workoutRepository.DeleteWorkout(workout);
                if (deleted)
                {
                    Workouts.Remove(workout);
                }
            }
        }

        #region Create Workout Popup

        public async Task<bool> CreateWorkout()
        {
            var valid = await ValidWorkout(WorkoutName);

            if (!valid) return false;

            try
            {
                WorkoutModel workout = new() { Name = WorkoutName, Description = WorkoutDescription, PreMade = 0 };
                await _workoutRepository.PostWorkoutPlanAsync(workout);
                Workouts.Add(workout);

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error!", $"Unable to get create workout{ex.Message}", "OK");
            }
            return true;
        }

        public async Task<bool> ValidWorkout(string name)
        {
            var exists = await _workoutRepository.DoesWorkoutAlreadyExist(name);

            if (string.IsNullOrWhiteSpace(name))
            {
                ErrorMessage = "Workout name cannot be empty.";
                return false;
            }
            else if (exists)
            {
                ErrorMessage = $"A workoutplan with the name \"{name}\" already exists.";
                return false;
            }
            return true;
        }

        [RelayCommand]
        public void DeclineCreateWorkout()
        {
            WorkoutName = string.Empty;
            WorkoutDescription = string.Empty;
            ErrorMessage = string.Empty;
        }

        // This method is used to read qr code data and create usable objects from it
        public List<ExerciseModel> Exercises { get; set; } = new List<ExerciseModel>();
        public void ReadQrCodeData(string qrCodeData)
        {
            // Unescape the values before splitting
            qrCodeData = Unescape(qrCodeData);

            // Split the data into separate parts based on the delimiter ';'
            string[] parts = qrCodeData.Split(';');

            // Extract workout details
            string workoutNamePart = parts.FirstOrDefault(p => p.StartsWith("WorkoutName:", StringComparison.OrdinalIgnoreCase));
            string workoutDescriptionPart = parts.FirstOrDefault(p => p.StartsWith("WorkoutDescription:", StringComparison.OrdinalIgnoreCase));

            string workoutName = workoutNamePart?.Split(':')[1];
            string workoutDescription = workoutDescriptionPart?.Split(':')[1];

            // Extract exercise details
            //List<Exercise> exercises = new List<Exercise>();

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

                    Exercises.Add(new ExerciseModel
                    {
                        Id = exerciseId,
                        Sets = sets,
                        Reps = reps
                    });
                }
            }

            WorkoutName = workoutName;
            WorkoutDescription = workoutDescription;
        }
        private string Unescape(string value)
        {
            // Replace the placeholder with ';'
            return value?.Replace("##semicolon##", ";") ?? "";
        }

        public async Task AddExercisesToWorkout()
        {
            //List<Exercise> exerciseList = new();

            //exerciseList.AddRange(Exercises);

            try
            {
                var workout = await _workoutRepository.GetSpecificWorkoutAsync(WorkoutName);

                foreach (var exercise in Exercises)
                {
                    await _workoutExercisesRepository.AddExerciseToWorkout(workout.Id, exercise);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error!", $"Unable to get add exercises to the workout{ex.Message}", "OK");
            }
            finally
            {
                Exercises.Clear();
            }
        }

        #endregion 


        #region Navigation

        [RelayCommand]
        public async Task GoToWorkoutDetails(WorkoutModel workout)
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
