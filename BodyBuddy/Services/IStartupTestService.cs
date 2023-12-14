using BodyBuddy.Dtos;

namespace BodyBuddy.Services
{
    public interface IStartupTestService
    {
        /// <summary>
        /// Saves Startup Test Data into local and online if internet and logged on
        /// </summary>
        /// <param name="startupTestDto"></param>
        void SaveStartupTestData(StartupTestDto startupTestDto);

        /// <summary>
        /// Getting StartupTestData from local database
        /// </summary>
        /// <returns></returns>
        Task<StartupTestDto> GetStartupTestData();

        /// <summary>
        /// Remove local data and replacing with remote db data if internet and logged in
        /// </summary>
        /// <returns></returns>
        Task ReplaceSQLiteDataWithRemoteData();

        /// <summary>
        /// Saving existing local data into remote db
        /// </summary>
        /// <returns></returns>
        Task BackUpExistingDataToSupabase();
    }
}