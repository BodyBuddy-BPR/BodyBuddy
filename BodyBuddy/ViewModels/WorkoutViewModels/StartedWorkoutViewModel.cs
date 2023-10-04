using BodyBuddy.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.ViewModels.WorkoutViewModels
{
    [QueryProperty(nameof(WorkoutDetails), "Workout")]
    public partial class StartedWorkoutViewModel : BaseViewModel
    {
        // Query field for the started workout
        [ObservableProperty]
        private Workout _workoutDetails;

        public StartedWorkoutViewModel()
        {
            
        }

        public async Task Initialize()
        {

        }

    }
}
