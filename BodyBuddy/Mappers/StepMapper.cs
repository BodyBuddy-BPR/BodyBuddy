using BodyBuddy.Dtos;
using BodyBuddy.Helpers;
using BodyBuddy.Models;
using BodyBuddy.Models.Supabase;

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
                Date = DateHelper.ConvertToEpochTimeAtMidnightUtc(stepDto.Date)
            };
        }

        public StepModel MapToDatabaseFromSb(StepsSbModel stepsSbModel)
        {
            return new StepModel()
            {
                Steps = stepsSbModel.Steps,
                StepGoal = stepsSbModel.StepGoal,
                Date = DateHelper.ConvertToEpochTimeAtMidnightUtc(stepsSbModel.Date)
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
