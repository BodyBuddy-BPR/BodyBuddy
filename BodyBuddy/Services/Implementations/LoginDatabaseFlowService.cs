using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.Repositories;

namespace BodyBuddy.Services.Implementations
{
    public class LoginDatabaseFlowService : ILoginDatabaseFlowService
    {
        private readonly IStartupTestService _startupTestService;

        public LoginDatabaseFlowService(IStartupTestService startupTestService)
        {
            _startupTestService = startupTestService;
        }

        public async Task StartLoginDatabaseFlow()
        {
            await ReplaceStartupTestData();
        }

        private async Task ReplaceStartupTestData()
        {
            await _startupTestService.RemoveAllSQLiteData();
            await _startupTestService.AddRemoteDataToSQLite();
        }
    }
}
