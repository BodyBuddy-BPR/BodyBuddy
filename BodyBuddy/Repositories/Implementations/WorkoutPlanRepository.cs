using BodyBuddy.Models;
using SQLite;

namespace BodyBuddy.Repositories.Implementations
{
    public class WorkoutPlanRepository : IWorkoutPlanRepository
    {

		private readonly SQLiteAsyncConnection _context;

		public WorkoutPlanRepository(SQLiteAsyncConnection context)
        {
			_context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<Workout>> GetWorkoutPlansAsync()
        {
			try
			{
				var workouts = await _context.Table<Workout>().ToListAsync();

				return workouts;
			}
			catch (Exception ex)
			{
				// Handle or log the exception
				Console.WriteLine($"Error in GetExercisesAsync: {ex}");
				return new List<Workout>(); // Return an empty list or handle the error gracefully.
			}
		}

		public async Task SaveWorkoutPlanAsync(Workout workoutPlan)
		{
			try
			{
				await _context.InsertAsync(workoutPlan);
			}
			catch (Exception ex)
			{
				// Handle or log the exception
				Console.WriteLine($"Error in SaveWorkoutPlanAsync: {ex}");
				throw; // Rethrow the exception or handle it gracefully as needed.
			}
		}
	}
}
