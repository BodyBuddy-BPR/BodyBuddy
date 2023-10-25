using BodyBuddy.Models;
using SQLite;

namespace BodyBuddy.Repositories.Implementations
{
    public class IntakeRepository : IIntakeRepository
	{

		private readonly SQLiteAsyncConnection _context;

		public IntakeRepository(SQLiteAsyncConnection context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public async Task<IntakeModel> GetIntakeAsync()
		{
			try
			{
				//Get current date at midnight in UTC, and convert it to a timestamp
				DateTime currentDateTime = DateTime.UtcNow.Date;
				int currentDateTimestamp = (int)(currentDateTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

				//Check if there already exists an entry in the database with the timestamp for today, if not, we create one.
				var existingIntake = await _context.Table<IntakeModel>()
					.Where(x => x.Date == currentDateTimestamp)
					.FirstOrDefaultAsync();

				if (existingIntake == null)
				{
					// Creating a new Intake entry in the database, if one does not already exist.
					existingIntake = new IntakeModel();
					existingIntake.Id = await GetNextIntakeId();
					existingIntake.Date = currentDateTimestamp;

					var previousIntake = await _context.Table<IntakeModel>()
						.OrderByDescending(x => x.Date)
						.FirstOrDefaultAsync();

					// If a previous entry in the database exists for an intake, get the CalorieGoal and WaterGoal from this to
					// also be on the new one. If one does not exist, set a default water and calorie goal.
					if (previousIntake != null)
					{
						existingIntake.CalorieGoal = previousIntake.CalorieGoal;
						existingIntake.WaterGoal = previousIntake.WaterGoal;
						existingIntake.CalorieCurrent = 0;
						existingIntake.WaterCurrent = 0;
					}
					else
					{
						existingIntake.CalorieGoal = 3000;
						existingIntake.WaterGoal = 2500;
						existingIntake.CalorieCurrent = 0;
						existingIntake.WaterCurrent = 0;
					}

					// Insert the new entry in the database
					await _context.InsertAsync(existingIntake);
				}

				return existingIntake;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error in GetIntakeAsync: {ex}");
				return new IntakeModel();
			}
		}

		private async Task<int> GetNextIntakeId()
		{
			var lastItem = await _context.Table<IntakeModel>().OrderByDescending(x => x.Id).FirstOrDefaultAsync();
			return lastItem?.Id + 1 ?? 1;
		}

		public async Task SaveChangesAsync(IntakeModel intakeDetails)
		{
			await _context.UpdateAsync(intakeDetails);
		}
	}
}
