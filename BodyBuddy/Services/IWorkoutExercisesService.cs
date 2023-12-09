using BodyBuddy.Dtos;

namespace BodyBuddy.Services
{
    public interface IWorkoutExercisesService
    {
        /// <summary>
        /// Adds Exercise to workout in local and remote db
        /// </summary>
        /// <param name="workoutId"></param>
        /// <param name="exerciseId"></param>
        /// <returns></returns>
        Task AddExerciseToWorkout(int workoutId, int exerciseId);

        /// <summary>
        /// Gets data from local db
        /// </summary>
        /// <param name="workoutId"></param>
        /// <returns>List of Exercises in workout</returns>
        Task<List<ExerciseDto>> GetExercisesInWorkout(int workoutId);

        /// <summary>
        /// Edits exercise data in local and remote db
        /// </summary>
        /// <param name="changedExercise"></param>
        /// <returns></returns>
        Task EditExerciseInWorkout(ExerciseDto changedExercise);

        /// <summary>
        /// Deletion of Exercise from the selected workout in local and remote db
        /// </summary>
        /// <param name="workoutId"></param>
        /// <param name="exerciseId"></param>
        /// <returns></returns>
        Task DeleteExerciseFromWorkout(int workoutId, int exerciseId);

        /// <summary>
        /// Remove local data and replacing with remote db data if internet and logged in
        /// </summary>
        /// <returns></returns>
        Task ReplaceSQLiteDataWithRemoteData();
    }
}
