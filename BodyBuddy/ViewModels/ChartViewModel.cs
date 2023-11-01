using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.Dtos;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BodyBuddy.ViewModels
{
    public partial class ChartViewModel : BaseViewModel
    {
        [ObservableProperty] private List<ChartData> _data;

        public ChartViewModel()
        {
            Data = CalculateStats();
        }

        private List<ChartData> CalculateStats()
        {
            var stats = new List<ChartData>();

            var groupedByDate = WorkoutDataFromDb.GroupBy(d => d.Date);

            foreach (var group in groupedByDate)
            {
                var max = group.Max(d => d.Weight);
                var min = group.Min(d => d.Weight);
                var avg = group.Average(d => d.Weight);

                stats.Add(new ChartData { Date = group.Key, MaxWeight = max, MinWeight = min, AverageWeight = avg });
            }

            return stats;
        }

        public List<ExerciseRecordsDto> WorkoutDataFromDb { get; set; } = new List<ExerciseRecordsDto>
        {
            new ExerciseRecordsDto { Date = new DateTime(2023, 01, 01), Weight = 26, Set = 1, Reps = 10},
            new ExerciseRecordsDto { Date = new DateTime(2023, 01, 01), Weight = 28, Set = 2, Reps = 10},
            new ExerciseRecordsDto { Date = new DateTime(2023, 01, 01), Weight = 30, Set = 3, Reps = 10},
            new ExerciseRecordsDto { Date = new DateTime(2023, 02, 01), Weight = 28, Set = 1, Reps = 10},
            new ExerciseRecordsDto { Date = new DateTime(2023, 03, 01), Weight = 34, Set = 1, Reps = 10},
        };

    }
}
