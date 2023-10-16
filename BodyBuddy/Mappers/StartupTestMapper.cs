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
        public StartupTestMapper() { }

        public StartupTest MapFromDtoToDb(StartupTestDto startupTestDto)
        {
            return new StartupTest()
            {
                Id = startupTestDto.Id,
                Name = startupTestDto.Name,
                Gender = GenderToInteger(startupTestDto.Gender),
                Weight = startupTestDto.Weight,
                Height = startupTestDto.Height,
                Birthday = BirthdayToInteger(startupTestDto.Birthday),
                ActiveAmount = ActiveAmountToInteger(startupTestDto.ActiveAmount),
                PassiveCalorieBurn = startupTestDto.PassiveCalorieBurn,
                Goal = GoalToInteger(startupTestDto.Goal)
            };
        }

        public StartupTestDto MapFromDbToDto(StartupTest startupTest)
        {
            throw new NotImplementedException();
        }

        #region Converters From Dto --> DB
        private int GoalToInteger(string goal)
        {
            throw new NotImplementedException();
        }

        private int ActiveAmountToInteger(string activeAmount)
        {
            throw new NotImplementedException();
        }

        private int BirthdayToInteger(DateTime birthday)
        {
            throw new NotImplementedException();
        }

        private int GenderToInteger(string gender)
        {
            if (gender == Strings.FEMALE)
                return 0;
            else if (gender == Strings.MALE)
                return 1;
            else if (gender == Strings.NO_GENDER)
                return 2;
            else
                throw new NotImplementedException();
        }
    }
    #endregion
}
}
