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
        private Mock<IUserAuthenticationService> _mockUserAuthService;
        private Mock<IQuoteService> _mockQuoteService;
        private Mock<IPopupNavigation> _mockPopupNavigation;
        private StepDto _defaultStep;

        [SetUp]
        public void Setup()
        {
            _mockStepService = new Mock<IStepService>();
            _mockUserAuthService = new Mock<IUserAuthenticationService>();
            _mockQuoteService = new Mock<IQuoteService>();
            _mockPopupNavigation = new Mock<IPopupNavigation>();
            _defaultStep = new StepDto() { Id = 1, Date = new DateTime(2023, 10, 10), Steps = 0, StepGoal = 10000};

            _target = new MainPageViewModel(_mockStepService.Object, _mockQuoteService.Object, _mockUserAuthService.Object, _mockPopupNavigation.Object);
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
    }
}
