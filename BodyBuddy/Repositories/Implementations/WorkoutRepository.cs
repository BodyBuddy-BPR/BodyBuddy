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

        public async Task<List<WorkoutModel>> GetWorkoutPlansAsync(int isPreMade)
        {
            try
            {
                var workouts = await _context.Table<WorkoutModel>().Where(x => x.PreMade.Equals(isPreMade)).ToListAsync();

                return workouts;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"Error in GetExercisesAsync: {ex}");
                return new List<WorkoutModel>(); // Return an empty list
            }
        }

        public async Task<int> PostWorkoutPlanAsync(WorkoutModel workout)
        {
            if (workout.Id != 0)
                return await _context.UpdateAsync(workout);

            workout.Id = await GetNextWorkoutId(); // Generate a unique Id
            return await _context.InsertAsync(workout);
        }

        public async Task<bool> DeleteWorkout(WorkoutModel workout)
        {
            // First deleting all exercises with the workout id from the joint table
            await _context.Table<WorkoutExercisesModel>().DeleteAsync(x => x.WorkoutId == workout.Id);

            // Then deleting the workout from workout table
            await _context.DeleteAsync(workout);
            return true;
        }

        public async Task<bool> DoesWorkoutAlreadyExist(string name)
        {
            return await _context.Table<WorkoutModel>().FirstOrDefaultAsync(x => x.Name == name) != null;
        }

        public async Task<WorkoutModel> GetSpecificWorkoutAsync(string name)
        {
            return await _context.Table<WorkoutModel>().FirstOrDefaultAsync(x => x.Name == name);
        }


        #region new sep

        public async Task<int> AddWorkoutPlanAsync(WorkoutModel workout)
        {
            workout.Id = await GetNextWorkoutId(); // Generate a unique Id
            await _context.InsertAsync(workout);
            return workout.Id;
        }

        public async Task<int> UpdateWorkoutPlanAsync(WorkoutModel workout)
        {
            return await _context.UpdateAsync(workout);
        }

        #endregion


        private async Task<int> GetNextWorkoutId()
        {
            var lastItem = await _context.Table<WorkoutModel>().OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            return lastItem?.Id + 1 ?? 1;
        }
    }
}
