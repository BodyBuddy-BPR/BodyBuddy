using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.Models;
using Postgrest;
using Client = Supabase.Client;

namespace BodyBuddy.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly Client _supabaseClient;
        private const string UserIdKey = "UserId";


        public UserRepository(Client client)
        {
            _supabaseClient = client;
        }

        // Gets a list of a users friends for all accepted requests
        public async Task<List<UserRelationsModel>> GetFriends(string userId)
        {
            var friendListModels = await _supabaseClient.From<UserRelationsModel>()
                .Where(x => x.UserId == userId && x.Type == "friend").Get();
            var friends = friendListModels.Models;

            //return friends.Select(user => user.User).ToList();

            return friends;
        }

        // Gets a list of pending friend requests a user has
        public async Task<List<UserRelationsModel>> GetFriendRequests(string userId)
        {
            var friendListModels = await _supabaseClient.From<UserRelationsModel>()
                .Where(x => x.FriendId == userId && x.Type == "pending").Get();
            var friendRequests = friendListModels.Models;

            return friendRequests;
        }

        public async Task<UserModel> DoesUserExist(string email)
        {
            var response = await _supabaseClient.From<UserModel>().Select(x => new object[] { x.Id }).Where(x => x.Email == email).Get();
            var user = response.Model;

            return user;
        }

        public async Task AddNewFriend(string userId, string friendId)
        {
            var relations = new List<UserRelationsModel>
            {
                new() { UserId = userId, FriendId = friendId, Type = "pending" },
                new() { UserId = friendId, FriendId = userId, Type = "pending" },
            };

            await _supabaseClient.From<UserRelationsModel>().Insert(relations);
        }

        public async Task AcceptFriendRequest(string userId)
        {
            await _supabaseClient.From<UserRelationsModel>()
                .Where(x => x.FriendId == userId)
                .Set(x => x.Type, "friend")
                .Update();
        }
    }
}
