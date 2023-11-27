using BodyBuddy.Enums;
using BodyBuddy.Mappers;
using BodyBuddy.Models;
using BodyBuddy.Services;
using BodyBuddy.ViewModels.StartupTest;
using State = BodyBuddy.Enums.StartupTestStates;
using Moq;

namespace UnitTest.ViewModels.StartupTest
{
    public class StartupTestViewModelTests
    {
        private Mock<IStartupTestService> _serviceMock;
        private StartupTestViewModel target;

        [SetUp]
        public void Setup()
        {
            _serviceMock = new Mock<IStartupTestService>();
            target = new StartupTestViewModel(_serviceMock.Object);
        }

        [TestCase(0, true, false, false, false, false, false, false, false, false, false)]
        [TestCase(1, false, true, false, false, false, false, false, false, false, false)]
        [TestCase(2, false, false, true, false, false, false, false, false, false, false)]
        [TestCase(3, false, false, false, true, false, false, false, false, false, false)]
        [TestCase(4, false, false, false, false, true, false, false, false, false, false)]
        [TestCase(5, false, false, false, false, false, true, false, false, false, false)]
        [TestCase(6, false, false, false, false, false, false, true, false, false, false)]
        [TestCase(7, false, false, false, false, false, false, false, true, false, false)]
        [TestCase(8, false, false, false, false, false, false, false, false, true, false)]
        [TestCase(9, false, false, false, false, false, false, false, false, false, true)]
        [TestCase(10, false, false, false, false, false, false, false, false, false, false)]
        public void Test_StateNext_Progression_AndPropertyVisibility(int stateNumber, bool welcomeVisible, bool nameVisible, bool genderVisible, bool weightVisible, bool heightVisible,
            bool birthdayVisible, bool activeVisible, bool passiveCalorieVisible, bool focusVisible, bool goalVisible)
        {
            // Arrange
            TargetSetup();

            // Act
            GoToState(stateNumber);

            // Assert
            Assert.That(target.IsWelcomeVisible, Is.EqualTo(welcomeVisible));
            Assert.That(target.IsNameVisible, Is.EqualTo(nameVisible));
            Assert.That(target.IsGenderVisible, Is.EqualTo(genderVisible));
            Assert.That(target.IsWeightVisible, Is.EqualTo(weightVisible));
            Assert.That(target.IsHeightVisible, Is.EqualTo(heightVisible));
            Assert.That(target.IsBirthdayVisible, Is.EqualTo(birthdayVisible));
            Assert.That(target.IsActiveVisible, Is.EqualTo(activeVisible));
            Assert.That(target.IsFocusAreaVisible, Is.EqualTo(focusVisible));
            Assert.That(target.IsPassiveCalorieBurnVisible, Is.EqualTo(passiveCalorieVisible));
            Assert.That(target.IsGoalVisible, Is.EqualTo(goalVisible));
        }

        [TestCase(0, "")]
        [TestCase(1, "What is your name?")]
        [TestCase(2, "What is your gender?")]
        [TestCase(3, "What do you weigh?")]
        [TestCase(4, "How tall are you?")]
        [TestCase(5, "When is your birthday?")]
        [TestCase(6, "How active are you?")]
        [TestCase(7, "Passive Calorie Burn")]
        [TestCase(8, "What are your focus areas?")]
        [TestCase(9, "What are your goals?")]
        [TestCase(10, "You're all set!")]
        public void ProgressingThroughStates_UpdatesQuestionnaireTextCorrectly(int stateNumber, string expectedQuestionnaireText)
        {
            // Arrange
            TargetSetup();

            // Act
            GoToState(stateNumber);

            // Assert
            Assert.That(target.QuestionnaireText, Is.EqualTo(expectedQuestionnaireText));
        }

        [TestCase(0,false)]
        [TestCase(1,false)]
        [TestCase(2,false)]
        [TestCase(3,false)]
        [TestCase(4,false)]
        [TestCase(5,false)]
        [TestCase(6,false)]
        [TestCase(7,false)]
        [TestCase(8,false)]
        [TestCase(9,false)]
        [TestCase(10,true)]
        public void Verify_SubmitIsVisible_OnlyOnDoneState(int stateNumber, bool expectedSubmitVisible)
        {
            // Arrange
            TargetSetup();

            // Act
            GoToState(stateNumber);

            // Assert
            Assert.That(target.SubmitDataIsVisible, Is.EqualTo(expectedSubmitVisible));

        }

        private void GoToState(int stateNumber)
        {
            for (var i = 0; i < stateNumber; i++)
            {
                target.NextButton();
            }
        }

        private void TargetSetup()
        {
            target.StartupTestDto.Name = "Name";
            target.StartupTestDto.Gender = EnumMapper.GetDisplayString(Gender.Female);
            target.StartupTestDto.ActiveAmount = EnumMapper.GetDisplayString(UserActivity.Active);
            target.StartupTestDto.Goal = EnumMapper.GetDisplayString(Goal.GainMuscle);
            target.StartupTestDto.Weight = 1.0;
            target.StartupTestDto.Height = 1;
            target.StartupTestDto.PassiveCalorieBurn = 1;
            target.StartupTestDto.TargetAreas = "Abs, Back";
            target.StartupTestDto.Birthday = new DateTime(1997, 1, 1);
        }
    }
}