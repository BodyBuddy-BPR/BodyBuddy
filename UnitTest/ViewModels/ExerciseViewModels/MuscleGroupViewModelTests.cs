using BodyBuddy.Services;
using BodyBuddy.ViewModels.ExerciseViewModels;
using Moq;

namespace UnitTest.ViewModels.ExerciseViewModels
{
    public class MuscleGroupViewModelTests
    {
        private MuscleGroupViewModel _target;
        private Mock<IExerciseService> mock;

        [SetUp]
        public void Setup()
        {
            mock = new Mock<IExerciseService>();
            _target = new MuscleGroupViewModel(mock.Object);
        }
    }
}
