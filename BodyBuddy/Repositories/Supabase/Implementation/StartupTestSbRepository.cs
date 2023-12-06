using BodyBuddy.Models.Supabase;
using Supabase;

namespace BodyBuddy.Repositories.Supabase.Implementation
{
    public class StartupTestSbRepository : IStartupTestSbRepository
    {
        private readonly Client _supabase;

        public StartupTestSbRepository(Client client)
        {
            _supabase = client;
        }
        public async Task<StartupTestSbModel> GetStartupTestSbModel()
        {
            var userId = SecureStorage.GetAsync("UserUID").Result;
            try
            {
                var stepModel = await _supabase.From<StartupTestSbModel>()
                    .Where(x => x.UserId == userId).Get();

                return stepModel.Model;
            }
            catch (Exception ex)
            {
                //If no data is found, it goes down here!
                return null;
            }
        }

        public async Task AddOrUpdateStartupTest(StartupTestSbModel startupTestSbModel)
        {
            startupTestSbModel.UserId = SecureStorage.GetAsync("UserUID").Result;
            await _supabase.From<StartupTestSbModel>().Upsert(startupTestSbModel);
        }
    }
}
