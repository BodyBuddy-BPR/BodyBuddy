using BodyBuddy.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BodyBuddy.Dtos
{
    public partial class UserTotalSteps : ObservableObject
    {
        [ObservableProperty] private UserModel _user;
        [ObservableProperty] private int _totalSteps;
    }
}
