﻿using BodyBuddy.Models;
using BodyBuddy.Repositories;
using BodyBuddy.Views.WorkoutViews;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using BodyBuddy.Dtos;
using BodyBuddy.Services;

namespace BodyBuddy.ViewModels.WorkoutViewModels
{
    public partial class WorkoutViewModel : BaseViewModel
    {
        private readonly IWorkoutService _workoutService;
        private readonly IWorkoutExercisesService _workoutExercisesService;

        [ObservableProperty] private List<WorkoutDto> _workoutList = new();
        private List<ExerciseDto> Exercises { get; set; } = new();

        [ObservableProperty]
        private bool _isPreMadeWorkout;

        [ObservableProperty]
        public string workoutName, workoutDescription;

        [ObservableProperty]
        public string errorMessage;

        public WorkoutViewModel(IWorkoutService workoutService, IWorkoutExercisesService workoutExercisesService)
        {
            Title = string.Empty;

            _workoutService = workoutService;
            _workoutExercisesService = workoutExercisesService;
        }


        [RelayCommand]
        public async Task GetWorkoutPlans()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                WorkoutList = await _workoutService.GetWorkoutPlans(IsPreMadeWorkout);
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
        async Task DeleteWorkout(WorkoutDto workout)
        {
            if (workout == null) return;

            bool result = await Shell.Current.DisplayAlert("Delete", $"Are you sure you want to delete {workout.Name}?", "OK", "Cancel");

            if (result)
            {
                bool deleted = await _workoutService.DeleteWorkout(workout);
                if (deleted)
                {
                    WorkoutList.Remove(workout);
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
                WorkoutDto workout = new() { Name = WorkoutName, Description = WorkoutDescription, PreMade = false };
                await _workoutService.SaveWorkoutData(workout);
                WorkoutList.Add(workout);

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
            var exists = await _workoutService.DoesWorkoutAlreadyExist(name);

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

                    Exercises.Add(new ExerciseDto()
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
            try
            {
                var workout = await _workoutService.GetSpecificWorkoutAsync(WorkoutName);

                foreach (var exercise in Exercises)
                {
                    await _workoutExercisesService.AddExerciseToWorkout(workout.Id, exercise.Id);
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
        public async Task GoToWorkoutDetails(WorkoutDto workout)
        {
            if (workout == null)
                return;

            await Task.Delay(100);
            await Shell.Current.GoToAsync(nameof(WorkoutDetailsPage), true, new Dictionary<string, object>
            {
                { "Workout", workout }
            });
        }
        #endregion
    }
}
