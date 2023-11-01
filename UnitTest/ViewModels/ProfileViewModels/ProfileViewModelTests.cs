using BodyBuddy.ViewModels.Profile;
using BodyBuddy.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace UnitTest.ViewModels.ProfileViewModels
{
	public class ProfileViewModelTests
	{
		private Mock<IStartupTestService> _startupServiceMock;
		private Mock<IIntakeService> _intakeServiceMock;
		private Mock<IUserAuthenticationService> _userAuthenticationServiceMock;
		private ProfileViewModel target;

		[SetUp]
		public void Setup()
		{
			_startupServiceMock = new Mock<IStartupTestService>();
			_intakeServiceMock = new Mock<IIntakeService>();
			_userAuthenticationServiceMock = new Mock<IUserAuthenticationService>();
			target = new ProfileViewModel(_startupServiceMock.Object, _intakeServiceMock.Object, _userAuthenticationServiceMock.Object);
		}
	}
}
