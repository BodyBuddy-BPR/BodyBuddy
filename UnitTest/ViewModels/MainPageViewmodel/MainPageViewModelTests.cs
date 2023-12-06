using BodyBuddy.Authentication;
using BodyBuddy.Dtos;
using BodyBuddy.Services;
using BodyBuddy.ViewModels;
using BodyBuddy.Views.Authentication;
using Microsoft.Maui.Controls;
using Mopups.Interfaces;
using Mopups.Pages;
using Moq;

namespace UnitTest.ViewModels.MainPageViewmodel
{
    public class MainPageViewModelTests
    {
        private MainPageViewModel _target;
        private Mock<IStepService> _mockStepService;
        private Mock<IStartupTestService> _mockStartupTestService;
        private Mock<IUserAuthenticationService> _mockUserAuthService;
        private Mock<IQuoteService> _mockQuoteService;
        private Mock<IWorkoutService> _mockWorkoutService;
        private Mock<IPopupNavigation> _mockPopupNavigation;
        private Mock<IChallengeService> _mockChallengeService;
        private StepDto _defaultStep;

        [SetUp]
        public void Setup()
        {
            _mockStepService = new Mock<IStepService>();
            _mockUserAuthService = new Mock<IUserAuthenticationService>();
            _mockQuoteService = new Mock<IQuoteService>();
            _mockPopupNavigation = new Mock<IPopupNavigation>();
            _mockStartupTestService = new Mock<IStartupTestService>();
            _mockWorkoutService = new Mock<IWorkoutService>();
            _mockChallengeService = new Mock<IChallengeService>();
            _defaultStep = new StepDto() { Id = 1, Date = new DateTime(2023, 10, 10), Steps = 0, StepGoal = 10000};

            _target = new MainPageViewModel(_mockStepService.Object, _mockQuoteService.Object, _mockUserAuthService.Object, _mockPopupNavigation.Object, _mockStartupTestService.Object, _mockWorkoutService.Object, _mockChallengeService.Object);
        }

        [Test]
        public void GetDailyQuote_SuccessfulServiceCall_SetsQuoteProperty()
        {
            // Arrange
            var mockQuote = new QuoteDto() { Quote = "Test Quote", Author = "Anonymous" };
            _mockQuoteService.Setup(q => q.GetDailyQuote()).ReturnsAsync(mockQuote);

            // Act
            _target.GetDailyQuote().Wait();

            // Assert
            Assert.That(_target.Quote, Is.EqualTo(mockQuote));
        }

        [Test]
        public void SaveNewStepGoalValue_WithValidInput_SavesStepDataAndUpdatesProgress()
        {
            // Arrange
            _target.NewStepGoal = 15000;

            // Act
            _target.UserSteps = _defaultStep;
            var result = _target.SaveNewStepGoalValue().Result;

            // Assert
            Assert.IsTrue(result);
            Assert.That(_target.UserSteps.StepGoal, Is.EqualTo(_target.NewStepGoal));
            Assert.That(_target.StepProgress, Is.EqualTo((double)_target.UserSteps.Steps / _target.UserSteps.StepGoal));
            _mockStepService.Verify(s => s.SaveStepData(It.IsAny<StepDto>()), Times.Once);
        }

        [Test]
        public async Task SetWorkoutsToShow_WithValidData_SetsPropertiesCorrectly()
        {
            // Arrange
            var startupTestDto = new StartupTestDto
            {
                TargetAreas = "Abs, Upperbody"
            };

            var workoutServiceMock = new Mock<IWorkoutService>();
            workoutServiceMock.Setup(w => w.GetWorkoutPlans(true)).ReturnsAsync(new List<WorkoutDto>());

            var startupTestServiceMock = new Mock<IStartupTestService>();
            startupTestServiceMock.Setup(s => s.GetStartupTestData()).ReturnsAsync(startupTestDto);

            _target = new MainPageViewModel(_mockStepService.Object, _mockQuoteService.Object, _mockUserAuthService.Object, _mockPopupNavigation.Object, startupTestServiceMock.Object, workoutServiceMock.Object, _mockChallengeService.Object);

            // Act
            await _target.SetWorkoutsToShow();

            // Assert
            Assert.IsNotNull(_target.TargetAreas);
            Assert.AreEqual(2, _target.TargetAreas.Count);

            Assert.IsNotNull(_target.AllWorkouts);

            Assert.IsNotNull(_target.WorkoutsToShow);
            Assert.IsEmpty(_target.WorkoutsToShow);
        }

        [Test]
        public void UpdateWorkoutsToShow_WithSearchTerm_FiltersWorkoutsCorrectly()
        {
            // Arrange
            var workout1 = new WorkoutDto { Name = "Abs Workout" };
            var workout2 = new WorkoutDto { Name = "Upperbody Workout" };

            _target.AllWorkouts = new List<WorkoutDto> { workout1, workout2 };

            // Act
            _target.UpdateWorkoutsToShow("Abs");

            // Assert
            Assert.AreEqual(1, _target.WorkoutsToShow.Count);
            Assert.Contains(workout1, _target.WorkoutsToShow);
        }

        [Test]
        public void UpdateWorkoutsToShow_WithEmptySearchTerm_SetsAllWorkouts()
        {
            // Arrange
            var workout1 = new WorkoutDto { Name = "Abs Workout" };
            var workout2 = new WorkoutDto { Name = "Upperbody Workout" };

            _target.AllWorkouts = new List<WorkoutDto> { workout1, workout2 };

            // Act
            _target.UpdateWorkoutsToShow(string.Empty);

            // Assert
            Assert.AreEqual(2, _target.WorkoutsToShow.Count);
            Assert.Contains(workout1, _target.WorkoutsToShow);
            Assert.Contains(workout2, _target.WorkoutsToShow);
        }

        [Test]
        public void UpdateWorkoutsToShow_WithNullSearchTerm_SetsAllWorkouts()
        {
            // Arrange
            var workout1 = new WorkoutDto { Name = "Abs Workout" };
            var workout2 = new WorkoutDto { Name = "Upperbody Workout" };

            _target.AllWorkouts = new List<WorkoutDto> { workout1, workout2 };

            // Act
            _target.UpdateWorkoutsToShow(null);

            // Assert
            Assert.AreEqual(2, _target.WorkoutsToShow.Count);
            Assert.Contains(workout1, _target.WorkoutsToShow);
            Assert.Contains(workout2, _target.WorkoutsToShow);
        }
    }
}
