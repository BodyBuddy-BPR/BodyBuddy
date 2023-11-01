﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.Dtos;
using Supabase;

namespace BodyBuddy.Authentication
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly Client _supabase;

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

        public UserDto GetCurrentUser()
        {
            var supabaseUser = _supabase.Auth.CurrentUser;

            if (supabaseUser is null)
            {
                var guestUser = new UserDto
                {
                    UserUid = null,
                    Role = "guest"
                };
                return guestUser;
            }

            var user = new UserDto
            {
                UserUid = supabaseUser.Id,
                Role = supabaseUser.Role
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




    }
}