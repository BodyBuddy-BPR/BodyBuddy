using BodyBuddy.Models;
using BodyBuddy.Repositories;
using BodyBuddy.ViewModels.IntakeViewmodels;
using Mopups.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.ViewModels
{
	public class IntakeViewModelTests
	{
		private IntakeViewModel target;
		private Mock<IIntakeRepository> mockRepo;
		private Mock<IPopupNavigation> mockPopupNavigation;

		[SetUp]
		public void Setup()
		{
			mockRepo = new Mock<IIntakeRepository>();
			mockPopupNavigation = new Mock<IPopupNavigation>();

			target = new IntakeViewModel(mockRepo.Object, mockPopupNavigation.Object);
		}

		[Test]
		public async Task IfNotBustGetIntakeGoalsIsCalled()
		{
			// Arrange
			target.IsBusy = false;

			var testIntake = new Intake() { Id = 1, Date = 1697022414, CalorieCurrent = 0, CalorieGoal = 3500, WaterCurrent = 0, WaterGoal = 3000 };
			mockRepo.Setup(repo => repo.GetIntakeAsync()).ReturnsAsync(testIntake);

			// Act
			await target.GetIntakeGoals();

			// Assert
			Assert.That(target.IntakeDetails, Is.EqualTo(testIntake));
			Assert.That(target.CaloriesCurrent, Is.EqualTo(testIntake.CalorieCurrent));
			Assert.That(target.WaterCurrent, Is.EqualTo(testIntake.WaterCurrent));
			Assert.That(target.CalorieGoal, Is.EqualTo(testIntake.CalorieGoal));
			Assert.That(target.WaterGoal, Is.EqualTo(testIntake.WaterGoal));
		}
	}
}