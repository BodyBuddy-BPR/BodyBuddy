using BodyBuddy.Dtos;
using BodyBuddy.Helpers;
using BodyBuddy.Models;
using BodyBuddy.Enums;
using BodyBuddy.Models.Supabase;

namespace BodyBuddy.Mappers
{
    public class StartupTestMapper
    {
        public StartupTestModel MapToDatabaseFromDto(StartupTestDto startupTestDto)
        {
            return new StartupTestModel()
            {
                Id = startupTestDto.Id,
                Name = startupTestDto.Name,
                Gender = (int)EnumMapper.GetGenderFromDisplayString(startupTestDto.Gender),
                Weight = startupTestDto.Weight,
                Height = startupTestDto.Height,
                Birthday = DateHelper.ConvertToEpochTimeAtMidnight(startupTestDto.Birthday),
                ActiveAmount = (int)EnumMapper.GetUserActivityFromDisplayString(startupTestDto.ActiveAmount),
                PassiveCalorieBurn = startupTestDto.PassiveCalorieBurn,
                TargetAreas = startupTestDto.TargetAreas,
                Goal = (int)EnumMapper.GetGoalFromDisplayString(startupTestDto.Goal)
            };
        }

        public StartupTestModel MapToDatabaseFromSb(StartupTestSbModel startupTestSbModel)
        {
            return new StartupTestModel()
            {
                Name = startupTestSbModel.Name,
                Gender = startupTestSbModel.Gender,
                Weight = startupTestSbModel.Weight,
                Height = startupTestSbModel.Height,
                Birthday = DateHelper.ConvertToEpochTimeAtMidnight(startupTestSbModel.Birthday),
                ActiveAmount = startupTestSbModel.ActiveAmount,
                PassiveCalorieBurn = startupTestSbModel.PassiveCalorieBurn,
                TargetAreas = startupTestSbModel.TargetAreas,
                Goal = startupTestSbModel.Goal
            };
        }

        public StartupTestSbModel MapToSbModel(StartupTestDto startupTestDto)
        {
            return new StartupTestSbModel()
            {
                Name = startupTestDto.Name,
                Gender = (int)EnumMapper.GetGenderFromDisplayString(startupTestDto.Gender),
                Weight = startupTestDto.Weight,
                Height = startupTestDto.Height,
                Birthday = startupTestDto.Birthday,
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
