using BodyBuddy.Models;
using BodyBuddy.Services;
using BodyBuddy.ViewModels.StartupTest;
using State = BodyBuddy.ViewModels.StartupTest.StartupTestViewModel.State;
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

        [TestCase(0, true, false, false, false, false, false, false, false)]
        [TestCase(1, false, true, false, false, false, false, false, false)]
        [TestCase(2, false, false, true, false, false, false, false, false)]
        [TestCase(3, false, false, false, true, false, false, false, false)]
        [TestCase(4, false, false, false, false, true, false, false, false)]
        [TestCase(5, false, false, false, false, false, true, false, false)]
        [TestCase(6, false, false, false, false, false, false, true, false)]
        [TestCase(7, false, false, false, false, false, false, false, true)]
        [TestCase(8, false, false, false, false, false, false, false, false)]
        public void Test_StateNext_Progression_AndPropertyVisibility(int nextClicks, bool nameVisible, bool genderVisible, bool weightVisible, bool heightVisible,
            bool birthdayVisible, bool activeVisible, bool passiveCalorieVisible, bool goalVisible)
        {
            target.Name = "Name";
            target.Gender = "Gender";
            target.Active = "Active";
            target.Goal = "Goal";
            target.Weight = 1.0;
            target.Height = 1;
            target.PassiveCalorieBurn = 1;
            target.SelectedDate = new DateTime(1997, 1, 1);
            for (int i = 0; i < nextClicks; i++)
            {
                target.NextButton();
            }

            Assert.That(target.IsNameVisible, Is.EqualTo(nameVisible));
            Assert.That(target.IsGenderVisible, Is.EqualTo(genderVisible));
            Assert.That(target.IsWeightVisible, Is.EqualTo(weightVisible));
            Assert.That(target.IsHeightVisible, Is.EqualTo(heightVisible));
            Assert.That(target.IsBirthdayVisible, Is.EqualTo(birthdayVisible));
            Assert.That(target.IsActiveVisible, Is.EqualTo(activeVisible));
            Assert.That(target.IsPassiveCalorieBurnVisible, Is.EqualTo(passiveCalorieVisible));
            Assert.That(target.IsGoalVisible, Is.EqualTo(goalVisible));
        }

        [TestCase(0, "What is your name?")]
        [TestCase(1, "What is your gender?")]
        [TestCase(2, "What is your weight?")]
        [TestCase(3, "When is your height?")]
        [TestCase(4, "When is your birthday?")]
        [TestCase(5, "How active are you?")]
        [TestCase(6, "What is your passive calorie burn?")]
        [TestCase(7, "What are your workout goals?")]
        [TestCase(8, "You're done!")]
        public void ProgressingThroughStates_UpdatesQuestionnaireTextCorrectly(int nextClicks, string expectedQuestionnaireText)
        {
            target.Name = "Name";
            target.Gender = "Gender";
            target.Active = "Active";
            target.Goal = "Goal";
            target.Weight = 1.0;
            target.Height = 1;
            target.PassiveCalorieBurn = 1;
            target.SelectedDate = new DateTime(1997, 1, 1);
            for (int i = 0; i < nextClicks; i++)
            {
                target.NextButton();
            }

            Assert.That(target.QuestionnaireText, Is.EqualTo(expectedQuestionnaireText));
        }
    }
}