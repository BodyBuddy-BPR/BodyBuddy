using BodyBuddy.Authentication;
using BodyBuddy.Dtos;
using BodyBuddy.Mappers;
using BodyBuddy.Models.Supabase;
using BodyBuddy.Repositories;
using BodyBuddy.Repositories.Supabase;

namespace BodyBuddy.Services.Implementations
{
    public class WorkoutExercisesService : IWorkoutExercisesService
    {
        private readonly IWorkoutExercisesRepository _workoutExercisesRepository;
        private readonly IWorkoutSbRepository _workoutSbRepository;
        private readonly IUserAuthenticationService _userAuthenticationService;
        private readonly ExerciseMapper _mapper = new();

        public WorkoutExercisesService(IWorkoutExercisesRepository workoutExercisesRepository, IWorkoutSbRepository workoutSbRepository, IUserAuthenticationService userAuthenticationService)
        {
            _workoutExercisesRepository = workoutExercisesRepository;
            _workoutSbRepository = workoutSbRepository;
            _userAuthenticationService = userAuthenticationService;
        }
        public async Task AddExerciseToWorkout(int workoutId, int exerciseId)
        {
            await _workoutExercisesRepository.AddExerciseToWorkout(workoutId, exerciseId);

            if (Connectivity.NetworkAccess != NetworkAccess.Internet || !_userAuthenticationService.IsUserLoggedIn())
                return;

            await _workoutSbRepository.AddOrUpdateWorkoutExercise(new WorkoutExerciseSbModel()
            {
                WorkoutId = workoutId,
                ExerciseId = exerciseId,
                Sets = 3,
                Reps = 12
            });
        }

        public async Task<List<ExerciseDto>> GetExercisesInWorkout(int workoutId)
        {
            var exerciseModels = await _workoutExercisesRepository.GetExercisesInWorkout(workoutId);

            return exerciseModels.Select(exerciseModel => _mapper.MapToDto(exerciseModel)).ToList();
        }

        public async Task EditExerciseInWorkout(ExerciseDto changedExercise)
        {
            await _workoutExercisesRepository.EditExerciseInWorkout(_mapper.MapToWorkoutExerciseModel(changedExercise));

            if (Connectivity.NetworkAccess != NetworkAccess.Internet || !_userAuthenticationService.IsUserLoggedIn())
                return;

            await _workoutSbRepository.AddOrUpdateWorkoutExercise(new WorkoutExerciseSbModel()
            {
                WorkoutId = changedExercise.WorkoutId,
                ExerciseId = changedExercise.Id,
                Reps = changedExercise.Reps,
                Sets = changedExercise.Sets
            });
        }

        public async Task DeleteExerciseFromWorkout(int workoutId, int exerciseId)
        {
            await _workoutExercisesRepository.DeleteExerciseFromWorkout(workoutId, exerciseId);

            if (Connectivity.NetworkAccess != NetworkAccess.Internet || !_userAuthenticationService.IsUserLoggedIn())
                return;
            await _workoutSbRepository.RemoveWorkoutExercise(workoutId, exerciseId);
        }

        public async Task ReplaceSQLiteDataWithRemoteData()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet || !_userAuthenticationService.IsUserLoggedIn())
                return;

            var supabaseData = await _workoutSbRepository.GetAllWorkoutExercisesForProfile();

            var workoutExerciseModels = supabaseData
                .Select(x => _mapper.MapToWorkoutExerciseDatabaseFromSbWorkoutExercisesModel(x)).ToList();

            if (workoutExerciseModels.Any())
                await _workoutExercisesRepository.AddExercisesToWorkouts(workoutExerciseModels);
        }
    }
}
