using CommunityToolkit.Mvvm.ComponentModel;

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
