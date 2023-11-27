using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.Dtos;
using BodyBuddy.Services;
using BodyBuddy.Views.Authentication;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BodyBuddy.ViewModels.User
{
    public partial class FriendsViewModel : BaseViewModel
    {
        private readonly IUserService _userService;

        [ObservableProperty]
        private string searchQuery; // Used to search for friend

        [ObservableProperty]
        private ObservableCollection<UserDto> _friends = new();
        [ObservableProperty]
        private ObservableCollection<UserDto> _pendingRequests = new();

        private const string UserUIDKey = "UserUID";

        [ObservableProperty]
        public bool isLoggedIn = false;

        public FriendsViewModel(IUserService userService)
        {
            _userService = userService;
        }

        public async Task Initialize()
        {
            IsLoggedIn = CheckIfUserLoggedIn();

            if (IsLoggedIn)
            {
                await GetFriends();
            }

        }

        private bool CheckIfUserLoggedIn()
        {
            // Retrieve the user ID from SecureStorage
            var userId = SecureStorage.GetAsync(UserUIDKey).Result;

            // If the user ID is not null, the user is logged in
            return !string.IsNullOrEmpty(userId);
        }

        public async Task GetFriends()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                Friends = new ObservableCollection<UserDto>(await _userService.GetFriends());
                PendingRequests = new ObservableCollection<UserDto>(await _userService.GetFriendRquests());

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error!", $"Unable to get friends {ex.Message}", "OK");

            }
            finally
            {
                IsBusy = false;
            }

        }

        [RelayCommand]
        public async Task AddNewFriend()
        {
            if (!await _userService.AddNewFriend(SearchQuery))
            {
                await Shell.Current.DisplayAlert("Error", "No User with that email found, try again", "OK");
                return;
            }

            await Shell.Current.DisplayAlert("Success", "Friend Request Sent", "OK");
            SearchQuery = "";
        }

        [RelayCommand]
        public async Task AcceptFriendRequest(UserDto friend)
        {
            if (!await _userService.AcceptFriendRequest(friend.Id)) return;

            PendingRequests.Remove(friend);
            Friends.Add(friend);
        }


        #region Navigation

        [RelayCommand]
        public async Task GoToLoginPage()
        {
            await Shell.Current.GoToAsync($"{nameof(LoginPage)}", true, new Dictionary<string, object>
            {
                { "SkipVisible", false }
            });
        }

        #endregion
    }
}
