using BodyBuddy.Models;
using BodyBuddy.Services;
using BodyBuddy.ViewModels.StartupTest;
using Moq;

namespace UnitTest.ViewModels.StartupTest
{
    public class StartupTestViewModelTests
    {
        private Mock<IStartupTestService> _serviceMock;
        private StartupTestViewModel target;

        [SetUp]
        public void Setup()
        {
            _serviceMock = new Mock<IStartupTestService>();
            target = new StartupTestViewModel(_serviceMock.Object);
        }

        [Test]
        public void Test()
        {
            Assert.True(true);
        }
    }
}