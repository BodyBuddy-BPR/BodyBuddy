using BodyBuddy.Models;

namespace BodyBuddy.Repositories
{
    public interface IStartupTestRepository
    {
        Task SaveStartupTestData(StartupTest startupTest);
        Task<StartupTest> GetStartupTestData();
    }
}