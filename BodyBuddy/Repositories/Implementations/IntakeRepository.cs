using BodyBuddy.Helpers;
using BodyBuddy.Models;
using SQLite;

namespace BodyBuddy.Repositories.Implementations
{
    public class IntakeRepository : IIntakeRepository
    {

        private readonly SQLiteAsyncConnection _context;
        private const int DefaultCalorieGoal = 3000;
        private const int DefaultWaterGoal = 2500;

        public IntakeRepository(SQLiteAsyncConnection context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IntakeModel> GetIntakeForDateAsync(long dateTimeUtc)
        {
            try
            {
                //Check if entry for today exists.
                var existingIntake = await _context.Table<IntakeModel>()
                    .Where(x => x.Date == dateTimeUtc)
                    .FirstOrDefaultAsync();

                if (existingIntake != null)
                    return existingIntake;

                var previousIntake = await _context.Table<IntakeModel>()
                    .OrderByDescending(x => x.Date)
                    .FirstOrDefaultAsync();

                // If no previous entry exists, use default values
                existingIntake = new IntakeModel
                {
                    Id = await GetNextId(),
                    Date = dateTimeUtc,
                    CalorieGoal = previousIntake?.CalorieGoal ?? DefaultCalorieGoal,
                    WaterGoal = previousIntake?.WaterGoal ?? DefaultWaterGoal,
                    CalorieCurrent = 0,
                    WaterCurrent = 0
                };

                // Insert the new entry in the database
                await _context.InsertAsync(existingIntake);

                return existingIntake;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetCurrentDayIntakeAsync: {ex}");
                return new IntakeModel();
            }
        }

        public async Task<List<IntakeModel>> GetAllIntakeDataAsync()
        {
            try
            {
                // Fetch all IntakeModel entries from the database
                var allIntakes = await _context.Table<IntakeModel>()
                    .ToListAsync();

                return allIntakes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllIntakeDataAsync: {ex}");
                return new List<IntakeModel>(); // Return an empty list in case of an exception
            }
        }

        private async Task<int> GetNextId()
        {
            var lastItem = await _context.Table<IntakeModel>().OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            return lastItem?.Id + 1 ?? 1;
        }

        public async Task SaveChangesAsync(IntakeModel intakeDetails)
        {
            await _context.UpdateAsync(intakeDetails);
        }

        public async Task ClearSQLiteData()
        {
            await _context.DeleteAllAsync<IntakeModel>();
        }

        public async Task AddListOfIntakeData(List<IntakeModel> intakeModels)
        {
            foreach (var intakeModel in intakeModels)
            {
                intakeModel.Id = await GetNextId();

                // Insert the new entry in the database
                await _context.InsertAsync(intakeModel);
            }
        }
    }
}
