using BodyBuddy.Models;
using BodyBuddy.Repositories;
using BodyBuddy.ViewModels.ExerciseViewModels;
using BodyBuddy.Views.ExerciseViews;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
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

		[ObservableProperty]
		private Workout _workoutDetails;

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
