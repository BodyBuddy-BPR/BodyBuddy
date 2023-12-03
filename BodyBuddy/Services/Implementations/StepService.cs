using BodyBuddy.Dtos;
using BodyBuddy.Mappers;
using BodyBuddy.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.Authentication;
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
        public async Task<StepDto> GetStepData()
        {
            //Get current date at midnight in UTC, and convert it to a timestamp
            DateTime currentDateTime = DateTime.UtcNow.Date;
            int currentDateTimestamp = (int)(currentDateTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            var stepData = await _repo.GetStepsForDayAsTimestampAsync(currentDateTimestamp);
            return _mapper.MapToDto(stepData);
        }

        public async Task SaveStepData(StepDto stepDto)
        {
            await _repo.SaveChangesAsync(_mapper.MapToDatabase(stepDto));

            if (Connectivity.NetworkAccess == NetworkAccess.Internet && _userAuthenticationService.IsUserLoggedIn())
                _stepsSupa.AddOrUpdateSteps(stepDto);
        }
    }
}
