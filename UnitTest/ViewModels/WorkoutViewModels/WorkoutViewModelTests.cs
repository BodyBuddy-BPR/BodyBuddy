using BodyBuddy.Dtos;
using BodyBuddy.Services;
using BodyBuddy.ViewModels.WorkoutViewModels;
using Moq;

namespace UnitTest.ViewModels.WorkoutViewModels
{
    public class WorkoutViewModelTests
    {
        private WorkoutViewModel target;
        private Mock<IWorkoutService> mockService;
        private Mock<IWorkoutExercisesService> workoutExercisesMockService;
        private WorkoutDto workout1, workout2;
        private List<WorkoutDto> workoutList;

        [SetUp]
        public void Setup()
        {
            mockService = new Mock<IWorkoutService>();
            workoutExercisesMockService = new Mock<IWorkoutExercisesService>();
            target = new WorkoutViewModel(mockService.Object, workoutExercisesMockService.Object);

            workout1 = new WorkoutDto { Id = 3, Name = "Workout3", PreMade = false };
            workout2 = new WorkoutDto { Id = 4, Name = "Workout4", PreMade = false };

            workoutList = new List<WorkoutDto> { workout1, workout2 };

        }

        [Test]
        public async Task IfBusy_GetWorkoutPlanIsNotCalled()
        {
            //Arrange
            target.IsBusy = true;

            //Act
            await target.GetWorkoutPlans();

            //Assert
            mockService.Verify(service => service.GetWorkoutPlans(It.IsAny<bool>()), Times.Never());
        }

        [Test]
        public async Task ValidWorkout_EmptyName_ReturnsFalseAndSetsErrorMessage()
        {
            // Arrange
            target.WorkoutName = string.Empty;

            // Act
            bool result = await target.ValidWorkout(target.WorkoutName);

            // Assert
            Assert.That(result, Is.False);
            Assert.That(target.ErrorMessage, Is.EqualTo("Workout name cannot be empty."));
        }

        [Test]
        public async Task ValidWorkout_ExistingName_ReturnsFalseAndSetsErrorMessage()
        {
            // Arrange
            target.WorkoutName = "ExistingWorkout";
            mockService.Setup(service => service.DoesWorkoutAlreadyExist(target.WorkoutName)).ReturnsAsync(true);

            // Act
            bool result = await target.ValidWorkout(target.WorkoutName);

            // Assert
            Assert.That(result, Is.False);
            Assert.That(target.ErrorMessage, Is.EqualTo("A workoutplan with the name \"ExistingWorkout\" already exists."));
        }

        [Test]
        public async Task ValidWorkout_ValidName_ReturnsTrueAndClearsErrorMessage()
        {
            // Arrange
            target.WorkoutName = "NewWorkout";
            mockService.Setup(service => service.DoesWorkoutAlreadyExist(target.WorkoutName)).ReturnsAsync(false);

            // Act
            bool result = await target.ValidWorkout(target.WorkoutName);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(target.ErrorMessage, Is.Null);
        }

        #region GetWorkoutPlans method tests

        #endregion

    }
}