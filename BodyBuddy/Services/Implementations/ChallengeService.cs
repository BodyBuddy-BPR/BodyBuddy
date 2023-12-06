using BodyBuddy.Authentication;
using BodyBuddy.Dtos;
using BodyBuddy.Helpers;
using BodyBuddy.Mappers;
using BodyBuddy.Models.Supabase;
using BodyBuddy.Repositories.Supabase;

namespace BodyBuddy.Services.Implementations
{
    public class ChallengeService : IChallengeService
    {
        private readonly IChallengeSbRepository _challengeSbRepository;
        private readonly IStepService _stepService;
        private readonly IUserAuthenticationService _userAuthenticationService;

        private readonly ChallengeMapper _challengeMapper = new();

        private List<ActiveChallengeSbModel> _activeChallengeSbModels = new();
        private List<ChallengeDto> _challengeDtos = new();

        public ChallengeService(IChallengeSbRepository challengeSbRepository, IStepService stepService, IUserAuthenticationService authenticationService)
        {
            _challengeSbRepository = challengeSbRepository;
            _stepService = stepService;
            _userAuthenticationService = authenticationService;

            _stepService.IsStepsChanged += UpdateStepChallenges;
        }

        public async Task<List<ChallengeDto>> GetActiveChallenges()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet || !_userAuthenticationService.IsUserLoggedIn())
                return _challengeDtos;

            //Clear to update data
            _challengeDtos.Clear();

            //Only get challenges if not gotten
            if (!_activeChallengeSbModels.Any())
                _activeChallengeSbModels = await _challengeSbRepository.GetActiveChallenges();

            //Adding Steps to all active challenges
            foreach (var activeChallenge in _activeChallengeSbModels)
            {
                var stepsCountForUser = await _stepService.GetStepsForPeriodFriends();
                stepsCountForUser.Add(await _stepService.GetStepDataForPeriodAsync(DateHelper.ConvertToEpochTimeAtMidnight(activeChallenge.From), DateHelper.GetCurrentDayAtMidnight()));
                stepsCountForUser = stepsCountForUser.OrderByDescending(u => u.TotalSteps).ToList();

                _challengeDtos.Add(_challengeMapper.MapFromSbToDto(activeChallenge, stepsCountForUser));
            }

            //Returning the challenges
            return _challengeDtos;
        }

        private async void UpdateStepChallenges(int steps)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet &&
                _userAuthenticationService.IsUserLoggedIn()) return;

            foreach (var challengeDto in _challengeDtos)
            {
                var totalSteps = challengeDto.UserTotalSteps.Sum(x => x.TotalSteps);
                await _challengeSbRepository.UpdateChallengeData(challengeDto.ActiveChallengeId, totalSteps);
            }
        }
    }
}
