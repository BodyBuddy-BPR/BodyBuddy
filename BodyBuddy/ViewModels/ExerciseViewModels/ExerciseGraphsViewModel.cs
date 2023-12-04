using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.Dtos;
using BodyBuddy.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BodyBuddy.ViewModels.ExerciseViewModels
{
    [QueryProperty(nameof(ExerciseDetails), "Exercise")]
    public partial class ExerciseGraphsViewModel : BaseViewModel
    {
        [ObservableProperty] private List<ChartData> _graphData;
        public List<ExerciseRecordsDto> WorkoutDataFromDb { get; set; } = new List<ExerciseRecordsDto>();


        [ObservableProperty]
        private ExerciseDto _exerciseDetails; // Get Exercise from this 
        private readonly IExerciseRecordsService _exerciseRecordsService;


        public ExerciseGraphsViewModel(IExerciseRecordsService exerciseRecordsService)
        {
            _exerciseRecordsService = exerciseRecordsService;
        }

        public async Task GetWorkoutDataForExercise()
        {
            WorkoutDataFromDb = await _exerciseRecordsService.GetAllExerciseRecordsForExercise(ExerciseDetails.Id);

            GraphData = ConvertIntoChartData();
        }

        private List<ChartData> ConvertIntoChartData()
        {
            var stats = new List<ChartData>();

            var groupedByDate = WorkoutDataFromDb.GroupBy(d => d.Date);

            foreach (var group in groupedByDate)
            {
                var max = group.Max(d => d.Weight);
                var min = group.Min(d => d.Weight);
                var avg = group.Average(d => d.Weight);
                var totalReps = group.Sum(d => d.Reps);

                stats.Add(new ChartData { Date = group.Key, MaxWeight = max, MinWeight = min, AverageWeight = avg, TotalReps = totalReps });
            }

            return stats;
        }
    }
}
