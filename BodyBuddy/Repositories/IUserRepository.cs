using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.Models;

namespace BodyBuddy.Repositories
{
    public interface IUserRepository
    {
        Task<List<UserRelationsModel>> GetFriends(string userId);
        Task<List<UserRelationsModel>> GetFriendRequests(string userId);

        Task<UserModel> DoesUserExist(string email);

        Task AddNewFriend(string userId, string friendId);

        Task<bool> AcceptFriendRequest(string userId, string friendId);
    }
}
