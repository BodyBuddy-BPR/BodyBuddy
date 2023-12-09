using BodyBuddy.Dtos;

namespace BodyBuddy.Services
{
    public interface IIntakeService
    {
        /// <summary>
        /// Get IntakeData for Current Day
        /// </summary>
        /// <returns></returns>
        Task<IntakeDto> GetIntakeTodayAsync();

        /// <summary>
        /// Get IntakeData for Selected Day 
        /// </summary>
        /// <param name="dateTimeUTC">time in epoch at midnight</param>
        /// <returns></returns>
        Task<IntakeDto> GetIntakeForDateAsync(long dateTimeUTC);

        /// <summary>
        /// Get all local IntakeData
        /// </summary>
        /// <returns></returns>
        Task<List<IntakeDto>> GetAllIntakeDataAsync();

        /// <summary>
        /// Saves changes in local and remote db
        /// </summary>
        /// <param name="intakeDetails"></param>
        /// <returns></returns>
        Task SaveChangesAsync(IntakeDto intakeDetails);


        /// <summary>
        /// Remove local data and replacing with remote db data if internet and logged in
        /// </summary>
        /// <returns></returns>
        Task ReplaceSQLiteDataWithRemoteData();
    }
}
