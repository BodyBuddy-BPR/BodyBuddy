using BodyBuddy.Dtos;
using BodyBuddy.Helpers;
using BodyBuddy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.Mappers
{
    public class StepMapper
    {
        public StepModel MapToDatabase(StepDto stepDto)
        {
            return new StepModel()
            {
                Id = stepDto.Id,
                Steps = stepDto.Steps,
                StepGoal = stepDto.StepGoal,
                Date = DateHelper.ConvertToEpochTime(stepDto.Date)
            };
        }

        public StepDto MapToDto(StepModel stepsModel)
        {
            if (stepsModel == null)
                return new StepDto();

            return new StepDto()
            {
                Id = stepsModel.Id,
                Steps = stepsModel.Steps,
                StepGoal = stepsModel.StepGoal,
                Date = DateHelper.ConvertToDateTime(stepsModel.Date)
            };
        }
    }
}
