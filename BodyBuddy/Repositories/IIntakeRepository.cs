using BodyBuddy.Models;

namespace BodyBuddy.Repositories
{
    public interface IIntakeRepository
	{
		Task<IntakeModel> GetIntakeAsync();

		Task SaveChangesAsync(IntakeModel intakeDetails);
	}
}
