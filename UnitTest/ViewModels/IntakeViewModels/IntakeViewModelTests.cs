using BodyBuddy.Dtos;
using BodyBuddy.Models;
using BodyBuddy.Repositories;
using BodyBuddy.Services;
using BodyBuddy.ViewModels.IntakeViewModels;
using Mopups.Interfaces;
using Moq;

namespace UnitTest.ViewModels.IntakeViewModels
{
    public class IntakeViewModelTests
    {
        private IntakeViewModel _target;
        private Mock<IIntakeService> _mockService;
        private Mock<IPopupNavigation> _mockPopupNavigation;
        private IntakeDto _defaultIntake;

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<IIntakeService>();
            _mockPopupNavigation = new Mock<IPopupNavigation>();
            _defaultIntake = new IntakeDto() { Id = 1, Date = new DateTime(2023,10,10), CalorieCurrent = 0, CalorieGoal = 3500, WaterCurrent = 0, WaterGoal = 3000 };

            _target = new IntakeViewModel(_mockService.Object, _mockPopupNavigation.Object);
        }

        [Test]
        public async Task IfNotBustGetIntakeGoalsIsCalledAndParametersSet()
        {
            // Arrange
            _target.IsBusy = false;

            _mockService.Setup(service => service.GetIntakeAsync()).ReturnsAsync(_defaultIntake);

            // Act
            await _target.GetIntakeGoals();

            // Assert
            Assert.That(_target.IntakeDetails, Is.EqualTo(_defaultIntake));
        }

        [TestCase(0, 0)]
        [TestCase(1, 250)]
        [TestCase(3, 750)]
        [TestCase(5, 1250)]
        public async Task CorrectAmountOfWaterIsAddedWhenAddWaterClickedMethodIsCalledTest(int timesPressed, int waterCurrentResult)
        {
            // Arrange
            _mockService.Setup(service => service.GetIntakeAsync()).ReturnsAsync(_defaultIntake);

            // Act
            await _target.GetIntakeGoals();
            for (int i = 0; i < timesPressed; i++)
            {
                await _target.AddWaterClicked();
            }

            // Assert
            Assert.That(_target.IntakeDetails, Is.EqualTo(_defaultIntake));
            Assert.That(_target.IntakeDetails.WaterCurrent, Is.EqualTo(waterCurrentResult));
        }

        [TestCase(200)]
        [TestCase(580)]
        [TestCase(1200)]
        public async Task CorrectAmountOfCaloriesIsAddedWhenAddCaloriesClickedMethodIsCalledTest(int calories)
        {
            // Arrange
            _mockService.Setup(service => service.GetIntakeAsync()).ReturnsAsync(_defaultIntake);

            // Act
            await _target.GetIntakeGoals();
            await _target.AddKcalClicked(calories);

            // Assert
            Assert.That(_target.IntakeDetails, Is.EqualTo(_defaultIntake));
            Assert.That(_target.IntakeDetails.CalorieCurrent, Is.EqualTo(calories));
        }

        [TestCase(0)]
        [TestCase(-10)]
        public async Task SaveNewIntakeGoalReturnsFalseIfNewIntakeGoalIsZeroOrNegativeTest(int newGoal)
        {
            // Arrange
            _target.NewIntakeGoal = newGoal;

            // Act
            bool result = await _target.SaveNewIntakeValues("Calorie");

            // Assert
            Assert.That(result, Is.EqualTo(false));
            Assert.That(_target.ErrorMessage, Is.EqualTo("New intake goal cannot be negative or zero."));
        }

        [Test]
		public async Task SaveNewIntakeGoalReturnsFalseIfNewIntakeCurrentIsNegativeTest()
		{
            // Arrange
            _target.NewCurrentIntake = -10;

			// Act
			bool result = await _target.SaveNewIntakeValues("Calorie");

			// Assert
			Assert.That(result, Is.EqualTo(false));
			Assert.That(_target.ErrorMessage, Is.EqualTo("Current intake cannot be negative."));
		}


		[TestCase("Calorie")]
        [TestCase("Water")]
        public async Task CorrectGoalIsChangedBasedOnIntakeTypeStringTest(string intakeType)
        {
            // Arrange
            _target.IntakeDetails = _defaultIntake;
            _target.NewIntakeGoal = 3500;

            // Act
            bool result = await _target.SaveNewIntakeValues(intakeType);

            // Assert
            if (intakeType == "Calorie")
            {
                Assert.That(_target.IntakeDetails.CalorieGoal, Is.EqualTo(3500));
            }
            else if (intakeType == "Water")
            {
                Assert.That(_target.IntakeDetails.WaterGoal, Is.EqualTo(3500));
            }
            Assert.That(result, Is.EqualTo(true));
            Assert.That(_target.ErrorMessage, Is.EqualTo(string.Empty));
            Assert.That(_target.NewIntakeGoal, Is.EqualTo(0));
        }

        [Test]
        public void ErrorMessageAndNewIntakeGoalAreResetWhenPopupIsDeclinedTest()
        {
            // Arrange
            _target.ErrorMessage = "Test Message";
            _target.NewIntakeGoal = 3500;

            // Act
            _target.DeclineEditIntake();

            // Assert
            Assert.That(_target.ErrorMessage, Is.EqualTo(""));
            Assert.That(_target.NewIntakeGoal, Is.EqualTo(0));
        }
	}
}