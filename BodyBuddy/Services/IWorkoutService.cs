using BodyBuddy.Dtos;

namespace BodyBuddy.Services
{
    public interface IWorkoutService
    {
        /// <summary>
        /// Saving Workout data in both local and remote db
        /// </summary>
        /// <param name="workoutDto"></param>
        /// <returns></returns>
        Task SaveWorkoutData(WorkoutDto workoutDto);

        /// <summary>
        /// Get all Workout plans of type premade or selfmade
        /// </summary>
        /// <param name="preMade"></param>
        /// <returns></returns>
        Task<List<WorkoutDto>> GetWorkoutPlans(bool preMade);

        /// <summary>
        /// Get Workout from local db using name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<WorkoutDto> GetSpecificWorkoutAsync(string name);

        /// <summary>
        /// Delete Workout in both local and remote db
        /// </summary>
        /// <param name="workoutDto"></param>
        /// <returns></returns>
        Task<bool> DeleteWorkout(WorkoutDto workoutDto);

        /// <summary>
        /// Checking if Workout Exists in Db via name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>If exist</returns>
        Task<bool> DoesWorkoutAlreadyExist(string name);

        /// <summary>
        /// Remove local data and replacing with remote db data if internet and logged in
        /// </summary>
        /// <returns></returns>
        Task ReplaceSQLiteDataWithRemoteData();
    }
}
