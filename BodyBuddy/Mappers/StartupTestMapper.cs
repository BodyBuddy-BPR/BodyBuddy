using BodyBuddy.Dtos;
using BodyBuddy.Helpers;
using BodyBuddy.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.Enums;

namespace BodyBuddy.Mappers
{
    public class StartupTestMapper
    {
        public StartupTestModel MapToDatabase(StartupTestDto startupTestDto)
        {
            return new StartupTestModel()
            {
                Id = startupTestDto.Id,
                Name = startupTestDto.Name,
                Gender = (int)EnumMapper.GetGenderFromDisplayString(startupTestDto.Gender),
                Weight = startupTestDto.Weight,
                Height = startupTestDto.Height,
                Birthday = DateHelper.ConvertToEpochTime(startupTestDto.Birthday),
                ActiveAmount = (int)EnumMapper.GetUserActivityFromDisplayString(startupTestDto.ActiveAmount),
                PassiveCalorieBurn = startupTestDto.PassiveCalorieBurn,
                TargetAreas = startupTestDto.TargetAreas,
                Goal = (int)EnumMapper.GetGoalFromDisplayString(startupTestDto.Goal)
            };
        }

        public StartupTestDto MapToDto(StartupTestModel startupTest)
        {
            if (startupTest == null)
                return new StartupTestDto();

            return new StartupTestDto()
            {
                Id = startupTest.Id,
                Name = startupTest.Name,
                Gender = EnumMapper.GetDisplayString((Gender)startupTest.Gender),
                Weight = startupTest.Weight,
                Height = startupTest.Height,
                Birthday = DateHelper.ConvertToDateTime(startupTest.Birthday),
                ActiveAmount = EnumMapper.GetDisplayString((UserActivity)startupTest.ActiveAmount),
                PassiveCalorieBurn = startupTest.PassiveCalorieBurn,
                TargetAreas = startupTest.TargetAreas,
                Goal = EnumMapper.GetDisplayString((Goal)startupTest.Goal)
            };
        }
    }
}
