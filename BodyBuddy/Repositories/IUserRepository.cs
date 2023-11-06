using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.Models;
using Java.Util;

namespace BodyBuddy.Repositories
{
    public interface IUserRepository
    {
        Task<List<UserModel>> GetFriends(string userId);

        //Task AddNewUser(string email);

        Task<UserModel> DoesUserExist(string email);

        Task AddNewFriend(string userId, string friendId);

    }
}
