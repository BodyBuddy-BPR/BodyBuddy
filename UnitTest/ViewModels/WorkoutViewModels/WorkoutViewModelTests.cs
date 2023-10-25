using BodyBuddy.Dtos;
using BodyBuddy.Models;
using BodyBuddy.Repositories;
using BodyBuddy.Services;
using BodyBuddy.ViewModels.WorkoutViewModels;
using Moq;

namespace UnitTest.ViewModels.WorkoutViewModels
{
    public class WorkoutViewModelTests
    {
        private WorkoutViewModel target;
        private Mock<IWorkoutService> mockService;
        private Mock<IWorkoutExercisesRepository> workoutExercisesMockRepo;
        private WorkoutDto workout1, workout2;
        private List<WorkoutDto> workoutList;

        [SetUp]
        public void Setup()
        {
            mockService = new Mock<IWorkoutService>();
            workoutExercisesMockRepo = new Mock<IWorkoutExercisesRepository>();
            target = new WorkoutViewModel(mockService.Object, workoutExercisesMockRepo.Object);

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

        [TestCase(true)]
        [TestCase(false)]
        public async Task GetWorkoutPlanIsCalledAndReturnsCorrectWorkoutList(bool preMade)
        {
            //Arrange
            target.IsBusy = false;
            target.IsPreMadeWorkout = preMade;
            mockService.Setup(service => service.GetWorkoutPlans(preMade)).ReturnsAsync(workoutList);
            target.Workouts.Add(workout2);

            //Act
            await target.GetWorkoutPlans();

            //Assert
            mockService.Verify(service => service.GetWorkoutPlans(preMade), Times.Exactly(1));
            mockService.Verify(service => service.GetWorkoutPlans(It.IsAny<bool>()), Times.Exactly(1));
            Assert.That(target.Workouts, Is.EqualTo(workoutList));
            Assert.That(target.Workouts.Count, Is.EqualTo(2));
        }
    }
}