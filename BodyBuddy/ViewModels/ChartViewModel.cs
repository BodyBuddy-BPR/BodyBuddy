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
        public List<ExerciseRecordsDto> WorkoutDataFromDb { get; set; } = new List<ExerciseRecordsDto>();


        public ChartViewModel()
        {
            //Random data simulation
            GeneratedDataSimulation(3, 1);
            //HardcodeDataSimulation();

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
                var totalReps = group.Sum(d => d.Reps);

                stats.Add(new ChartData { Date = group.Key, MaxWeight = max, MinWeight = min, AverageWeight = avg, TotalReps = totalReps });
            }

            return stats;
        }

        private void HardcodeDataSimulation()
        {
            WorkoutDataFromDb.AddRange(new List<ExerciseRecordsDto>
            {
                new() { Date = new DateTime(2023, 01, 01), Weight = 26, Set = 1, Reps = 10 },
                new() { Date = new DateTime(2023, 01, 01), Weight = 28, Set = 2, Reps = 10 },
                new() { Date = new DateTime(2023, 01, 01), Weight = 30, Set = 3, Reps = 10 },
                new() { Date = new DateTime(2023, 02, 01), Weight = 28, Set = 1, Reps = 10 },
                new() { Date = new DateTime(2023, 02, 01), Weight = 28, Set = 2, Reps = 10 },
                new() { Date = new DateTime(2023, 02, 01), Weight = 30, Set = 3, Reps = 10 },
                new() { Date = new DateTime(2023, 03, 01), Weight = 30, Set = 1, Reps = 10 },
                new() { Date = new DateTime(2023, 03, 01), Weight = 30, Set = 1, Reps = 10 },
                new() { Date = new DateTime(2023, 03, 01), Weight = 32, Set = 1, Reps = 10 },
                new() { Date = new DateTime(2023, 04, 01), Weight = 32, Set = 1, Reps = 10 },
                new() { Date = new DateTime(2023, 04, 01), Weight = 32, Set = 1, Reps = 10 },
                new() { Date = new DateTime(2023, 04, 01), Weight = 32, Set = 1, Reps = 10 },
            });
        }

        private void GeneratedDataSimulation(int monthsOfTraining, int trainingAWeek)
        {
            Random rand = new Random();

            int startingWeight = 26;
            int weightIncrement = 1;
            int maxWeightIncrement = 3;
            DateTime startDate = new DateTime(2023, 01, 01);
            DateTime endDate = startDate.AddMonths(monthsOfTraining);

            for (DateTime date = startDate; date <= endDate; date = date.AddDays(7 / trainingAWeek))
            {
                int sessionsThisWeek = trainingAWeek;
                for (int session = 0; session < sessionsThisWeek; session++)
                {
                    int sets = rand.Next(1, 6); // Randomly choose 1-5 sets

                    for (int set = 1; set <= sets; set++)
                    {
                        int weight = startingWeight + weightIncrement * set;
                        weight += rand.Next(-1, 2); // Small variation in weight

                        int reps = rand.Next(8, 12); // Randomly choose 8-12 reps

                        WorkoutDataFromDb.Add(new ExerciseRecordsDto
                        {
                            Date = date.AddDays(session * (7 / trainingAWeek)), // Distribute sessions over the week
                            Weight = weight,
                            Set = set,
                            Reps = reps
                        });
                    }
                }

                // Incrementally increase the weight over time
                if (date.Day % 15 == 0)
                {
                    weightIncrement += rand.Next(1, maxWeightIncrement);
                }
            }
        }
    }
}
