using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.Dtos;
using BodyBuddy.Models;

namespace BodyBuddy.Services
{
    public interface IExerciseService
    {
        //FIXME: Not used, but in IExerciseRepository?
        //Task<List<string>> GetMuscleGroupsForCategory(string category);

        Task<List<ExerciseDto>> GetExercisesAsync(string category, string muscleGroup);

        Task<ExerciseDto> GetExerciseDetails(int id);

        Task<List<string>> GetUniqueCategoriesAsync();

        //Task<List<(string PrimaryMuscle, string TargetArea)>> GetMuscleGroupsForCategory(string category);
        Task<List<ExerciseModel>> GetMuscleGroupsForCategory(string category);
    }
}
