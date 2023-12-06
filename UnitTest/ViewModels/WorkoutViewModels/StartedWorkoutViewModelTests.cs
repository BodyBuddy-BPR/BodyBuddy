using BodyBuddy.ViewModels.WorkoutViewModels;
using Moq;
using BodyBuddy.Dtos;
using BodyBuddy.Services;

namespace UnitTest.ViewModels.WorkoutViewModels
{
    public class StartedWorkoutViewModelTests
    {
        private StartedWorkoutViewModel target;
        private Mock<IExerciseRecordsService> exerciseRecordsMockService;
        private Mock<IWorkoutExercisesService> workoutExercisesMockService;

        private WorkoutDto workout;
        private List<ExerciseDto> exercises;

        [SetUp]
        public void Setup()
        {
            exerciseRecordsMockService = new Mock<IExerciseRecordsService>();
            workoutExercisesMockService = new Mock<IWorkoutExercisesService>();

            target = new StartedWorkoutViewModel(workoutExercisesMockService.Object, exerciseRecordsMockService.Object);

            workout = new WorkoutDto() { Id = 1, Name = "Workout1", PreMade = false };
            exercises = new List<ExerciseDto>
            {
                new () { Id = 1},
                new () { Id = 2},
                new () { Id = 3}
            };
        }

        [Test]
        public async Task ClearExercisesIfNotEmptyTest()
        {
            var displayedExercise = new ExerciseDto() { Id = 4 };

            // Arrange
            var returnExercises = new List<ExerciseDto>()
            {
                displayedExercise,
                new() { Id = 5},
                new() { Id = 6},
                new() { Id = 7},
                new() { Id = 8}
            };

            target.IsBusy = false;
            target.Exercises = exercises;
            target.WorkoutDetails = workout;
            workoutExercisesMockService.Setup(service => service.GetExercisesInWorkout(It.IsAny<int>()))
                .ReturnsAsync(returnExercises);

            // Act 
            await target.GetExercisesFromWorkout();

            // Assert
            Assert.That(target.Exercises, Is.EqualTo(returnExercises));
            Assert.That(target.IsBusy, Is.EqualTo(false));
            Assert.That(target.DisplayedExercise, Is.EqualTo(displayedExercise));
        }

    }
}
