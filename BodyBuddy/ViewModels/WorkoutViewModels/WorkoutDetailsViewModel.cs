using BodyBuddy.Models;
using BodyBuddy.Repositories;
using BodyBuddy.ViewModels.ExerciseViewModels;
using BodyBuddy.Views.ExerciseViews;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Imagekit.Constant;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BodyBuddy.ViewModels.WorkoutViewModels
{
	[QueryProperty(nameof(WorkoutDetails), "Workout")]
	public partial class WorkoutDetailsViewModel : BaseViewModel
	{
		private readonly IWorkoutRepository _workoutRepository;

		// Query field
		[ObservableProperty]
		private Workout _workoutDetails;

		// Displayed Fields
        [ObservableProperty]
        public string workoutName, workoutDescription;

		// Popup fields
		[ObservableProperty]
		public string popupName, popupDescription, errorMessage;

        public ObservableCollection<Exercise> Exercises { get; set; } = new ObservableCollection<Exercise>();

		public WorkoutDetailsViewModel(IWorkoutRepository workoutRepository)
        {
			_workoutRepository = workoutRepository;
		}

		public async Task GetExercisesFromWorkout()
		{
            if (IsBusy) return;

			try
			{
				IsBusy = true;

                WorkoutName = WorkoutDetails.Name;
                WorkoutDescription = WorkoutDetails.Description;

                var workoutPlan = await _workoutRepository.GetExercisesInWorkout(WorkoutDetails.Id, false); // False for user made workouts

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

		[RelayCommand]
		public async Task AddExercises()
		{
            await Task.Delay(100); // Add a short delay
			CachedData.SharedWorkout = WorkoutDetails;
            await Shell.Current.GoToAsync($"{nameof(CategoryPage)}");
        }

        public async Task<bool> SaveWorkout()
        {
            var exists = await _workoutRepository.DoesWorkoutAlreadyExist(PopupName);

            if (string.IsNullOrWhiteSpace(PopupName))
            {

                ErrorMessage = "Workout name cannot be empty.";
                return false;
            }
            else if (exists && PopupName != WorkoutName)
            {
                ErrorMessage = $"A workoutplan with the name \"{PopupName}\" already exists.";
                return false;
            }
            else
            {
                Workout workout = new() { Id = WorkoutDetails.Id , Name = PopupName, Description = PopupDescription, PreMade = 0 };
                await _workoutRepository.PostWorkoutPlanAsync(workout);

                WorkoutName = PopupName;
				WorkoutDescription = PopupDescription;

                return true;
            }
        }

        [RelayCommand]
        public void DeclineEditWorkout()
        {
            ErrorMessage = string.Empty;
        }

        [RelayCommand]
		async Task DeleteWorkout(Workout workout)
		{
			bool result = await Shell.Current.DisplayAlert("Delete", $"Are you sure you want to delete {workout.Name}?", "OK", "Cancel");

			if (result)
			{
				if (workout == null) return;
				await _workoutRepository.DeleteWorkout(workout);
				await GoBackAsync();
			}
		}

	}
}
