﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.Dtos;

namespace BodyBuddy.Services
{
    public interface IUserService
    {
        Task<List<UserDto>> GetFriends();

        Task<List<UserDto>> GetFriendRquests();

        Task<bool> AddNewFriend(string email);

        Task<bool> AcceptFriendRequest(string friendId);
    }
}