﻿using BodyBuddy.Dtos;
using BodyBuddy.Mappers;
using BodyBuddy.Repositories;
using BodyBuddy.Authentication;
using BodyBuddy.Helpers;
using BodyBuddy.Repositories.Supabase;

namespace BodyBuddy.Services.Implementations
{
    public class StepService : IStepService
    {
        private readonly IStepRepository _repo;
        private readonly IStepsSb _stepsSupa;
        private readonly IUserAuthenticationService _userAuthenticationService;

        private readonly StepMapper _mapper = new();

        public StepService(IStepRepository stepRepository, IStepsSb stepsSupabase, IUserAuthenticationService userAuthenticationService)
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
            return stepsList.GroupBy(step => step.User)
                .Select(group => new UserTotalSteps
                {
                    User = group.Key,
                    TotalSteps = group.Sum(item => item.Steps)
                })
                .ToList();
        }

        public async Task SaveStepData(StepDto stepDto)
        {
            await _repo.SaveChangesAsync(_mapper.MapToDatabase(stepDto));

            if (Connectivity.NetworkAccess == NetworkAccess.Internet && _userAuthenticationService.IsUserLoggedIn())
            {
                if (stepDto.Steps % 100 != 0) return;
                _stepsSupa.AddOrUpdateSteps(stepDto);
            }
        }
    }
}
