using BodyBuddy.Models;
using BodyBuddy.Repositories;
using BodyBuddy.ViewModels.IntakeViewmodels;
using Mopups.Interfaces;
using Moq;

namespace UnitTest.ViewModels.IntakeViewModels
{
    public class IntakeViewModelTests
    {
        private IntakeViewModel target;
        private Mock<IIntakeRepository> mockRepo;
        private Mock<IPopupNavigation> mockPopupNavigation;
        private IntakeModel defaultIntake;

        [SetUp]
        public void Setup()
        {
            mockRepo = new Mock<IIntakeRepository>();
            mockPopupNavigation = new Mock<IPopupNavigation>();
            defaultIntake = new IntakeModel() { Id = 1, Date = 1697022414, CalorieCurrent = 0, CalorieGoal = 3500, WaterCurrent = 0, WaterGoal = 3000 };

            target = new IntakeViewModel(mockRepo.Object, mockPopupNavigation.Object);
        }

        [Test]
        public async Task IfNotBustGetIntakeGoalsIsCalledAndParametersSet()
        {
            // Arrange
            target.IsBusy = false;

            mockRepo.Setup(repo => repo.GetIntakeAsync()).ReturnsAsync(defaultIntake);

            // Act
            await target.GetIntakeGoals();

            // Assert
            Assert.That(target.IntakeDetails, Is.EqualTo(defaultIntake));
            Assert.That(target.CaloriesCurrent, Is.EqualTo(defaultIntake.CalorieCurrent));
            Assert.That(target.WaterCurrent, Is.EqualTo(defaultIntake.WaterCurrent));
            Assert.That(target.CalorieGoal, Is.EqualTo(defaultIntake.CalorieGoal));
            Assert.That(target.WaterGoal, Is.EqualTo(defaultIntake.WaterGoal));
        }

        [TestCase(0, 0)]
        [TestCase(1, 250)]
        [TestCase(3, 750)]
        [TestCase(5, 1250)]
        public async Task CorrectAmountOfWaterIsAddedWhenAddWaterClickedMethodIsCalledTest(int timesPressed, int waterCurrentResult)
        {
            // Arrange
            mockRepo.Setup(repo => repo.GetIntakeAsync()).ReturnsAsync(defaultIntake);

            // Act
            await target.GetIntakeGoals();
            for (int i = 0; i < timesPressed; i++)
            {
                await target.AddWaterClicked();
            }

            // Assert
            Assert.That(target.IntakeDetails, Is.EqualTo(defaultIntake));
            Assert.That(target.WaterCurrent, Is.EqualTo(waterCurrentResult));
        }

        [TestCase(200)]
        [TestCase(580)]
        [TestCase(1200)]
        public async Task CorrectAmountOfCaloriesIsAddedWhenAddCaloriesClickedMethodIsCalledTest(int calories)
        {
            // Arrange
            mockRepo.Setup(repo => repo.GetIntakeAsync()).ReturnsAsync(defaultIntake);

            // Act
            await target.GetIntakeGoals();
            await target.AddKcalClicked(calories);

            // Assert
            Assert.That(target.IntakeDetails, Is.EqualTo(defaultIntake));
            Assert.That(target.CaloriesCurrent, Is.EqualTo(calories));
        }

        [Test]
        public async Task SaveNewIntakeGoalReturnsFalseIfNewIntakeGoalIsZeroTest()
        {
            // Arrange
            target.NewIntakeGoal = 0;

            // Act
            bool result = await target.SaveNewIntakeGoal("Calorie");

            // Assert
            Assert.That(result, Is.EqualTo(false));
            Assert.That(target.ErrorMessage, Is.EqualTo("New intake goal cannot be empty."));
        }

        [TestCase("Calorie")]
        [TestCase("Water")]
        public async Task CorrectGoalIsChangedBasedOnIntakeTypeStringTest(string intakeType)
        {
            // Arrange
            target.IntakeDetails = defaultIntake;
            target.NewIntakeGoal = 3500;

            // Act
            bool result = await target.SaveNewIntakeGoal(intakeType);

            // Assert
            if (intakeType == "Calorie")
            {
                Assert.That(target.IntakeDetails.CalorieGoal, Is.EqualTo(3500));
            }
            else if (intakeType == "Water")
            {
                Assert.That(target.IntakeDetails.WaterGoal, Is.EqualTo(3500));
            }
            Assert.That(result, Is.EqualTo(true));
            Assert.That(target.ErrorMessage, Is.EqualTo(string.Empty));
            Assert.That(target.NewIntakeGoal, Is.EqualTo(0));
        }

        [Test]
        public void ErrorMessageAndNewIntakeGoalAreResetWhenPopupIsDeclinedTest()
        {
            // Arrange
            target.ErrorMessage = "Test Message";
            target.NewIntakeGoal = 3500;

            // Act
            target.DeclineEditIntake();

            // Assert
            Assert.That(target.ErrorMessage, Is.EqualTo(""));
            Assert.That(target.NewIntakeGoal, Is.EqualTo(0));
        }
    }
}