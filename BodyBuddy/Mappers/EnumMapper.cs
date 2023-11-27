using BodyBuddy.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.Helpers;

namespace BodyBuddy.Mappers
{
    public static class EnumMapper
    {
        private static readonly Dictionary<Gender, string> GenderDisplayStrings = new()
        {
            { Gender.Female, Strings.STARTUP_GENDER_FEMALE },
            { Gender.Male, Strings.STARTUP_GENDER_MALE },
            { Gender.None, Strings.STARTUP_GENDER_NONE },
        };

        private static readonly Dictionary<UserActivity, string> UserActivityDisplayStrings = new()
        {
            { UserActivity.VeryActive, Strings.STARTUP_ACTIVITY_VERYACTIVE },
            { UserActivity.Active, Strings.STARTUP_ACTIVITY_ACTIVE},
            { UserActivity.LittleActive, Strings.STARTUP_ACTIVITY_LITTLEACTIVE },
            { UserActivity.NotVeryActive, Strings.STARTUP_ACTIVITY_NOTVERYACTIVE },
        };

        private static readonly Dictionary<Goal, string> GoalDisplayStrings = new()
        {
            { Goal.LoseWeight, Strings.STARTUP_GOAL_LOSEWEIGHT },
            { Goal.GainMuscle, Strings.STARTUP_GOAL_GAINMUSCLE },
        };

        private static readonly Dictionary<TargetArea, string> FocusAreaDisplayStrings = new()
        {
            { TargetArea.UpperBody, Strings.STARTUP_FOCUSAREA_UPPERBODY },
            { TargetArea.LowerBody, Strings.STARTUP_FOCUSAREA_LOWERBODY },
            { TargetArea.Abs, Strings.STARTUP_FOCUSAREA_ABS },
            { TargetArea.Back, Strings.STARTUP_FOCUSAREA_BACK },
        };

        public static string GetDisplayString(Gender gender) => GenderDisplayStrings[gender];

        public static string GetDisplayString(UserActivity userActivity) => UserActivityDisplayStrings[userActivity];

        public static string GetDisplayString(Goal goal) => GoalDisplayStrings[goal];

        public static string GetDisplayString(TargetArea targetArea) => FocusAreaDisplayStrings[targetArea];

        public static Gender GetGenderFromDisplayString(string displayString) => GenderDisplayStrings.FirstOrDefault(x => x.Value == displayString).Key;

        public static UserActivity GetUserActivityFromDisplayString(string displayString) => UserActivityDisplayStrings.FirstOrDefault(x => x.Value == displayString).Key;

        public static Goal GetGoalFromDisplayString(string displayString) => GoalDisplayStrings.FirstOrDefault(x => x.Value == displayString).Key;

        public static string GetFocusAreaFromDisplayString(List<string> displayStrings)
        {
            bool firstTime = false;
            StringBuilder result = new StringBuilder();

            foreach (string str in displayStrings)
            {
                if (!firstTime)
                {
                    result.Append(str);
                    firstTime = true;
                }
                result.Append(", " + str);
            }

            return result.ToString();
        }

        public static List<string> GetFocusAreaToListFromDisplayString(string focusAreasString)
        {
            string[] focusAreasArray = focusAreasString.Split(',').Select(area => area.Trim()).ToArray();

            List<string> focusAreasList = new List<string>(focusAreasArray);

            return focusAreasList;
        }
    }
}
