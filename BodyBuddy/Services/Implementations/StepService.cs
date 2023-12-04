using BodyBuddy.Dtos;
using BodyBuddy.Mappers;
using BodyBuddy.Repositories;
using BodyBuddy.Authentication;
using BodyBuddy.Helpers;
using BodyBuddy.Repositories.Supabase.Implementation;

namespace BodyBuddy.Services.Implementations
{
    public class StepService : IStepService
    {
        private readonly IStepRepository _repo;
        private readonly IStepsSupabase _stepsSupa;
        private readonly IUserAuthenticationService _userAuthenticationService;

        private readonly StepMapper _mapper = new();

        public StepService(IStepRepository stepRepository, IStepsSupabase stepsSupabase, IUserAuthenticationService userAuthenticationService)
        {
            _repo = stepRepository;
            _stepsSupa = stepsSupabase;
            _userAuthenticationService = userAuthenticationService;
        }
        public async Task<StepDto> GetStepDataTodayAsync()
        {
            var stepData = await _repo.GetStepsForDayAsTimestampAsync(DateHelper.GetCurrentDayAtMidnight());
            return _mapper.MapToDto(stepData);
        }

        public async Task<List<UserTotalSteps>> GetStepsForPeriodFriends()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet && _userAuthenticationService.IsUserLoggedIn())
            {
                var stepsList = await _stepsSupa.GetStepsForPeriodFriends();
                return stepsList.GroupBy(step => step.User)
                    .Select(group => new UserTotalSteps
                    {
                        User = group.Key,
                        TotalSteps = group.Sum(item => item.Steps)
                    })
                    .ToList();
            }
            return new List<UserTotalSteps>();
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
