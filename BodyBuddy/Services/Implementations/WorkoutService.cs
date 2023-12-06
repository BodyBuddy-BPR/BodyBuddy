using BodyBuddy.Dtos;
using BodyBuddy.Mappers;
using BodyBuddy.Repositories;

namespace BodyBuddy.Services.Implementations
{
    public class WorkoutService : IWorkoutService
    {
        private readonly IWorkoutRepository _repo;
        private readonly WorkoutMapper _mapper = new();

        public WorkoutService(IWorkoutRepository repository)
        {
            _repo = repository;
        }

        public async Task<int> SaveWorkoutData(WorkoutDto startupTestDto)
        {
            if (startupTestDto.Id == 0)
                return await _repo.AddWorkoutPlanAsync(_mapper.MapToDatabase(startupTestDto));

            return await _repo.UpdateWorkoutPlanAsync(_mapper.MapToDatabase(startupTestDto));
        }

        public async Task<List<WorkoutDto>> GetWorkoutPlans(bool preMade)
        {
            var workouts = await _repo.GetWorkoutPlansAsync(preMade ? 1 : 0);

            return workouts.Select(workoutModel => _mapper.MapToDto(workoutModel)).ToList();
        }

        public async Task<WorkoutDto> GetSpecificWorkoutAsync(string name)
        {
            return _mapper.MapToDto(await _repo.GetSpecificWorkoutAsync(name));
        }

        public async Task<bool> DeleteWorkout(WorkoutDto workoutDto)
        {
            return await _repo.DeleteWorkout(_mapper.MapToDatabase(workoutDto));
        }

        public async Task<bool> DoesWorkoutAlreadyExist(string name)
        {
            return await _repo.DoesWorkoutAlreadyExist(name);
        }
    }
}
