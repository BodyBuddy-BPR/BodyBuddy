using BodyBuddy.Dtos;
using BodyBuddy.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.ViewModels.Profile
{
    public partial class ProfileViewModel : BaseViewModel
    {
        private IStartupTestService _startupTestService;
        [ObservableProperty]
        private StartupTestDto _startupTestDto;
        public ProfileViewModel(IStartupTestService startupTestService)
        {
            _startupTestService = startupTestService;
        }

        public async Task Initialize()
        {
            StartupTestDto = await _startupTestService.GetStartupTestData();
        }
    }
}
