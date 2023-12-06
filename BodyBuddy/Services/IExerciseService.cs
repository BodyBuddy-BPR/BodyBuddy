using BodyBuddy.Dtos;

namespace BodyBuddy.Services
{
    public interface IExerciseService
    {
        Task<List<ExerciseDto>> GetExercisesAsync(string category, string muscleGroup);

        Task<ExerciseDto> GetExerciseDetails(int id);

        /// <summary>
        /// Retrieves a list with all unique categories in database
        /// </summary>
        /// <returns>List of Categories</returns>
        Task<List<string>> GetUniqueCategoriesAsync();

        /// <summary>
        /// Retrieves a list of ExerciseDto with PrimaryMuscle, TargetArea and category populated
        /// </summary>
        /// <param name="category"></param>
        /// <returns>List of ExerciseDtos</returns>
        Task<List<ExerciseDto>> GetMuscleGroupsForCategory(string category);
    }
}
