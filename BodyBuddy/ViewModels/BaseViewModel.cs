using BodyBuddy.Views.Profile;
using BodyBuddy.Views.User;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BodyBuddy.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        bool isBusy;

        [ObservableProperty]
        string title;

        public bool IsNotBusy => !IsBusy;

        public BaseViewModel()
        {
            
        }

        public static async Task GoBackAsync()
        {
            await Shell.Current.Navigation.PopAsync();
        }

    }
}
