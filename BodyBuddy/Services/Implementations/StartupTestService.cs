using BodyBuddy.Dtos;
using BodyBuddy.Mappers;
using BodyBuddy.Repositories;
using BodyBuddy.Repositories.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.Authentication;
using BodyBuddy.Repositories.Supabase;

namespace BodyBuddy.Services.Implementations
{
    public class StartupTestService : IStartupTestService
    {
        private readonly IStartupTestRepository _startupTestRepository;
        private readonly IStartupTestSbRepository _startupTestSbRepository;
        private readonly IUserAuthenticationService _userAuthenticationService;

        private readonly StartupTestMapper mapper = new();


        public StartupTestService(IStartupTestRepository startupTestRepository, IStartupTestSbRepository startupTestSbRepository, IUserAuthenticationService userAuthenticationService)
        {
            _startupTestRepository = startupTestRepository;
            _startupTestSbRepository = startupTestSbRepository;
            _userAuthenticationService = userAuthenticationService;
        }

        public async Task<StartupTestDto> GetStartupTestData()
        {
            var startupTestData = await _startupTestRepository.GetStartupTestData();
            return mapper.MapToDto(startupTestData);
        }

        public void SaveStartupTestData(StartupTestDto startupTestDto)
        {
            _startupTestRepository.SaveStartupTestData(mapper.MapToDatabase(startupTestDto));

            if (Connectivity.NetworkAccess != NetworkAccess.Internet || !_userAuthenticationService.IsUserLoggedIn())
                return;

            _startupTestSbRepository.AddOrUpdateStartupTest(mapper.MapToSbModel(startupTestDto));
        }

        #region Clear and add data to SQLite from remote
        public async Task RemoveAllSQLiteData()
        {
            throw new NotImplementedException();
        }

        public async Task AddRemoteDataToSQLite()
        {
            throw new NotImplementedException();
        }
        #endregion



    }
}
