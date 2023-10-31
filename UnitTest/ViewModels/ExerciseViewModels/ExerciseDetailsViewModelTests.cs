using BodyBuddy.Dtos;
using BodyBuddy.Models;
using BodyBuddy.Repositories;
using BodyBuddy.Services;
using BodyBuddy.ViewModels.ExerciseViewModels;
using Moq;

namespace UnitTest.ViewModels.ExerciseViewModels
{
    public class ExerciseDetailsViewModelTests
    {
        private Mock<IExerciseService> _mock;
        private Mock<IConnectivity> _connectivityMock;
        private ExerciseDetailsViewModel target;

        [SetUp]
        public void Setup()
        {
            _mock = new Mock<IExerciseService>();
            _connectivityMock = new Mock<IConnectivity>();
            target = new ExerciseDetailsViewModel(_mock.Object, _connectivityMock.Object)
            {
                ExerciseDetails = new ExerciseDto() { Id = 1 }
            };
        }

        [Test]
        public async Task GetExerciseDetails_WithValidExercise_ReturnsDetails()
        {
            // Arrange
            var mockExercise = new ExerciseDto
            {
                Id = 1,
                Name = "Squat",
                Level = "Beginner",
                Images = "image1.jpg,image2.jpg"
            };

            _connectivityMock.Setup(c => c.NetworkAccess).Returns(NetworkAccess.Internet);
            _mock.Setup(repo => repo.GetExerciseDetails(It.IsAny<int>())).ReturnsAsync(mockExercise);

            // Act
            await target.GetExerciseDetails();

            // Assert
            Assert.That(target.ExerciseDetails.Name, Is.EqualTo(mockExercise.Name));
            Assert.That(target.ExerciseDetails.Level, Is.EqualTo(mockExercise.Level));
            Assert.That(target.ExerciseImages.Count, Is.EqualTo(2));
        }

        //[Test]
        //public async Task ProcessExerciseDetails_WithNullProperties_ConvertsToEmptyString()
        //{
        //    // Arrange
        //    var exercise = new ExerciseDto
        //    {
        //        Id=1
        //    };
        //    _connectivityMock.Setup(c => c.NetworkAccess).Returns(NetworkAccess.Internet);
        //    _mock.Setup(repo => repo.GetExerciseDetails(It.IsAny<int>())).ReturnsAsync(exercise);

        //    // Act
        //    await target.GetExerciseDetails();

        //    // Assert
        //    Assert.That(target.ExerciseDetails.Name, Is.EqualTo(string.Empty));
        //    Assert.That(target.ExerciseDetails.Level, Is.EqualTo(string.Empty));
        //    Assert.That(target.ExerciseDetails.Category, Is.EqualTo(string.Empty));
        //    Assert.That(target.ExerciseDetails.PrimaryMuscles, Is.EqualTo(string.Empty));
        //    Assert.That(target.ExerciseDetails.SecondaryMuscles, Is.EqualTo(string.Empty));
        //    Assert.That(target.ExerciseDetails.Equipment, Is.EqualTo(string.Empty));
        //    Assert.That(target.ExerciseDetails.Force, Is.EqualTo(string.Empty));
        //    Assert.That(target.ExerciseDetails.Mechanic, Is.EqualTo(string.Empty));
        //    Assert.That(target.ExerciseDetails.Instructions, Is.EqualTo(string.Empty));
        //}

        [TestCase("image1.jpg,image2.jpg,image3.jpg", 3)]
        [TestCase("image1.jpg,image2.jpg", 2)]
        [TestCase("image1.jpg", 1)]
        [TestCase("", 0)]
        [TestCase("image1.jpg,image2.jpg,image3.jpg,test.jpg", 4)]
        [TestCase("image1.jpg,image2.jpg,image3.jpg,test.jpg,test2.jpg", 5)]
        public async Task PopulateExerciseImagesList_WithValidImages_AddsImages(string imagePath, int amountOfImages)
        {
            // Arrange
            var mockExercise = new ExerciseDto
            {
                Id = 1,
                Name = "Squat",
                Level = "Beginner",
                Images = imagePath
            };
            _connectivityMock.Setup(c => c.NetworkAccess).Returns(NetworkAccess.Internet);
            _mock.Setup(repo => repo.GetExerciseDetails(It.IsAny<int>())).ReturnsAsync(mockExercise);


            // Act
            await target.GetExerciseDetails();

            // Assert
            Assert.That(target.ExerciseImages.Count, Is.EqualTo(amountOfImages));
        }
    }
}
