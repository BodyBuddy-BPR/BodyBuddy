using BodyBuddy.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.ViewModels
{
    [QueryProperty(nameof(WorkoutPlan), "WorkoutPlan")]
    public partial class WorkoutPlanDetailsViewModel : BaseViewModel
    {
        [ObservableProperty]
        private WorkoutPlan _workoutPlan;

        public ObservableCollection<Exercise> ExercisesList { get; set; } = new ObservableCollection<Exercise>();


        public WorkoutPlanDetailsViewModel()
        {

        }
    }
}
