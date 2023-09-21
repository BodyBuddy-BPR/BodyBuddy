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
    }
}
