using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.Dtos;
using BodyBuddy.Models;
using BodyBuddy.Services.Implementations;

namespace BodyBuddy.Mappers
{
    public class IntakeMapper
    {
        private readonly DateTimeService _dateTimeService = new DateTimeService();

        public IntakeModel MapToDatabase(IntakeDto intakeDto)
        {
            return new IntakeModel()
            {
                Id = intakeDto.Id,
                CalorieGoal = intakeDto.CalorieGoal,
                WaterGoal = intakeDto.WaterGoal,
                CalorieCurrent = intakeDto.CalorieCurrent,
                WaterCurrent = intakeDto.WaterCurrent,
                Date = _dateTimeService.ConvertToEpochTime(intakeDto.Date)
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
                Date = _dateTimeService.ConvertToDateTime(intakeModel.Date)
            };
        }
    }
}
