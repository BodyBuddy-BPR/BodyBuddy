using BodyBuddy.Models;
using BodyBuddy.Repositories;
using BodyBuddy.ViewModels.ExerciseViewModels;
using Moq;

namespace UnitTest.ViewModels.ExerciseViewModels
{
    public class MuscleGroupViewModelTests
    {
        private MuscleGroupViewModel target;
        [SetUp]
        public void Setup()
        {
            Mock<IExerciseRepository> mock = new Mock<IExerciseRepository>();
            target = new MuscleGroupViewModel(mock.Object);
            target.ExerciseCategory = new Exercise();
        }

        [Test]
        public async Task GetMusclegroupsReturnsCorrectTargetAreasAndNumbers_Strength()
        {
            // Arrange
            target.ExerciseCategory.Category = "Strength";
            
            // Act
            await target.GetMusclegroups();

            // Assert
            Assert.That(4, Is.EqualTo(target.MuscleGroups.FirstOrDefault(g => g.Key == "Upper Body")?.Count()));
            Assert.That(3, Is.EqualTo(target.MuscleGroups.FirstOrDefault(g => g.Key == "Arms")?.Count()));
            Assert.That(3, Is.EqualTo(target.MuscleGroups.FirstOrDefault(g => g.Key == "Back")?.Count()));
            Assert.That(1, Is.EqualTo(target.MuscleGroups.FirstOrDefault(g => g.Key == "Abs and Core")?.Count()));
            Assert.That(6, Is.EqualTo(target.MuscleGroups.FirstOrDefault(g => g.Key == "Lower Body")?.Count()));
        }

        [Test]
        public async Task GetMusclegroupsReturnsCorrectTargetAreasAndNumbers_Stretching()
        {
            // Arrange
            target.ExerciseCategory.Category = "Stretching";

            // Act
            await target.GetMusclegroups();

            // Assert
            Assert.That(3, Is.EqualTo(target.MuscleGroups.FirstOrDefault(g => g.Key == "Upper Body")?.Count()));
            Assert.That(3, Is.EqualTo(target.MuscleGroups.FirstOrDefault(g => g.Key == "Arms")?.Count()));
            Assert.That(3, Is.EqualTo(target.MuscleGroups.FirstOrDefault(g => g.Key == "Back")?.Count()));
            Assert.That(1, Is.EqualTo(target.MuscleGroups.FirstOrDefault(g => g.Key == "Abs and Core")?.Count()));
            Assert.That(6, Is.EqualTo(target.MuscleGroups.FirstOrDefault(g => g.Key == "Lower Body")?.Count()));
        }

        [Test]
        public async Task GetMusclegroupsReturnsCorrectTargetAreasAndNumbers_Plyometrics()
        {
            // Arrange
            target.ExerciseCategory.Category = "Plyometrics";

            // Act
            await target.GetMusclegroups();

            // Assert
            Assert.That(2, Is.EqualTo(target.MuscleGroups.FirstOrDefault(g => g.Key == "Upper Body")?.Count()));
            Assert.That(1, Is.EqualTo(target.MuscleGroups.FirstOrDefault(g => g.Key == "Arms")?.Count()));
            Assert.That(1, Is.EqualTo(target.MuscleGroups.FirstOrDefault(g => g.Key == "Back")?.Count()));
            Assert.That(1, Is.EqualTo(target.MuscleGroups.FirstOrDefault(g => g.Key == "Abs and Core")?.Count()));
            Assert.That(3, Is.EqualTo(target.MuscleGroups.FirstOrDefault(g => g.Key == "Lower Body")?.Count()));
        }

        [Test]
        public async Task GetMusclegroupsReturnsCorrectTargetAreasAndNumbers_Strongman()
        {
            // Arrange
            target.ExerciseCategory.Category = "Strongman";

            // Act
            await target.GetMusclegroups();

            // Assert
            Assert.That(2, Is.EqualTo(target.MuscleGroups.FirstOrDefault(g => g.Key == "Upper Body")?.Count()));
            Assert.That(1, Is.EqualTo(target.MuscleGroups.FirstOrDefault(g => g.Key == "Arms")?.Count()));
            Assert.That(1, Is.EqualTo(target.MuscleGroups.FirstOrDefault(g => g.Key == "Back")?.Count()));
            Assert.That(2, Is.EqualTo(target.MuscleGroups.FirstOrDefault(g => g.Key == "Lower Body")?.Count()));
        }

        [Test]
        public async Task GetMusclegroupsReturnsCorrectTargetAreasAndNumbers_Powerlifting()
        {
            // Arrange
            target.ExerciseCategory.Category = "Powerlifting";

            // Act
            await target.GetMusclegroups();

            // Assert
            Assert.That(1, Is.EqualTo(target.MuscleGroups.FirstOrDefault(g => g.Key == "Upper Body")?.Count()));
            Assert.That(1, Is.EqualTo(target.MuscleGroups.FirstOrDefault(g => g.Key == "Arms")?.Count()));
            Assert.That(1, Is.EqualTo(target.MuscleGroups.FirstOrDefault(g => g.Key == "Back")?.Count()));
            Assert.That(3, Is.EqualTo(target.MuscleGroups.FirstOrDefault(g => g.Key == "Lower Body")?.Count()));
        }

        [Test]
        public async Task GetMusclegroupsReturnsCorrectTargetAreasAndNumbers_Cardio()
        {
            // Arrange
            target.ExerciseCategory.Category = "Cardio";

            // Act
            await target.GetMusclegroups();

            // Assert
            Assert.That(2, Is.EqualTo(target.MuscleGroups.FirstOrDefault(g => g.Key == "Lower Body")?.Count()));
        }

        [Test]
        public async Task GetMusclegroupsReturnsCorrectTargetAreasAndNumbers_OlympicWeightlifting()
        {
            // Arrange
            target.ExerciseCategory.Category = "Olympic weightlifting";

            // Act
            await target.GetMusclegroups();

            // Assert
            Assert.That(2, Is.EqualTo(target.MuscleGroups.FirstOrDefault(g => g.Key == "Upper Body")?.Count()));
            Assert.That(3, Is.EqualTo(target.MuscleGroups.FirstOrDefault(g => g.Key == "Lower Body")?.Count()));
        }
    }
}
