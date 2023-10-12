using BodyBuddy.Models;

namespace BodyBuddy.Repositories
{
    public interface IWorkoutRepository
    {
        Task<List<Workout>> GetWorkoutPlansAsync(int isPreMade);

		Task<int> PostWorkoutPlanAsync(Workout workout);

		Task DeleteWorkout(Workout workout);

		Task<bool> DoesWorkoutAlreadyExist(string name);
    }


}
