using BodyBuddy.Dtos;

namespace BodyBuddy.Authentication
{
    public interface IUserAuthenticationService
    {
        // Returns the user data, if there is a logged in user.
        UserDto GetCurrentUser();

        // Checks if the current user is logged in 
        bool IsUserLoggedIn();

        // Signs a user in if login information is correct
        // Saves this info in SecureStorage for future AutoSignIn()
        Task<bool> SignUserIn(string user, string password);

        // Tries to sign the user in automatically bases on saved info in SecureStorage
        Task<bool> TryAutoSignIn();

        // Signs a user out
        // Removes login info from SecureStorage
        Task<bool> SignUserOut();

        // Registers a new user
        // Automatically signs that user in afterwards
        Task<bool> SignUserUp(string user, string password);

    }
}
