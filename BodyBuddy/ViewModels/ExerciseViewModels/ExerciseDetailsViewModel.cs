using BodyBuddy.Models;
using BodyBuddy.Repositories;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Diagnostics;
using BodyBuddy.Dtos;
using BodyBuddy.Services;

namespace BodyBuddy.ViewModels.ExerciseViewModels
{
    [QueryProperty(nameof(ExerciseDetails), "Exercise")]
    public partial class ExerciseDetailsViewModel : BaseViewModel
    {
        private readonly IExerciseService _exerciseService;
        private readonly IConnectivity _connectivity;

        [ObservableProperty]
        private ExerciseDto _exerciseDetails;
        public ObservableCollection<string> ExerciseImages { get; set; } = new();


        public ExerciseDetailsViewModel(IExerciseService exerciseService, IConnectivity connectivity)
        {
            _exerciseService = exerciseService;
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

                ExerciseDetails = await _exerciseService.GetExerciseDetails(ExerciseDetails.Id);

                // Make sure the secondary muscles fit in the ellipse to show these
                if(ExerciseDetails.SecondaryMuscles.Length >= 16)
                {
                    ExerciseDetails.SecondaryMuscles = ExerciseDetails.SecondaryMuscles.Substring(0, ExerciseDetails.SecondaryMuscles.LastIndexOf(',', 16));
                }

                // Populate ExerciseImages
                PopulateExerciseImagesList(ExerciseDetails.Images);
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

        #endregion
    }
}
