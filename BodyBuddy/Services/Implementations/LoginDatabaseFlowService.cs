namespace BodyBuddy.Services.Implementations
{
    public class LoginDatabaseFlowService : ILoginDatabaseFlowService
    {
        private readonly IStartupTestService _startupTestService;
        private readonly IStepService _stepService;
        private readonly IIntakeService _intakeService;

        public LoginDatabaseFlowService(IStartupTestService startupTestService, IStepService stepService, IIntakeService intakeService)
        {
            _startupTestService = startupTestService;
            _stepService = stepService;
            _intakeService = intakeService;
        }

        public async Task StartLoginDatabaseFlow()
        {
            await ReplaceStartupTestData();
        }

        private async Task ReplaceStartupTestData()
        {
            await _startupTestService.ReplaceSQLiteDataWithRemoteData();
            await _stepService.ReplaceSQLiteDataWithRemoteData();
            await _intakeService.ReplaceSQLiteDataWithRemoteData();
        }
    }
}
