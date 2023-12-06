using BodyBuddy.Dtos;
using BodyBuddy.Enums;
using BodyBuddy.Helpers;
using BodyBuddy.Mappers;
using BodyBuddy.Models;

namespace UnitTest.Mappers
{
    public class StartupTestMapperTests
    {
        private StartupTestMapper target;
        private StartupTestModel startupTest;
        private StartupTestDto startupTestDto;


        [SetUp]
        public void Setup()
        {
            target = new StartupTestMapper();
            startupTest = new()
            {
                Id = 1,
                Name = "Børge",
                Gender = 0,
                Weight = 60.88,
                Height = 166,
                Birthday = 946688400,
                ActiveAmount = 0,
                PassiveCalorieBurn = 2500,
                Goal = 0
            };

            startupTestDto = new()
            {
                Id = 1,
                Name = "Børge",
                Gender = EnumMapper.GetDisplayString(Gender.Female),
                Weight = 60.88,
                Height = 166,
                Birthday = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                ActiveAmount = EnumMapper.GetDisplayString(UserActivity.Active),
                PassiveCalorieBurn = 2500,
                Goal = EnumMapper.GetDisplayString(Goal.GainMuscle)
            };
        }

        [Test]
        public void MapToDto_DoesNotReturnNull_WhenInputIsNull()
        {
            //Act
            var returnStartupTestDb = target.MapToDto(null);

            //Assert
            Assert.NotNull(returnStartupTestDb);
        }

        
        [TestCase("Female", 0, "Very Active", 0, "Gain Muscle", 0)]
        [TestCase("Male", 1, "Active", 1, "Gain Muscle", 0)]
        [TestCase("Prefer not to say", 2, "A Little Active", 2, "Lose Weight", 1)]
        [TestCase("Female", 0, "Not Very Active", 3, "Lose Weight", 1)]
        public void CorrectlyMappingDefaultDtoToDbEntity(string gender, int expectedGender, string activity, int expectedActivity, string goal, int expectedGoal)
        {
            // Arrange
            startupTestDto.Gender = gender;
            startupTestDto.ActiveAmount = activity;
            startupTestDto.Goal = goal;

            // Act
            var returnStartupTestDb = target.MapToDatabaseFromDto(startupTestDto);

            // Assert
            Assert.That(returnStartupTestDb.Id, Is.EqualTo(startupTestDto.Id));
            Assert.That(returnStartupTestDb.Name, Is.EqualTo(startupTestDto.Name));
            Assert.That(returnStartupTestDb.Gender, Is.EqualTo(expectedGender));
            Assert.That(returnStartupTestDb.Weight, Is.EqualTo(startupTestDto.Weight));
            Assert.That(returnStartupTestDb.Height, Is.EqualTo(startupTestDto.Height));
            Assert.That(returnStartupTestDb.Birthday, Is.EqualTo(946688400));
            Assert.That(returnStartupTestDb.ActiveAmount, Is.EqualTo(expectedActivity));
            Assert.That(returnStartupTestDb.PassiveCalorieBurn, Is.EqualTo(startupTestDto.PassiveCalorieBurn));
            Assert.That(returnStartupTestDb.Goal, Is.EqualTo(expectedGoal));
        }

        [TestCase("Female", 0, "Very Active", 0, "Gain Muscle", 0)]
        [TestCase("Male", 1, "Active", 1, "Gain Muscle", 0)]
        [TestCase("Prefer not to say", 2, "A Little Active", 2, "Lose Weight", 1)]
        [TestCase("Female", 0, "Not Very Active", 3, "Lose Weight", 1)]
        public void CorrectlyMappingDefaultDbToDto(string expectedGender, int gender, string expectedActivity, int activity, string expectedGoal, int goal)
        {
            // Arrange
            startupTest.Gender = gender;
            startupTest.ActiveAmount = activity;
            startupTest.Goal = goal;

            // Act
            var returnStartupTestDb = target.MapToDto(startupTest);

            // Assert
            Assert.That(returnStartupTestDb.Id, Is.EqualTo(startupTest.Id));
            Assert.That(returnStartupTestDb.Name, Is.EqualTo(startupTest.Name));
            Assert.That(returnStartupTestDb.Gender, Is.EqualTo(expectedGender));
            Assert.That(returnStartupTestDb.Weight, Is.EqualTo(startupTest.Weight));
            Assert.That(returnStartupTestDb.Height, Is.EqualTo(startupTest.Height));
            Assert.That(returnStartupTestDb.Birthday, Is.EqualTo(new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc)));
            Assert.That(returnStartupTestDb.ActiveAmount, Is.EqualTo(expectedActivity));
            Assert.That(returnStartupTestDb.PassiveCalorieBurn, Is.EqualTo(startupTest.PassiveCalorieBurn));
            Assert.That(returnStartupTestDb.Goal, Is.EqualTo(expectedGoal));
        }

        //Raise the testcases Integers to maximum number in Directionary+1 in the Mapper
        [TestCase(3,0,0)]
        [TestCase(0,4,0)]
        [TestCase(0,0,2)]
        public void NotImplementedDbToDto(int gender, int activity, int goal)
        {
            // Arrange
            startupTest.Gender = gender;
            startupTest.ActiveAmount = activity;
            startupTest.Goal = goal;

            // Act && Assert
            Assert.Throws<KeyNotFoundException>(()
    => target.MapToDto(startupTest));
        }

        //[TestCase("a", "Very Active", "Gain Muscle")]
        //[TestCase("Female", "a", "Gain Muscle")]
        //[TestCase("Female", "Very Active", "a")]
        //public void NotImplementedDtoToDatabaseEntity(string gender, string activity, string goal)
        //{
        //    // Arrange
        //    startupTestDto.Gender = gender;
        //    startupTestDto.ActiveAmount = activity;
        //    startupTestDto.Goal = goal;

        //    // Act && Assert
        //    Assert.Throws<KeyNotFoundException>(()
        //        => target.MapToDatabaseFromDto(startupTestDto));
        //}
    }
}
