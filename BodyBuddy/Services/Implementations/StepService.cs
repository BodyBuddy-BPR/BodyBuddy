using BodyBuddy.Dtos;
using BodyBuddy.Mappers;
using BodyBuddy.Repositories;
using BodyBuddy.Authentication;
using BodyBuddy.Helpers;
using BodyBuddy.SupaBase;

namespace BodyBuddy.Services.Implementations
{
    public class StepService : IStepService
    {
        private readonly IStepRepository _repo;
        private readonly IStepsSupaBase _stepsSupa;
        private readonly IUserAuthenticationService _userAuthenticationService;

        private readonly StepMapper _mapper = new();

        public StepService(IStepRepository stepRepository, IStepsSupaBase stepsSupaBase, IUserAuthenticationService userAuthenticationService)
        {
            _repo = stepRepository;
            _stepsSupa = stepsSupaBase;
            _userAuthenticationService = userAuthenticationService;
        }
        public async Task<StepDto> GetStepDataTodayAsync()
        {
            var stepData = await _repo.GetStepsForDayAsTimestampAsync(DateHelper.GetCurrentDayAtMidnight());
            return _mapper.MapToDto(stepData);
        }

        public async Task SaveStepData(StepDto stepDto)
        {
            await _repo.SaveChangesAsync(_mapper.MapToDatabase(stepDto));

            if (Connectivity.NetworkAccess == NetworkAccess.Internet && _userAuthenticationService.IsUserLoggedIn())
            {
                await _stepsSupa.GetStepsForPeriodFriends();
                _stepsSupa.AddOrUpdateSteps(stepDto);
            }
        }
    }
}
