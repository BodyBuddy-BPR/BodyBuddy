using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.ViewModels
{
    [QueryProperty(nameof(ExerciseDetails), "Exercise")]
    public partial class ExerciseDetailsViewModel : BaseViewModel
    {
        private readonly IExerciseRepository _exerciseRepository;
        private IConnectivity _connectivity;

        [ObservableProperty]
        private Exercise _exerciseDetails;
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

                ExerciseDetails = await _exerciseRepository.GetExerciseDetailsAsync(ExerciseDetails.Id);
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
    }
}
