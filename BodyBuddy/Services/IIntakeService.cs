using BodyBuddy.Dtos;

namespace BodyBuddy.Services
{
    public interface IIntakeService
    {
        Task<IntakeDto> GetIntakeAsync();
        Task<IntakeDto> GetIntakeForDateAsync(int dateTimeUTC);
        Task<List<IntakeDto>> GetAllIntakeDataAsync();

        Task SaveChangesAsync(IntakeDto intakeDetails);
    }
}
