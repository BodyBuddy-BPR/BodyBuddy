using BodyBuddy.Models;

namespace BodyBuddy.Repositories
{
    public interface IExerciseRepository
    {
        Task<List<string>> GetMuscleGroupsForCategory(string category);

        Task<List<ExerciseModel>> GetExercisesAsync(string category, string muscleGroup);

        Task<ExerciseModel> GetExerciseDetails(int id);

        Task<List<string>> GetUniqueCategoriesAsync();
    }
}
