using BodyBuddy.ViewModels.Calendar;
using BodyBuddy.Views.Popups;
using Mopups.Interfaces;

namespace BodyBuddy.Views.Calendar;

public partial class CalenderPage : ContentPage
{
    private readonly CalendarViewModel _viewModel;
    private IPopupNavigation _popupNavigation;

    public CalenderPage(CalendarViewModel calendarViewModel, IPopupNavigation popupNavigation)
    {
        InitializeComponent();
        _viewModel = calendarViewModel;
        BindingContext = calendarViewModel;

        _popupNavigation = popupNavigation;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.Initialize();
    }

    private void AddEventClicked(object sender, EventArgs e)
    {
        _viewModel.InitializePopup();
        _popupNavigation.PushAsync(new AddEventPopup(_viewModel));
    }
}