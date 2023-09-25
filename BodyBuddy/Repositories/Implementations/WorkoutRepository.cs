using BodyBuddy.Models;
using SQLite;

namespace BodyBuddy.Repositories.Implementations
{
    public class WorkoutRepository : IWorkoutRepository
    {

        private readonly SQLiteAsyncConnection _context;

        public WorkoutRepository(SQLiteAsyncConnection context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<Workout>> GetWorkoutPlansAsync(int isPreMade)
        {
            try
            {
                var workouts = await _context.Table<Workout>().Where(x => x.PreMade.Equals(isPreMade)).ToListAsync();

                return workouts;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"Error in GetExercisesAsync: {ex}");
                return new List<Workout>(); // Return an empty list or handle the error gracefully.
            }
        }


        public async Task<List<Exercise>> GetExercisesInWorkout(int workoutId, bool isPreMade)
        {
            if (isPreMade)
            {
                List<Exercise> exercises = new();
                try
                {
                    var workoutIds = await _context.Table<PreMadeWorkoutExercises>().Where(x => x.WorkoutId == workoutId).ToListAsync();
                    foreach (var workout in workoutIds)
                    {
                        var exercise = await _context.Table<Exercise>().FirstOrDefaultAsync(x => x.Id == workout.ExerciseId);
                        exercises.Add(exercise);
                    }
                    return exercises;
                }
                catch (Exception ex)
                {
                    // Handle or log the exception
                    Console.WriteLine($"Error in GetExercisesAsync: {ex}");
                    return new List<Exercise>(); // Return an empty list or handle the error gracefully.
                }
            }
            else
            {
                List<Exercise> exercises = new();
                try
                {
                    var workoutIds = await _context.Table<WorkoutExercises>().Where(x => x.WorkoutId == workoutId).ToListAsync();
                    foreach (var workout in workoutIds)
                    {
                        var exercise = await _context.Table<Exercise>().FirstOrDefaultAsync(x => x.Id == workout.ExerciseId);
                        exercises.Add(exercise);
                    }
                    return exercises;
                }
                catch (Exception ex)
                {
                    // Handle or log the exception
                    Console.WriteLine($"Error in GetExercisesAsync: {ex}");
                    return new List<Exercise>(); // Return an empty list or handle the error gracefully.
                }
            }
        }

		public async Task<int> PostWorkoutPlanAsync(Workout workout)
		{
			if (workout.Id != 0)
				return await _context.UpdateAsync(workout);
			else
			{
				workout.Id = await GetNextItemId(); // Generate a unique Id
				return await _context.InsertAsync(workout);
			}
		}

		private async Task<int> GetNextItemId()
		{
			var lastItem = await _context.Table<Workout>().OrderByDescending(x => x.Id).FirstOrDefaultAsync();
			return lastItem?.Id + 1 ?? 1;
		}

        public async Task DeleteWorkout(Workout workout)
        {
            await _context.DeleteAsync(workout);
        }
	}
}
