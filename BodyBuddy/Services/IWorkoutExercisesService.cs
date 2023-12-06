using BodyBuddy.Dtos;

namespace BodyBuddy.Services
{
    public interface IWorkoutExercisesService
    {
        Task AddExerciseToWorkout(int workoutId, int exerciseId);

        Task<List<ExerciseDto>> GetExercisesInWorkout(int workoutId);

        Task EditExerciseInWorkout(ExerciseDto changedExercise);

        Task DeleteExerciseFromWorkout(int workoutId, int exerciseId);
    }
}
