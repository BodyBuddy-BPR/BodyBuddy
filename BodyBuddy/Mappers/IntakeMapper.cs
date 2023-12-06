using BodyBuddy.Dtos;
using BodyBuddy.Helpers;
using BodyBuddy.Models;
using BodyBuddy.Models.Supabase;

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
                Date = DateHelper.ConvertToEpochTimeAtMidnightUtc(intakeDto.Date)
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

        //SbModels
        public IntakeSbModel MapToSbModel(IntakeDto intakeDto)
        {
            return new IntakeSbModel()
            {
                Date = intakeDto.Date,
                CalorieCurrent = intakeDto.CalorieCurrent,
                WaterCurrent = intakeDto.WaterCurrent,
                CalorieGoal = intakeDto.CalorieGoal,
                WaterGoal = intakeDto.WaterGoal,
            };
        }

        public IntakeModel MapToDatabaseFromSb(IntakeSbModel intakeSbModel)
        {
            return new IntakeModel()
            {
                CalorieGoal = intakeSbModel.CalorieGoal,
                WaterGoal = intakeSbModel.WaterGoal,
                CalorieCurrent = intakeSbModel.CalorieCurrent,
                WaterCurrent = intakeSbModel.WaterCurrent,
                Date = DateHelper.ConvertToEpochTimeAtMidnightUnspecified(intakeSbModel.Date)
            };
        }
    }
}
