﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.Dtos;
using BodyBuddy.Services;
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
        public bool isAcceptButtonVisible = false;

        public FriendsViewModel(IUserService userService)
        {
            _userService = userService;
        }

        public async Task Initialize()
        {
            // Hvis det her id matcher det i Pending Requests så skal accept button vises, ellers viser det hvem man har sendt den til
            var userId = SecureStorage.GetAsync(UserUIDKey).Result;

            await GetFriends();
        }

        public async Task GetFriends()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                var friends = await _userService.GetFriends();
                var friendRequests = await _userService.GetFriendRquests();

                // Clear existing lists
                Friends.Clear();
                PendingRequests.Clear();

                // Categorize users based on type
                foreach (var user in friends)
                {
                   Friends.Add(user);;
                }

                foreach (var request in friendRequests)
                {
                    PendingRequests.Add(request);
                }
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
        public async Task AcceptFriendRequest(UserDto user)
        {
            await _userService.AcceptFriendRequest(user.Id);
        }

    }
}
