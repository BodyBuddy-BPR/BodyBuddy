using BodyBuddy.Models;
using BodyBuddy.Repositories;
using BodyBuddy.ViewModels.WorkoutViewModels;
using Moq;
using System.Collections.ObjectModel;

namespace UnitTest.ViewModels.WorkoutViewModels
{
    public class StartedWorkoutViewModelTests
    {
        private StartedWorkoutViewModel target;
        private Mock<IExerciseRecordsRepository> exerciseRecordsMockRepo;
        private Mock<IWorkoutExercisesRepository> workoutExercisesMockRepo;

        private Workout workout;
        private ObservableCollection<Exercise> exercises;

        [SetUp]
        public void Setup()
        {
            exerciseRecordsMockRepo = new Mock<IExerciseRecordsRepository>();
            workoutExercisesMockRepo = new Mock<IWorkoutExercisesRepository>();

            target = new StartedWorkoutViewModel(workoutExercisesMockRepo.Object, exerciseRecordsMockRepo.Object);

            workout = new Workout() { Id = 1, Name = "Workout1", PreMade = 0 };
            exercises = new ObservableCollection<Exercise>()
            {
                new Exercise() { Id = 1},
                new Exercise() { Id = 2},
                new Exercise() { Id = 3}
            };
        }

        [Test]
        public async Task ClearExercisesIfNotEmptyTest()
        {
            var displayedExercise = new Exercise() { Id = 4 };

            // Arrange
            var returnExercises = new List<Exercise>()
            {
                displayedExercise,
                new Exercise() { Id = 5},
                new Exercise() { Id = 6},
                new Exercise() { Id = 7},
                new Exercise() { Id = 8}
            };

            target.IsBusy = false;
            target.Exercises = exercises;
            target.WorkoutDetails = workout;
            workoutExercisesMockRepo.Setup(repo => repo.GetExercisesInWorkout(It.IsAny<int>()))
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
