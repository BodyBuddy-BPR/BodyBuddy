using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.Authentication;
using BodyBuddy.Dtos;
using BodyBuddy.Mappers;
using BodyBuddy.Models;
using BodyBuddy.Repositories;
using Java.Util;

namespace BodyBuddy.Services.Implementations
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private IUserAuthenticationService _userAuthenticationService;
        private readonly UserMapper _mapper = new();

        private const string UserUIDKey = "UserUID";

        public UserService(IUserRepository userRepository, IUserAuthenticationService userAuthenticationService)
        {
            _userRepository = userRepository;
            _userAuthenticationService = userAuthenticationService;
        }

        public async Task<List<UserDto>> GetFriends()
        {
            var userId = SecureStorage.GetAsync(UserUIDKey).Result;
            var relations = await _userRepository.GetFriends(userId);

            return relations.Select(model => _mapper.MapUserRelationsToDto(model)).ToList();
        }

        public async Task<List<UserDto>> GetFriendRquests()
        {
            var userId = SecureStorage.GetAsync(UserUIDKey).Result;
            var relations = await _userRepository.GetFriendRequests(userId);

            return relations.Select(model => _mapper.MapUserRelationsToDto(model)).ToList();
        }

        public async Task<bool> AddNewFriend(string email)
        {
            var friend = await _userRepository.DoesUserExist(email);

            if (friend == null) return false;

            var currentUserId = _userAuthenticationService.GetCurrentUser().Id;

            await _userRepository.AddNewFriend(currentUserId, friend.Id.ToString());
            return true;
        }

    }
}
