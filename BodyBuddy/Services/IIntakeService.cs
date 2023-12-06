using BodyBuddy.Dtos;

namespace BodyBuddy.Services
{
    public interface IIntakeService
    {
        Task<IntakeDto> GetIntakeTodayAsync();
        Task<IntakeDto> GetIntakeForDateAsync(long dateTimeUTC);
        Task<List<IntakeDto>> GetAllIntakeDataAsync();

        Task SaveChangesAsync(IntakeDto intakeDetails);
    }
}
