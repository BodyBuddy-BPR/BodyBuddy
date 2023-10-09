using BodyBuddy.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.Repositories.Implementations
{
	public class IntakeRepository : IIntakeRepository
	{

		private readonly SQLiteAsyncConnection _context;

		public IntakeRepository(SQLiteAsyncConnection context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
			//_supabaseClient = supabaseClient;
		}

		public async Task<Intake> GetIntakeAsync()
		{
			try
			{
				DateTime currentDateTime = DateTime.UtcNow.Date;
				int currentDateTimestamp = (int)(currentDateTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

				var existingIntake = await _context.Table<Intake>()
					.Where(x => x.Date == currentDateTimestamp)
					.FirstOrDefaultAsync();

				if (existingIntake == null)
				{
					existingIntake = new Intake();
					existingIntake.Date = currentDateTimestamp;

					var previousIntake = await _context.Table<Intake>()
						.OrderByDescending(x => x.Date)
						.FirstOrDefaultAsync();

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

					await _context.InsertAsync(existingIntake);
				}

				return existingIntake;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error in GetIntakeAsync: {ex}");
				return new Intake();
			}
		}

		public async Task SaveChangesAsync(Intake intakeDetails)
		{
			await _context.UpdateAsync(intakeDetails);
		}
	}
}
