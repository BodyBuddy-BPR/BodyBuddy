using BodyBuddy.Models;

namespace BodyBuddy.Repositories
{
    public interface IWorkoutExercisesRepository
    {
        Task<List<Exercise>> GetExercisesInWorkout(int workoutId);

        Task AddExerciseToWorkout(int workoutId, int exerciseId);

        Task EditExerciseInWorkout(int workoutId, Exercise changedExercise);

        Task DeleteExerciseFromWorkout(int workoutId, int exerciseId);

    }
}
