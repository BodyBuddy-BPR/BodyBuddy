﻿using BodyBuddy.ViewModels.Profile;
using BodyBuddy.Services;
using Moq;
using BodyBuddy.Mappers;
using BodyBuddy.Authentication;

namespace UnitTest.ViewModels.ProfileViewModels
{
	public class ProfileViewModelTests
	{
		private Mock<IStartupTestService> _startupServiceMock;
		private Mock<IIntakeService> _intakeServiceMock;
		private Mock<IUserAuthenticationService> _userAuthenticationServiceMock;
		private ProfileViewModel target;
		private readonly IntakeMapper _mapper = new();

		[SetUp]
		public void Setup()
		{
			_startupServiceMock = new Mock<IStartupTestService>();
			_intakeServiceMock = new Mock<IIntakeService>();
			_userAuthenticationServiceMock = new Mock<IUserAuthenticationService>();
			target = new ProfileViewModel(_startupServiceMock.Object, _intakeServiceMock.Object, _userAuthenticationServiceMock.Object);
		}

		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		[TestCase(4)]
		[TestCase(5)]
		[TestCase(6)]
		[TestCase(7)]
		public async Task Test_That_Correct_CurrentSelectedDate_Is_Set_And_DateDifference_Is_Correct_Based_On_Button_Clicked(int buttonNumber)
		{
			// Arrange
			target.currentDayOfWeek = (int)DateTime.UtcNow.DayOfWeek;

			// Act
			await target.WeekdayButtonClicked(buttonNumber);

			// Assert
			Assert.That(target.CurrentSelectedDate, Is.EqualTo(buttonNumber));
			Assert.That(target.currentAndSelectedDayDifference, Is.EqualTo(target.CurrentSelectedDate - target.currentDayOfWeek));
		}

	}
}
