using BodyBuddy.Models.Supabase;

namespace BodyBuddy.Repositories.Supabase
{
    public interface IStartupTestSbRepository
    {
        Task<StartupTestSbModel> GetStartupTestSbModel();
        Task AddOrUpdateStartupTest(StartupTestSbModel startupTestSbModel);
    }
}
