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
                return new List<Workout>(); // Return an empty list
            }
        }

        public async Task<int> PostWorkoutPlanAsync(Workout workout)
        {
            if (workout.Id != 0)
                return await _context.UpdateAsync(workout);
            else
            {
                workout.Id = await GetNextIWorkoutd(); // Generate a unique Id
                return await _context.InsertAsync(workout);
            }
        }
        private async Task<int> GetNextIWorkoutd()
        {
            var lastItem = await _context.Table<Workout>().OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            return lastItem?.Id + 1 ?? 1;
        }

        public async Task DeleteWorkout(Workout workout)
        {
            // First deleting all exercises with the workout id from the joint table
            await _context.Table<WorkoutExercises>().DeleteAsync(x => x.WorkoutId == workout.Id);

            // Then deleting the workout from workout table
            await _context.DeleteAsync(workout);
        }

        public async Task<bool> DoesWorkoutAlreadyExist(string name)
        {
            if (await _context.Table<Workout>().FirstOrDefaultAsync(x => x.Name == name) == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }



    }
}
