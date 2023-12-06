using BodyBuddy.Views.Profile;
using BodyBuddy.Views.User;

namespace BodyBuddy.Controls.Common
{
    public class CommonToolBarItems : ContentPage
    {
        public static void AddCommonToolbarItems(ContentPage page)
        {
            page.ToolbarItems.Add(new ToolbarItem
            {
                Command = new Command(() => GoToProfile(page)),
                IconImageSource = "profile.png",
                Order = ToolbarItemOrder.Secondary,
                Text = "Profile"
            });

            page.ToolbarItems.Add(new ToolbarItem
            {
                Command = new Command(() => GoToFriends(page)),
                Order = ToolbarItemOrder.Secondary,
                Text = "Friends"
            });
        }

        private static async void GoToProfile(ContentPage page)
        {
            await Shell.Current.GoToAsync(nameof(ProfilePage), true);
        }

        private static async void GoToFriends(ContentPage page)
        {
            await Shell.Current.GoToAsync(nameof(FriendsPage), true);
        }
    }
}
