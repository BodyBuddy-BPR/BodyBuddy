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
        private List<UserDto> _friends = new();

        public FriendsViewModel(IUserService userService)
        {
            _userService = userService;
        }

        public async Task Initialize()
        {
            //Does nothing for now
            await GetFriends();
        }

        public async Task GetFriends()
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;

                Friends = await _userService.GetFriends();
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

            await Shell.Current.DisplayAlert("Success", "Friend Added", "OK");

        }
    }
}
