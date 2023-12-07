using BodyBuddy.Models;

namespace BodyBuddy.Repositories
{
    public interface IIntakeRepository
	{
        Task<IntakeModel> GetIntakeForDateAsync(long dateTimeUtc);
        Task<List<IntakeModel>> GetAllIntakeDataAsync();
		Task SaveChangesAsync(IntakeModel intakeDetails);


        //Supabase Clean
        Task ClearSQLiteData();
        Task AddListOfIntakeData(List<IntakeModel> intakeModels);
    }
}
