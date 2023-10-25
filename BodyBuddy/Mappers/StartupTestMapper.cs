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
    public class StartupTestMapper
    {
        #region MappingStringsToIntegers --> Add here if more options are being added later!
        private readonly Dictionary<string, int> _goalToInteger = new()
        {
    { Strings.STARTUP_GOAL_GAINMUSCLE, 0 },
    { Strings.STARTUP_GOAL_LOSEWEIGHT, 1 }
};

        private readonly Dictionary<string, int> _activityToInteger = new()
        {
    { Strings.STARTUP_ACTIVITY_VERYACTIVE, 0 },
    { Strings.STARTUP_ACTIVITY_ACTIVE, 1 },
    { Strings.STARTUP_ACTIVITY_LITTLEACTIVE, 2 },
    { Strings.STARTUP_ACTIVITY_NOTVERYACTIVE, 3 }
};

        private readonly Dictionary<string, int> _genderToInteger = new()
        {
    { Strings.STARTUP_GENDER_FEMALE, 0 },
    { Strings.STARTUP_GENDER_MALE, 1 },
    { Strings.STARTUP_GENDER_NONE, 2 }
};

        private readonly Dictionary<int, string> _goalToString;
        private readonly Dictionary<int, string> _activityToString;
        private readonly Dictionary<int, string> _genderToString;
        #endregion

        public StartupTestMapper()
        {
            _goalToString = _goalToInteger.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
            _activityToString = _activityToInteger.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
            _genderToString = _genderToInteger.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
        }

        public StartupTest MapToDatabase(StartupTestDto startupTestDto)
        {
            return new StartupTest()
            {
                Id = startupTestDto.Id,
                Name = startupTestDto.Name,
                Gender = GenderToInteger(startupTestDto.Gender),
                Weight = startupTestDto.Weight,
                Height = startupTestDto.Height,
                Birthday = DateTimeToEpoch(startupTestDto.Birthday),
                ActiveAmount = ActivityToInteger(startupTestDto.ActiveAmount),
                PassiveCalorieBurn = startupTestDto.PassiveCalorieBurn,
                Goal = GoalToInteger(startupTestDto.Goal)
            };
        }

        public StartupTestDto MapToDto(StartupTest startupTest)
        {
            if(startupTest == null)
                return new StartupTestDto();

            return new StartupTestDto()
            {
                Id = startupTest.Id,
                Name = startupTest.Name,
                Gender = IntegerToGender(startupTest.Gender),
                Weight = startupTest.Weight,
                Height = startupTest.Height,
                Birthday = EpochToDateTime(startupTest.Birthday),
                ActiveAmount = IntegerToActivity(startupTest.ActiveAmount),
                PassiveCalorieBurn = startupTest.PassiveCalorieBurn,
                Goal = IntegerToGoal(startupTest.Goal)
            };
        }



        #region Converters
        //Birthday
        private long DateTimeToEpoch(DateTime dateTime)
        {
            DateTime epochStart = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)(dateTime.ToUniversalTime() - epochStart).TotalSeconds;

        }

        private DateTime EpochToDateTime(long epoch)
        {
            DateTime epochStart = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epochStart.AddSeconds(epoch);
        }

        //Goal
        public int GoalToInteger(string goal)
        {
            return MapToInteger(goal, _goalToInteger);
        }

        public string IntegerToGoal(int value)
        {
            return MapToString(value, _goalToString);
        }

        //Activity
        public int ActivityToInteger(string activity)
        {
            return MapToInteger(activity, _activityToInteger);
        }

        public string IntegerToActivity(int value)
        {
            return MapToString(value, _activityToString);
        }

        // Gender
        public int GenderToInteger(string gender)
        {
            return MapToInteger(gender, _genderToInteger);
        }

        public string IntegerToGender(int value)
        {
            return MapToString(value, _genderToString);
        }

        private int MapToInteger(string value, Dictionary<string, int> mapping)
        {
            if (mapping.TryGetValue(value, out int result))
            {
                return result;
            }
            throw new NotImplementedException(value);
        }

        private string MapToString(int value, Dictionary<int, string> mapping)
        {
            if (mapping.TryGetValue(value, out string result))
            {
                return result;
            }
            throw new NotImplementedException(value.ToString());
        }
    }
    #endregion
}
