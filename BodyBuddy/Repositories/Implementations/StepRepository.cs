using BodyBuddy.Models;
using SQLite;

namespace BodyBuddy.Repositories.Implementations
{
    public class StepRepository : IStepRepository
    {
        private readonly SQLiteAsyncConnection _context;

        public StepRepository(SQLiteAsyncConnection context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<StepModel> GetStepsForDayAsTimestampAsync(long dayAsTimestamp)
        {
            try
            {
                //Check if entry for today exists.
                var existingStepCount = await _context.Table<StepModel>()
                    .Where(x => x.Date == dayAsTimestamp)
                    .FirstOrDefaultAsync();

                if (existingStepCount != null)
                    return existingStepCount;


                var previousStepCount = await _context.Table<StepModel>()
                    .OrderByDescending(x => x.Date)
                    .FirstOrDefaultAsync();

                // If no previous entry exists, use default values
                existingStepCount = new StepModel
                {
                    Id = await GetNextId(),
                    Date = dayAsTimestamp,
                    Steps = 0,
                    StepGoal = previousStepCount?.StepGoal ?? 8000
                };

                // Insert the new entry in the database
                await _context.InsertAsync(existingStepCount);

                return existingStepCount;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetStepsForDayAsTimestampAsync: {ex}");
                return new StepModel();
            }
        }

        private async Task<int> GetNextId()
        {
            var lastItem = await _context.Table<StepModel>().OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            return lastItem?.Id + 1 ?? 1;
        }

        public async Task SaveChangesAsync(StepModel stepDetails)
        {
            await _context.UpdateAsync(stepDetails);
        }

        public async Task ClearSQLiteData()
        {
            await _context.DeleteAllAsync<StepModel>();
        }

        public async Task AddListOfStepData(List<StepModel> stepModels)
        {
            foreach (var stepModel in stepModels)
            {
                stepModel.Id = await GetNextId();

                // Insert the new entry in the database
                await _context.InsertAsync(stepModel);
            }
        }
    }
}
