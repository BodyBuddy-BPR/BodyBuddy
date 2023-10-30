using BodyBuddy.Models;
using BodyBuddy.Repositories;
using BodyBuddy.ViewModels.WorkoutViewModels;
using Moq;
using System.Collections.ObjectModel;
using BodyBuddy.Dtos;

namespace UnitTest.ViewModels.WorkoutViewModels
{
    public class StartedWorkoutViewModelTests
    {
        private StartedWorkoutViewModel target;
        private Mock<IExerciseRecordsRepository> exerciseRecordsMockRepo;
        private Mock<IWorkoutExercisesRepository> workoutExercisesMockRepo;

        private WorkoutDto workout;
        private ObservableCollection<ExerciseModel> exercises;

        [SetUp]
        public void Setup()
        {
            exerciseRecordsMockRepo = new Mock<IExerciseRecordsRepository>();
            workoutExercisesMockRepo = new Mock<IWorkoutExercisesRepository>();

            target = new StartedWorkoutViewModel(workoutExercisesMockRepo.Object, exerciseRecordsMockRepo.Object);

            workout = new WorkoutDto() { Id = 1, Name = "Workout1", PreMade = false };
            exercises = new ObservableCollection<ExerciseModel>
            {
                new () { Id = 1},
                new () { Id = 2},
                new () { Id = 3}
            };
        }

        [Test]
        public async Task ClearExercisesIfNotEmptyTest()
        {
            var displayedExercise = new ExerciseModel() { Id = 4 };

            // Arrange
            var returnExercises = new List<ExerciseModel>()
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
