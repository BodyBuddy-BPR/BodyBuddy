using BodyBuddy.Models;
using BodyBuddy.Repositories;
using BodyBuddy.ViewModels.WorkoutViewModels;
using Moq;

namespace UnitTest.ViewModels.WorkoutViewModels
{
    public class WorkoutViewModelTests
    {
        private WorkoutViewModel target;
        private Mock<IWorkoutRepository> mockRepo;
        private Mock<IWorkoutExercisesRepository> workoutExercisesMockRepo;
        private Workout preMadeWorkout1, preMadeWorkout2;
        private Workout workout1, workout2;
        private List<Workout> premadeWorkoutList;
        private List<Workout> workoutList;

        [SetUp]
        public void Setup()
        {
            mockRepo = new Mock<IWorkoutRepository>();
            workoutExercisesMockRepo = new Mock<IWorkoutExercisesRepository>();
            target = new WorkoutViewModel(mockRepo.Object, workoutExercisesMockRepo.Object);

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

        [TestCase(true, 1)]
        [TestCase(false, 0)]
        public async Task GetWorkoutPlanIsCalledAndReturnsCorrectWorkoutList(bool boolIsPremade, int isPremadeNumber)
        {
            //Arrange
            target.IsBusy = false;
            target.IsPreMadeWorkout = boolIsPremade;
            mockRepo.Setup(repo => repo.GetWorkoutPlansAsync(isPremadeNumber)).ReturnsAsync(workoutList);
            target.Workouts.Add(workout2);

            //Act
            await target.GetWorkoutPlans();

            //Assert
            mockRepo.Verify(repo => repo.GetWorkoutPlansAsync(isPremadeNumber), Times.Exactly(1));
            mockRepo.Verify(repo => repo.GetWorkoutPlansAsync(It.IsAny<int>()), Times.Exactly(1));
            Assert.That(target.Workouts, Is.EqualTo(workoutList));
            Assert.That(target.Workouts.Count, Is.EqualTo(2));
        }
    }
}