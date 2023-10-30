using BodyBuddy.ViewModels.ExerciseViewModels;
using BodyBuddy.Services;
using Moq;

namespace UnitTest.ViewModels.ExerciseViewModels
{
    public class CategoryViewModelTests
    {
        private CategoryViewModel _target;
        private IMock<IExerciseService> _mockExerciseService;

        [SetUp]
        public void Setup()
        {
            _mockExerciseService = new Mock<IExerciseService>();
            _target = new CategoryViewModel(_mockExerciseService.Object);
        }
    }
}
