using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supabase;

namespace BodyBuddy.Services.Implementations
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private Client _supabase;

        // Used for Secure Storage
        private const string UserIdKey = "UserId";
        private const string UserEmailKey = "UserEmail";
        private const string UserPasswordKey = "UserPassword";

        // Used for Preferences
        private readonly string _skipLoginKey = "SkipLogInKey"; 

        public UserAuthenticationService(Client client)
        {
            _supabase = client;
        }

        public bool IsUserLoggedIn()
        {
            var user = _supabase.Auth.CurrentUser;
            if (user is not null && user.Role == "authenticated")
            {
                return true;
            }
            return false;
        }

        public async Task<bool> SignUserIn(string user, string password)
        {
            var loginInfo = await _supabase.Auth.SignIn(user, password);
            var success = loginInfo is not null && loginInfo.User.Role == "authenticated";

            if (success)
            {
                // Save user ID securely for future use
                await SecureStorage.SetAsync(UserIdKey, loginInfo.User.Id);
                await SecureStorage.SetAsync(UserEmailKey, loginInfo.User.Email);
                await SecureStorage.SetAsync(UserPasswordKey, password);
            }

            return success;
        }

        public async Task<bool> TryAutoSignIn()
        {
            // Check if there is a saved user ID
            //var savedUserId = SecureStorage.GetAsync(UserIdKey).Result;
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
                SecureStorage.Remove(UserEmailKey);
                SecureStorage.Remove(UserPasswordKey);

                //Preferences.Set(_skipLoginKey, false);

                var user = _supabase.Auth.CurrentUser;

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

            if (signUpInfo is not null && signUpInfo.User.Role == "authenticated")
            {
                await SignUserIn(user, password);
                return true;
            }

            return false;
        }

        public string GetSavedUserId()
        {
            return SecureStorage.GetAsync(UserIdKey).Result;
        }

        //public Task<bool> UserSkippedLogin()
        //{

        //}
    }
}
