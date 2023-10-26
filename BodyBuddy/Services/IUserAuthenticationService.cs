using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.Services
{
    public interface IUserAuthenticationService
    {
        bool IsUserLoggedIn();

        Task<bool> SignUserIn(string user, string password);

        Task<bool> TryAutoSignIn();

        Task<bool> SignUserOut();

        Task<bool> SignUserUp(string user, string password);

        string GetSavedUserId();
    }
}
