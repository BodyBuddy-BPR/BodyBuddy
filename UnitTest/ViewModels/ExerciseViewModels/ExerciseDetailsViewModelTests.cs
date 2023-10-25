using BodyBuddy.Models;
using BodyBuddy.Repositories;
using BodyBuddy.ViewModels.ExerciseViewModels;
using Moq;

namespace UnitTest.ViewModels.ExerciseViewModels
{
    public class ExerciseDetailsViewModelTests
    {
        private Mock<IExerciseRepository> _exerciseRepoMock;
        private Mock<IConnectivity> _connectivityMock;
        private ExerciseDetailsViewModel target;

        [SetUp]
        public void Setup()
        {
            _exerciseRepoMock = new Mock<IExerciseRepository>();
            _connectivityMock = new Mock<IConnectivity>();
            target = new ExerciseDetailsViewModel(_exerciseRepoMock.Object, _connectivityMock.Object);
            target.ExerciseDetails = new ExerciseModel() { Id = 1 };
        }

        [Test]
        public async Task GetExerciseDetails_WithValidExercise_ReturnsDetails()
        {
            // Arrange
            var mockExercise = new ExerciseModel
            {
                Id = 1,
                Name = "Squat",
                Level = "Beginner",
                Images = "image1.jpg,image2.jpg"
            };

            _connectivityMock.Setup(c => c.NetworkAccess).Returns(NetworkAccess.Internet);
            _exerciseRepoMock.Setup(repo => repo.GetExerciseDetails(It.IsAny<int>())).ReturnsAsync(mockExercise);

            // Act
            await target.GetExerciseDetails();

            // Assert
            Assert.That(mockExercise.Name, Is.EqualTo(target.ExerciseDetails.Name));
            Assert.That(mockExercise.Level, Is.EqualTo(target.ExerciseDetails.Level));
            Assert.That(2, Is.EqualTo(target.ExerciseImages.Count));
        }

        [Test]
        public async Task ProcessExerciseDetails_WithNullProperties_ConvertsToEmptyString()
        {
            // Arrange
            var exercise = new ExerciseModel
            {
                Id=1
            };
            _connectivityMock.Setup(c => c.NetworkAccess).Returns(NetworkAccess.Internet);
            _exerciseRepoMock.Setup(repo => repo.GetExerciseDetails(It.IsAny<int>())).ReturnsAsync(exercise);

            // Act
            await target.GetExerciseDetails();

            // Assert
            Assert.That(target.ExerciseDetails.Name, Is.EqualTo(string.Empty));
            Assert.That(target.ExerciseDetails.Level, Is.EqualTo(string.Empty));
            Assert.That(target.ExerciseDetails.Category, Is.EqualTo(string.Empty));
            Assert.That(target.ExerciseDetails.PrimaryMuscles, Is.EqualTo(string.Empty));
            Assert.That(target.ExerciseDetails.SecondaryMuscles, Is.EqualTo(string.Empty));
            Assert.That(target.ExerciseDetails.Equipment, Is.EqualTo(string.Empty));
            Assert.That(target.ExerciseDetails.Force, Is.EqualTo(string.Empty));
            Assert.That(target.ExerciseDetails.Mechanic, Is.EqualTo(string.Empty));
            Assert.That(target.ExerciseDetails.Instructions, Is.EqualTo(string.Empty));
        }

        [TestCase("image1.jpg,image2.jpg,image3.jpg", 3)]
        [TestCase("image1.jpg,image2.jpg", 2)]
        [TestCase("image1.jpg", 1)]
        [TestCase("", 0)]
        [TestCase("image1.jpg,image2.jpg,image3.jpg,test.jpg", 4)]
        [TestCase("image1.jpg,image2.jpg,image3.jpg,test.jpg,test2.jpg", 5)]
        public async Task PopulateExerciseImagesList_WithValidImages_AddsImages(string imagePath, int amountOfImages)
        {
            // Arrange
            var mockExercise = new ExerciseModel
            {
                Id = 1,
                Name = "Squat",
                Level = "Beginner",
                Images = imagePath
            };
            _connectivityMock.Setup(c => c.NetworkAccess).Returns(NetworkAccess.Internet);
            _exerciseRepoMock.Setup(repo => repo.GetExerciseDetails(It.IsAny<int>())).ReturnsAsync(mockExercise);


            // Act
            await target.GetExerciseDetails();

            // Assert
            Assert.That(target.ExerciseImages.Count, Is.EqualTo(amountOfImages));
        }
    }
}
