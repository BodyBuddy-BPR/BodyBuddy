using BodyBuddy.Enums;
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

        private static readonly Dictionary<TargetArea, string> TargetAreaDisplayStrings = new()
        {
            { TargetArea.UpperBody, Strings.STARTUP_TARGETAREA_UPPERBODY },
            { TargetArea.LowerBody, Strings.STARTUP_TARGETAREA_LOWERBODY },
            { TargetArea.Abs, Strings.STARTUP_TARGETAREA_ABS },
            { TargetArea.Back, Strings.STARTUP_TARGETAREA_BACK },
        };

        public static string GetDisplayString(Gender gender) => GenderDisplayStrings[gender];

        public static string GetDisplayString(UserActivity userActivity) => UserActivityDisplayStrings[userActivity];

        public static string GetDisplayString(Goal goal) => GoalDisplayStrings[goal];

        public static string GetDisplayString(TargetArea targetArea) => TargetAreaDisplayStrings[targetArea];

        public static Gender GetGenderFromDisplayString(string displayString) => GenderDisplayStrings.FirstOrDefault(x => x.Value == displayString).Key;

        public static UserActivity GetUserActivityFromDisplayString(string displayString) => UserActivityDisplayStrings.FirstOrDefault(x => x.Value == displayString).Key;

        public static Goal GetGoalFromDisplayString(string displayString) => GoalDisplayStrings.FirstOrDefault(x => x.Value == displayString).Key;
    }
}
