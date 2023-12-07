using BodyBuddy.Dtos;

namespace BodyBuddy.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Supabase: Used to get a list of all your friends
        /// </summary>
        /// <returns>List of current friends</returns>
        Task<List<UserDto>> GetFriends();

        /// <summary>
        /// Supabase: Getting friend requests
        /// </summary>
        /// <returns>List of people who added the user</returns>
        Task<List<UserDto>> GetFriendRequests();

        /// <summary>
        /// Supabase: Used to add a friend via email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>If succeeded</returns>
        Task<bool> AddNewFriend(string email);

        /// <summary>
        /// Supabase: Accepting incoming friend request via their friendId
        /// </summary>
        /// <param name="friendId"></param>
        /// <returns>If succeeded</returns>
        Task<bool> AcceptFriendRequest(string friendId);
    }
}
