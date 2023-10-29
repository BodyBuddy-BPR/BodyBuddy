using BodyBuddy.Enums;
using BodyBuddy.Mappers;

namespace UnitTest.Mappers
{
    public class EnumMapperTests
    {
        [Test]
        public void AllGenderEnumValues_AreMappedCorrectly()
        {
            foreach (Gender gender in Enum.GetValues(typeof(Gender)))
            {
                // Act
                var displayString = EnumMapper.GetDisplayString(gender);
                var mappedGender = EnumMapper.GetGenderFromDisplayString(displayString);

                // Assert
                Assert.That(mappedGender, Is.EqualTo(gender), $"Mismatch for Gender enum value: {gender}");
            }
        }

        [Test]
        public void AllUserActivityEnumValues_AreMappedCorrectly()
        {
            foreach (UserActivity userActivity in Enum.GetValues(typeof(UserActivity)))
            {
                // Act
                var displayString = EnumMapper.GetDisplayString(userActivity);
                var mappedUserActivity = EnumMapper.GetUserActivityFromDisplayString(displayString);

                // Assert
                Assert.That(mappedUserActivity, Is.EqualTo(userActivity), $"Mismatch for UserActivity enum value: {userActivity}");
            }
        }

        [Test]
        public void AllGoalEnumValues_AreMappedCorrectly()
        {
            foreach (Goal goal in Enum.GetValues(typeof(Goal)))
            {
                // Act
                var displayString = EnumMapper.GetDisplayString(goal);
                var mappedGoal = EnumMapper.GetGoalFromDisplayString(displayString);

                // Assert
                Assert.That(mappedGoal, Is.EqualTo(goal), $"Mismatch for Goal enum value: {goal}");
            }
        }

        [Test]
        public void AllFocusAreaEnumValues_AreMappedCorrectly()
        {
            foreach (FocusArea focusArea in Enum.GetValues(typeof(FocusArea)))
            {
                // Act
                var displayString = EnumMapper.GetDisplayString(focusArea);
                var mappedFocusArea = EnumMapper.GetFocusAreaFromDisplayString(displayString);

                // Assert
                Assert.That(mappedFocusArea, Is.EqualTo(focusArea), $"Mismatch for FocusArea enum value: {focusArea}");
            }
        }
    }
}