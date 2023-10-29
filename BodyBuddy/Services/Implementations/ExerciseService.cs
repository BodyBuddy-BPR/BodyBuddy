using System;
using System.Collections.Generic;
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
            return await _exerciseRepository.GetUniqueCategoriesAsync();
        }

        //public async Task<List<(string PrimaryMuscle, string TargetArea)>> GetMuscleGroupsForCategory(string category)
        public async Task<List<ExerciseModel>> GetMuscleGroupsForCategory(string category)
        {
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

            return exerciseDtoList;
        }
    }
}
