using BodyBuddy.Models;

namespace BodyBuddy.Repositories
{
    public interface IIntakeRepository
	{
		Task<Intake> GetIntakeAsync();

		Task SaveChangesAsync(Intake intakeDetails);
	}
}
