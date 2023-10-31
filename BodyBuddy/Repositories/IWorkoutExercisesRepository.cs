using BodyBuddy.Models;

namespace BodyBuddy.Repositories
{
    public interface IWorkoutExercisesRepository
    {
        Task<List<ExerciseModel>> GetExercisesInWorkout(int workoutId);

        Task AddExerciseToWorkout(int workoutId, int exerciseId);

        Task EditExerciseInWorkout(WorkoutExercisesModel changedWorkoutExercisesModel);

        Task DeleteExerciseFromWorkout(int workoutId, int exerciseId);

    }
}
