using BodyBuddy.Models;
using BodyBuddy.Repositories;
using BodyBuddy.ViewModels.WorkoutViewModels;
using Moq;

namespace UnitTests.ViewModels
{
    public class PreMadeWorkoutsViewModelTests
    {
        private PreMadeWorkoutsViewModel target;
        private Mock<IWorkoutRepository> mockRepo;
        private Workout workout1;
        private Workout workout2;
        private List<Workout> workoutList;

        [SetUp]
        public void Setup()
        {
            mockRepo = new Mock<IWorkoutRepository>();
            target = new PreMadeWorkoutsViewModel(mockRepo.Object);
            workout1 = new Workout() { Id=1, Name="Workout1", PreMade=1};
            workout1 = new Workout() { Id=2, Name="Workout2", PreMade=0};
            workoutList = new List<Workout>() { workout1 ,workout2 };
        }

        [Test]
        public async Task IfBusy_GetWorkoutPlanIsNotCalled()
        {
            //Assert
            target.IsBusy = true;

            //Act
            await target.GetPreMadeWorkoutPlans();

            //Verify
            mockRepo.Verify(repo => repo.GetWorkoutPlansAsync(It.IsAny<int>()), Times.Never());
        }

        [Test]
        public async Task IfNotBusy_GetWorkoutPlanIsCalled()
        {
            //Assert
            target.IsBusy = false;
            mockRepo.Setup(repo => repo.GetWorkoutPlansAsync(It.IsAny<int>())).ReturnsAsync(workoutList);

            //Act
            await target.GetPreMadeWorkoutPlans();

            //Verify
            mockRepo.Verify(repo => repo.GetWorkoutPlansAsync(It.IsAny<int>()), Times.Once());
            Assert.That(target.Workouts, Is.EqualTo(workoutList));
        }
    }
}