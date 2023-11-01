using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.Dtos;
using BodyBuddy.Models;
using BodyBuddy.Repositories;
using Supabase;

namespace BodyBuddy.Authentication
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly Client _supabase;
        private IUserRepository _userRepository;

        // Used for Secure Storage
        private const string UserIdKey = "UserId";
        private const string UserUIDKey = "UserUID";
        private const string UserEmailKey = "UserEmail";
        private const string UserPasswordKey = "UserPassword";

        // Used for Preferences
        private readonly string _skipLoginKey = "SkipLogInKey";



        public UserAuthenticationService(Client client, IUserRepository userRepository)
        {
            _supabase = client;
            _userRepository = userRepository;
        }

        public UserDto GetCurrentUser()
        {
            var supabaseUser = _supabase.Auth.CurrentUser;

            var userId = SecureStorage.GetAsync(UserIdKey).Result;

            if (supabaseUser is null)
            {
                var guestUser = new UserDto
                {
                    UserUid = null,
                    Role = "guest",
                    Email = null,
                    Id = 0
                };
                return guestUser;
            }

            var user = new UserDto
            {
                UserUid = supabaseUser.Id,
                Role = supabaseUser.Role,
                Email = supabaseUser.Email,
                Id = int.Parse(userId)
            };

            return user;
        }

        public bool IsUserLoggedIn()
        {
            var user = GetCurrentUser();

            return user is not null && user.Role == "authenticated";
        }

        public async Task<bool> SignUserIn(string user, string password)
        {
            var loginInfo = await _supabase.Auth.SignIn(user, password);
            var success = loginInfo is not null && loginInfo.User.Role == "authenticated";

            var response = await _supabase.From<UserModel>().Select(x => new object[] { x.Id }).Where(x => x.Email == user)
                .Get();
            var userId = response.Model.Id;

            if (success)
            {
                await SecureStorage.SetAsync(UserIdKey, userId.ToString());
                await SecureStorage.SetAsync(UserUIDKey, loginInfo.User.Id);
                await SecureStorage.SetAsync(UserEmailKey, loginInfo.User.Email);
                await SecureStorage.SetAsync(UserPasswordKey, password);
            }

            return success;
        }

        public async Task<bool> TryAutoSignIn()
        {
            //var savedUserId = SecureStorage.GetAsync(UserUIDKey).Result;
            var savedUserEmail = SecureStorage.GetAsync(UserEmailKey).Result;
            var savedUserPassword = SecureStorage.GetAsync(UserPasswordKey).Result;

            if (string.IsNullOrEmpty(savedUserEmail) || string.IsNullOrEmpty(savedUserPassword)) return false;

            // Auto sign-in using the saved user info
            var loginInfo = await _supabase.Auth.SignIn(savedUserEmail, savedUserPassword);
            var success = loginInfo is not null && loginInfo.User.Role == "authenticated";

            return success;

        }

        public async Task<bool> SignUserOut()
        {
            if (!IsUserLoggedIn()) return false;

            try
            {
                await _supabase.Auth.SignOut();

                // Clear user info on sign-out
                SecureStorage.Remove(UserIdKey);
                SecureStorage.Remove(UserUIDKey);
                SecureStorage.Remove(UserEmailKey);
                SecureStorage.Remove(UserPasswordKey);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> SignUserUp(string user, string password)
        {
            var signUpInfo = await _supabase.Auth.SignUp(user, password);

            if (signUpInfo is null || signUpInfo.User.Role != "authenticated") return false;

            await _userRepository.AddNewUser(user);

            return await SignUserIn(user, password);
        }

    }
}
