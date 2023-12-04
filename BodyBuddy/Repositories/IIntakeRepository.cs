using BodyBuddy.Models;

namespace BodyBuddy.Repositories
{
    public interface IIntakeRepository
	{
		Task<IntakeModel> GetCurrentDayIntakeAsync();
        Task<List<IntakeModel>> GetAllIntakeDataAsync();
        Task<IntakeModel> GetIntakeForDateAsync(int dateTimeUTC);
		Task SaveChangesAsync(IntakeModel intakeDetails);
	}
}
