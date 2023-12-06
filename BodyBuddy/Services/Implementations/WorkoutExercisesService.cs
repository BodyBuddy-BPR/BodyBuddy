using BodyBuddy.Dtos;
using BodyBuddy.Mappers;
using BodyBuddy.Repositories;

namespace BodyBuddy.Services.Implementations
{
    public class WorkoutExercisesService : IWorkoutExercisesService
    {
        private readonly IWorkoutExercisesRepository _repo;
        private readonly ExerciseMapper _mapper;

        public WorkoutExercisesService(IWorkoutExercisesRepository repo)
        {
            _mapper = new ExerciseMapper();
            _repo = repo;
        }
        public async Task AddExerciseToWorkout(int workoutId, int exerciseId)
        {
            await _repo.AddExerciseToWorkout(workoutId, exerciseId);
        }

        public async Task<List<ExerciseDto>> GetExercisesInWorkout(int workoutId)
        {
            var exerciseModels = await _repo.GetExercisesInWorkout(workoutId);

            return exerciseModels.Select(exerciseModel => _mapper.MapToDto(exerciseModel)).ToList();
        }

        public async Task EditExerciseInWorkout(ExerciseDto changedExercise)
        {
            await _repo.EditExerciseInWorkout(_mapper.MapToWorkoutExerciseModel(changedExercise));
        }

        public async Task DeleteExerciseFromWorkout(int workoutId, int exerciseId)
        {
            await _repo.DeleteExerciseFromWorkout(workoutId, exerciseId);
        }
    }
}
