using BodyBuddy.Dtos;

namespace BodyBuddy.Services
{
    public interface IWorkoutService
    {
        Task<int> SaveWorkoutData(WorkoutDto workoutDto);

        Task<List<WorkoutDto>> GetWorkoutPlans(bool preMade);

        Task<WorkoutDto> GetSpecificWorkoutAsync(string name);

        Task<bool> DeleteWorkout(WorkoutDto workoutDto);

        Task<bool> DoesWorkoutAlreadyExist(string name);
    }
}
