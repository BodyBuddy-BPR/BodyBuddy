using BodyBuddy.Models;
using BodyBuddy.Repositories;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BodyBuddy.ViewModels.WorkoutViewModels
{
	[QueryProperty(nameof(Workout), "Workout")]
	public partial class WorkoutDetailsViewModel : BaseViewModel
	{
		private readonly IWorkoutRepository _workoutRepository;

		[ObservableProperty]
		private Workout workout;

		public ObservableCollection<Exercise> Exercises { get; set; } = new ObservableCollection<Exercise>();

		public WorkoutDetailsViewModel(IWorkoutRepository workoutRepository)
        {
			Title = Workout.Name;

			_workoutRepository = workoutRepository;
		}

		public async Task GetExercisesFromWorkout()
		{
			if (IsBusy) return;

			try
			{
				IsBusy = true;

				var workoutPlan = await _workoutRepository.GetExercisesFromWorkoutId(Workout.Id);

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
    }
}
