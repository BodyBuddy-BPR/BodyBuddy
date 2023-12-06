namespace BodyBuddy.Services.Implementations
{
    public class LoginDatabaseFlowService : ILoginDatabaseFlowService
    {
        private readonly IStartupTestService _startupTestService;
        private readonly IStepService _stepService;

        public LoginDatabaseFlowService(IStartupTestService startupTestService, IStepService stepService)
        {
            _startupTestService = startupTestService;
            _stepService = stepService;
        }

        public async Task StartLoginDatabaseFlow()
        {
            await ReplaceStartupTestData();
        }

        private async Task ReplaceStartupTestData()
        {
            await _startupTestService.ReplaceSQLiteDataWithRemoteData();
            await _stepService.ReplaceSQLiteDataWithRemoteData();
        }
    }
}
