using BodyBuddy.Models;

namespace BodyBuddy.Repositories
{
    public interface IWorkoutRepository
    {
        Task<List<Workout>> GetWorkoutPlansAsync(int isPreMade);

        Task<Workout> GetSpecificWorkoutAsync(string name);

		Task<int> PostWorkoutPlanAsync(Workout workout);

		Task<bool> DeleteWorkout(Workout workout);

		Task<bool> DoesWorkoutAlreadyExist(string name);
    }


}
