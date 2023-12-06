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
