using BodyBuddy.Models;

namespace BodyBuddy.Repositories
{
    public interface IIntakeRepository
	{
		Task<IntakeModel> GetIntakeAsync();
		Task<IntakeModel> GetIntakeForDateAsync(int dateTimeUTC);

		Task SaveChangesAsync(IntakeModel intakeDetails);
	}
}
