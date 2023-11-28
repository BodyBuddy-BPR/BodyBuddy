using BodyBuddy.Dtos;
using BodyBuddy.Mappers;
using BodyBuddy.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.Services.Implementations
{
    public class StepService : IStepService
    {
        private IStepRepository _repo;
        private StepMapper mapper = new StepMapper();

        public StepService(IStepRepository stepRepository)
        {
            _repo = stepRepository;
        }
        public async Task<StepDto> GetStepData()
        {
            var stepData = await _repo.GetStepsAsync();
            return mapper.MapToDto(stepData);
        }

        public void SaveStepData(StepDto stepDto)
        {
            _repo.SaveChangesAsync(mapper.MapToDatabase(stepDto));
        }
    }
}
