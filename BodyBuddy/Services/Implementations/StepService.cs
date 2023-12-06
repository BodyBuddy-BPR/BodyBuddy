using BodyBuddy.Dtos;
using BodyBuddy.Mappers;
using BodyBuddy.Repositories;
using BodyBuddy.Authentication;
using BodyBuddy.Helpers;
using BodyBuddy.Models;
using BodyBuddy.Repositories.Supabase;

namespace BodyBuddy.Services.Implementations
{
    public class StepService : IStepService
    {
        private readonly IStepRepository _repo;
        private readonly IStepsSbRepository _stepsSupa;
        private readonly IUserAuthenticationService _userAuthenticationService;

        private readonly StepMapper _mapper = new();
        private List<UserTotalSteps> _totalSteps = new();

        //Delegate to notify ChallengeService
        public delegate void StepsChanged(int steps);
        public event IStepService.StepsChanged IsStepsChanged;

        public StepService(IStepRepository stepRepository, IStepsSbRepository stepsSupabase, IUserAuthenticationService userAuthenticationService)
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
            if (Connectivity.NetworkAccess != NetworkAccess.Internet || !_userAuthenticationService.IsUserLoggedIn())
                return new List<UserTotalSteps>();

            var stepsList = await _stepsSupa.GetStepsForPeriodFriends();

            _totalSteps = stepsList.GroupBy(step => step.User)
                .Select(group => new UserTotalSteps
                {
                    User = group.Key,
                    TotalSteps = group.Sum(item => item.Steps)
                })
                .ToList();

            return _totalSteps;
        }

        public async Task<UserTotalSteps> GetStepDataForPeriodAsync(long startDate, long endDate)
        {
            List<StepModel> stepsList = new();
            while (startDate != endDate)
            {
                stepsList.Add(await _repo.GetStepsForDayAsTimestampAsync(startDate));
                startDate += 86400;
            }

            return new UserTotalSteps()
            {
                User = new UserModel() { Email = "You" },
                TotalSteps = stepsList.Sum(item => item.Steps)
            };
        }

        public async Task SaveStepData(StepDto stepDto)
        {
            await _repo.SaveChangesAsync(_mapper.MapToDatabase(stepDto));

            if (Connectivity.NetworkAccess == NetworkAccess.Internet && _userAuthenticationService.IsUserLoggedIn())
            {
                if (stepDto.Steps % 25 != 0) return;
                await _stepsSupa.AddOrUpdateSteps(stepDto);
                IsStepsChanged?.Invoke(stepDto.Steps);
            }
        }
    }
}
