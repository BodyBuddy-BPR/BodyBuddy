using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.Models.Supabase;
using Supabase;

namespace BodyBuddy.Repositories.Supabase.Implementation
{
    public class StartupTestSbRepository : IStartupTestSbRepository
    {
        private readonly Client _supabase;
        private string _userId = SecureStorage.GetAsync("UserUID").Result;

        public StartupTestSbRepository(Client client)
        {
            _supabase = client;
        }
        public async Task<StartupTestSbModel> GetStartupTestSbModel()
        {
            var stepModel = await _supabase.From<StartupTestSbModel>().Where(x => x.UserId == _userId).Get();

            return stepModel.Model;
        }

        public async Task AddOrUpdateStartupTest(StartupTestSbModel startupTestSbModel)
        {
            startupTestSbModel.UserId = _userId;
            await _supabase.From<StartupTestSbModel>().Upsert(startupTestSbModel);
        }
    }
}
