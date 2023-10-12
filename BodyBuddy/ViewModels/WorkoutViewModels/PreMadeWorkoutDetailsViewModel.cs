﻿using BodyBuddy.Models;
using BodyBuddy.Repositories;
using BodyBuddy.Views.WorkoutViews;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.ViewModels.WorkoutViewModels
{
    [QueryProperty(nameof(WorkoutDetails), "Workout")]
    public partial class PreMadeWorkoutDetailsViewModel : BaseViewModel
    {
        private readonly IWorkoutExercisesRepository _workoutExercisesRepository;

        [ObservableProperty]
        private Workout _workoutDetails;

        // Displayed Fields
        [ObservableProperty]
        public string workoutDescription;

        public ObservableCollection<Exercise> Exercises { get; set; } = new ObservableCollection<Exercise>();

        public PreMadeWorkoutDetailsViewModel(IWorkoutExercisesRepository workoutExercisesRepository)
        {
            _workoutExercisesRepository = workoutExercisesRepository;
        }

        public async Task Initialize()
        {
            if (string.IsNullOrWhiteSpace(WorkoutDetails.Description))
            {
                WorkoutDetails.Description = "Try giving this workout a description";
            }
            WorkoutDescription = WorkoutDetails.Description;

            await GetExercisesFromWorkout();
        }

        public async Task GetExercisesFromWorkout()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                var workoutPlan = await _workoutExercisesRepository.GetExercisesInWorkout(WorkoutDetails.Id);

                if (Exercises.Count != 0)
                {
                    Exercises.Clear();
                }

                foreach (var exercise in workoutPlan)
                {
                    Exercises.Add(exercise);
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

        #region Navigation

        [RelayCommand]
        async Task StartPreMadeWorkout()
        {
            if (WorkoutDetails == null) return;

            await Task.Delay(100); // Add a short delay
            await Shell.Current.GoToAsync(nameof(StartedWorkoutPage), true, new Dictionary<string, object>
            {
                { "Workout", WorkoutDetails }
            });
        }

        #endregion
    }
}
