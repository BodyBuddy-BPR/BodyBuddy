using BodyBuddy.Dtos;
using BodyBuddy.Helpers;
using BodyBuddy.Models;

namespace BodyBuddy.Mappers
{
    public class IntakeMapper
    {
        public IntakeModel MapToDatabase(IntakeDto intakeDto)
        {
            return new IntakeModel()
            {
                Id = intakeDto.Id,
                CalorieGoal = intakeDto.CalorieGoal,
                WaterGoal = intakeDto.WaterGoal,
                CalorieCurrent = intakeDto.CalorieCurrent,
                WaterCurrent = intakeDto.WaterCurrent,
                Date = DateHelper.ConvertToEpochTime(intakeDto.Date)
            };
        }

        public IntakeDto MapToDto(IntakeModel intakeModel)
        {
            if (intakeModel == null)
                return new IntakeDto();

            return new IntakeDto()
            {
                Id = intakeModel.Id,
                CalorieGoal = intakeModel.CalorieGoal,
                WaterGoal = intakeModel.WaterGoal,
                CalorieCurrent = intakeModel.CalorieCurrent,
                WaterCurrent = intakeModel.WaterCurrent,
                Date = DateHelper.ConvertToDateTime(intakeModel.Date)
            };
        }
    }
}
