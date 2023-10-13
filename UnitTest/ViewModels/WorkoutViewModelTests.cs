using BodyBuddy.Models;
using BodyBuddy.Repositories;
using BodyBuddy.ViewModels.WorkoutViewModels;
using Moq;

namespace UnitTests.ViewModels
{
    public class WorkoutViewModelTests
    {
        private WorkoutViewModel target;
        private Mock<IWorkoutRepository> mockRepo;
        private Workout preMadeWorkout1, preMadeWorkout2;
        private Workout workout1, workout2;
        private List<Workout> premadeWorkoutList, workoutList;

        [SetUp]
        public void Setup()
        {
            mockRepo = new Mock<IWorkoutRepository>();
            target = new WorkoutViewModel(mockRepo.Object);

            preMadeWorkout1 = new Workout() { Id = 1, Name = "Workout1", PreMade = 1 };
            preMadeWorkout2 = new Workout() { Id = 2, Name = "Workout2", PreMade = 1 };

            workout1 = new Workout() { Id = 3, Name = "Workout3", PreMade = 0 };
            workout2 = new Workout() { Id = 4, Name = "Workout4", PreMade = 0 };

            premadeWorkoutList = new List<Workout>() { preMadeWorkout1, preMadeWorkout2 };
            workoutList = new List<Workout>() { workout1, workout2 };

        }

        [Test]
        public async Task IfBusy_GetWorkoutPlanIsNotCalled()
        {
            //Arrange
            target.IsBusy = true;

            //Act
            await target.GetWorkoutPlans();

            //Assert
            mockRepo.Verify(repo => repo.GetWorkoutPlansAsync(It.IsAny<int>()), Times.Never());
        }

        [Test]
        public async Task IfNotBusy_GetWorkoutPlanIsCalled()
        {
            //Arrange
            target.IsBusy = false;
            mockRepo.Setup(repo => repo.GetWorkoutPlansAsync(1)).ReturnsAsync(premadeWorkoutList);
            mockRepo.Setup(repo => repo.GetWorkoutPlansAsync(0)).ReturnsAsync(workoutList);
            target.PreMadeWorkouts.Add(workout2);
            target.Workouts.Add(workout2);

            //Act
            await target.GetWorkoutPlans();

            //Assert
            mockRepo.Verify(repo => repo.GetWorkoutPlansAsync(It.IsAny<int>()), Times.Exactly(2));
            Assert.That(target.PreMadeWorkouts, Is.EqualTo(premadeWorkoutList));
            Assert.That(target.Workouts, Is.EqualTo(workoutList));
        }
    }
}