using BodyBuddy.Dtos;

namespace BodyBuddy.Services
{
    public interface IStartupTestService
    {
        //Converts Dto --> Db object
        void SaveStartupTestData(StartupTestDto startupTestDto);

        //Converts Db --> Dto object
        Task<StartupTestDto> GetStartupTestData();

        Task RemoveAllSQLiteData();
        Task AddRemoteDataToSQLite();
    }
}