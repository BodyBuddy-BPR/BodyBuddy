using BodyBuddy.Models;
using BodyBuddy.Repositories;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.ViewModels.ExerciseViewModels
{
    [QueryProperty(nameof(ExerciseDetails), "Exercise")]
    public partial class ExerciseDetailsViewModel : BaseViewModel
    {
        private readonly IExerciseRepository _exerciseRepository;
        private IConnectivity _connectivity;

        [ObservableProperty]
        private Exercise _exerciseDetails;
        public ObservableCollection<string> ExerciseImages { get; set; } = new ObservableCollection<string>();


        public ExerciseDetailsViewModel(IExerciseRepository exerciseRepository, IConnectivity connectivity)
        {
            _exerciseRepository = exerciseRepository;
            _connectivity = connectivity;
        }

        public async Task GetExerciseDetails()
        {
            if (IsBusy) return;

            try
            {
                if (_connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await Shell.Current.DisplayAlert("Internet Issue!", "Check your internet and try again", "OK");
                    return;
                }
                IsBusy = true;

                var exercise = await _exerciseRepository.GetExerciseDetails(ExerciseDetails.Id);
                ProcessExerciseDetails(exercise);
                ExerciseDetails = exercise;

                // Populate ExerciseImages
                PopulateExerciseImagesList(exercise.Images);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error!", $"Unable to get exercise details {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }


        #region Helper methods

        // Method to populate ExerciseImages list from the images string
        private void PopulateExerciseImagesList(string images)
        {
            ExerciseImages.Clear();
            if (!string.IsNullOrEmpty(images))
            {
                string[] imagePaths = images.Split(',');
                foreach (string path in imagePaths)
                {
                    ExerciseImages.Add(path.Trim()); // Trim to remove any leading/trailing whitespace
                }
            }
        }

        // If any values in the exercise details response from the repo is null, it converts these to empty string instead
        private void ProcessExerciseDetails(Exercise exercise)
        {
            if (exercise == null)
                return;

            // Check and convert any null properties to empty strings
            exercise.Name ??= string.Empty;
            exercise.Level ??= string.Empty;
            exercise.Category ??= string.Empty;
            exercise.PrimaryMuscles ??= string.Empty;
            exercise.SecondaryMuscles ??= string.Empty;
            exercise.Equipment ??= string.Empty;
            exercise.Force ??= string.Empty;
            exercise.Mechanic ??= string.Empty;
            exercise.Instructions ??= string.Empty;
        }

        #endregion
    }
}
