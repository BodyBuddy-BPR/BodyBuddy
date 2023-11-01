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

		public async Task<IntakeModel> GetIntakeAsync()
		{
			try
			{
				//Get current date at midnight in UTC, and convert it to a timestamp
				DateTime currentDateTime = DateTime.UtcNow.Date;
				int currentDateTimestamp = (int)(currentDateTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

				//Check if entry for today exists.
				var existingIntake = await _context.Table<IntakeModel>()
					.Where(x => x.Date == currentDateTimestamp)
					.FirstOrDefaultAsync();

				if (existingIntake != null)
					return existingIntake;


				var previousIntake = await _context.Table<IntakeModel>()
					.OrderByDescending(x => x.Date)
					.FirstOrDefaultAsync();

				// If no previous entry exists, use default values
				existingIntake = new IntakeModel
				{
					Id = await GetNextIntakeId(),
					Date = currentDateTimestamp,
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

		public async Task<IntakeModel> GetIntakeForDateAsync(int dateTimeUTC)
		{
			try
			{
				var Intake = await _context.Table<IntakeModel>()
					.Where(x => x.Date == dateTimeUTC)
					.FirstOrDefaultAsync();

				return Intake;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error in GetIntakeForDateAsync: {ex}");
				return new IntakeModel();
			}
		}
	}
}
