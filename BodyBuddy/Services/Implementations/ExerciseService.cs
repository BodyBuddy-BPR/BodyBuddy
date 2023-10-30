using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.Dtos;
using BodyBuddy.Mappers;
using BodyBuddy.Models;
using BodyBuddy.Repositories;

namespace BodyBuddy.Services.Implementations
{
    public class ExerciseService : IExerciseService
    {
        private readonly IExerciseRepository _exerciseRepository;
        private readonly ExerciseMapper _mapper = new();

        public ExerciseService(IExerciseRepository exerciseRepository)
        {
            _exerciseRepository = exerciseRepository;
        }
        public async Task<List<ExerciseDto>> GetExercisesAsync(string category, string muscleGroup)
        {
            var exercises = await _exerciseRepository.GetExercisesAsync(category, muscleGroup);
            return exercises.Select(exerciseModel => _mapper.MapToDto(exerciseModel)).ToList();
        }

        public async Task<ExerciseDto> GetExerciseDetails(int id)
        {
            return _mapper.MapToDto(await _exerciseRepository.GetExerciseDetails(id));
        }

        public async Task<List<string>> GetUniqueCategoriesAsync()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var categories = await _exerciseRepository.GetUniqueCategoriesAsync();

            stopwatch.Stop();
            Console.WriteLine($"GetUniqueCategoriesAsync took {stopwatch.ElapsedMilliseconds} ms with DB Call");
            Stopwatch stopwatch2 = new Stopwatch();
            stopwatch2.Start();
            var categories2 = new List<string>()
            {
                "Strength",
                "Cardio",
                "Stretching",
                "Plyometrics",
                "Strongman",
                "Powerlifting",
                "Olympic weightlifting",
            };
            stopwatch2.Stop();
            Console.WriteLine($"GetUniqueCategoriesAsync took {stopwatch2.ElapsedMilliseconds} ms with list generated ourselves {categories2}");


            return categories2;
        }

        public async Task<List<ExerciseModel>> GetMuscleGroupsForCategory(string category)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var exerciseDtoList = new List<ExerciseModel>();

            var primaryMuscleGroups = await _exerciseRepository.GetMuscleGroupsForCategory(category);
            
            foreach (var primaryMuscle in primaryMuscleGroups.Distinct())
            {
                exerciseDtoList.Add(new ExerciseModel()
                {
                    Category = category,
                    PrimaryMuscles = primaryMuscle,
                    TargetArea = _mapper.MapPrimaryMuscleToTargetArea(primaryMuscle),
                });
            }

            stopwatch.Stop();
            Console.WriteLine($"GetMuscleGroupsForCategory took {stopwatch.ElapsedMilliseconds} ms with DB Call");

            return exerciseDtoList;
        }
    }
}
