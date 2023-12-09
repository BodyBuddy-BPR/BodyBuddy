using BodyBuddy.Authentication;
using BodyBuddy.Dtos;
using BodyBuddy.Mappers;
using BodyBuddy.Repositories;

namespace BodyBuddy.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserAuthenticationService _userAuthenticationService;
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

        public async Task<List<UserDto>> GetFriendRequests()
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
            if (currentUserId == friend.Id) return false;

            await _userRepository.AddNewFriend(currentUserId, friend.Id);
            return true;
        }

        public async Task<bool> AcceptFriendRequest(string friendId)
        {
            var userId = SecureStorage.GetAsync(UserUIDKey).Result;

            return await _userRepository.AcceptFriendRequest(userId, friendId);
        }
    }
}
