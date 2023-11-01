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
        Task<List<UserModel>> GetFriends(int userId);

        Task AddNewUser(string email);

        Task<UserModel> DoesUserExist(string email);

        Task AddNewFriend(FriendListModel friends);

    }
}
