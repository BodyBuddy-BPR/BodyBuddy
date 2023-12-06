using BodyBuddy.Models;

namespace BodyBuddy.Repositories
{
    public interface IStartupTestRepository
    {
        Task SaveStartupTestData(StartupTestModel startupTest);
        Task<StartupTestModel> GetStartupTestData();
        Task ClearSQLiteData();
    }
}