using BodyBuddy.Dtos;
using BodyBuddy.Mappers;
using BodyBuddy.Repositories;
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

        private readonly string _runStartupTest = "StartupTest";


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
            _startupTestRepository.SaveStartupTestData(mapper.MapToDatabaseFromDto(startupTestDto));

            if (Connectivity.NetworkAccess != NetworkAccess.Internet || !_userAuthenticationService.IsUserLoggedIn())
                return;

            _startupTestSbRepository.AddOrUpdateStartupTest(mapper.MapToSbModel(startupTestDto));
        }

        public async Task ReplaceSQLiteDataWithRemoteData()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet || !_userAuthenticationService.IsUserLoggedIn())
                return;

            await _startupTestRepository.ClearSQLiteData();

            var supabaseData = await _startupTestSbRepository.GetStartupTestSbModel();

            if (supabaseData != null)
            {
                await _startupTestRepository.SaveStartupTestData(mapper.MapToDatabaseFromSb(supabaseData));
                Preferences.Set(_runStartupTest, false);
            }
            else
            {
                Preferences.Set(_runStartupTest, true);
            }
        }

        public async Task BackUpExistingDataSupa()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet || !_userAuthenticationService.IsUserLoggedIn())
                return;

            var startupTestData = mapper.MapToDto(await _startupTestRepository.GetStartupTestData());
            await _startupTestSbRepository.AddOrUpdateStartupTest(mapper.MapToSbModel(startupTestData));
        }
    }
}
