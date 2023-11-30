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

            return friends;
        }

        // Gets a list of pending friend requests a user has
        public async Task<List<UserRelationsModel>> GetFriendRequests(string userId)
        {
            var friendListModels = await _supabaseClient.From<UserRelationsModel>()
                .Where(x => x.UserId == userId && x.Type == "pending").Get();
            var friendRequests = friendListModels.Models;

            return friendRequests;
        }

        public async Task AddNewFriend(string userId, string friendId)
        {
            var relation = new UserRelationsModel()
            {
                UserId = friendId,
                FriendId = userId,
                Type = "pending",
            };

            await _supabaseClient.From<UserRelationsModel>().Insert(relation);
        }


        public async Task<bool> AcceptFriendRequest(string userId, string friendId)
        {
            try
            {
                await _supabaseClient.From<UserRelationsModel>()
                    .Where(x => x.UserId == userId && x.FriendId == friendId)
                    .Set(x => x.Type, "friend")
                    .Update();

                var relation = new UserRelationsModel()
                {
                    UserId = friendId,
                    FriendId = userId,
                    Type = "friend",
                };

                await _supabaseClient.From<UserRelationsModel>().Insert(relation);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

        }

        public async Task<UserModel> DoesUserExist(string email)
        {
            var response = await _supabaseClient.From<UserModel>().Select(x => new object[] { x.Id }).Where(x => x.Email == email).Get();
            var user = response.Model;

            return user;
        }
    }
}
