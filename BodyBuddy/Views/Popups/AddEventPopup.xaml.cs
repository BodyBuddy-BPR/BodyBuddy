using BodyBuddy.ViewModels.Calendar;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Mopups.Services;

namespace BodyBuddy.Views.Popups;

public partial class AddEventPopup
{
    private readonly CalendarViewModel _viewModel;
    public AddEventPopup(CalendarViewModel calendarViewModel)
    {
        InitializeComponent();
        _viewModel = calendarViewModel;
        BindingContext = calendarViewModel;
    }

    private async void CreateBtn_Clicked(object sender, EventArgs e)
    {
        if (!EventNameValid.IsValid)
        {
            EventNameError.Text = EventNameValid.Errors.FirstOrDefault().ToString();
            EventNameError.IsVisible = true;

            // Start the timer to hide the error label after 3 seconds
            StartErrorLabelTimer(EventNameError);
        }
        else if (EventNameValid.IsValid)
        {
            EventNameError.IsVisible = false;

            await _viewModel.CreateEvent();

            await MopupService.Instance.PopAsync();

            _viewModel.EventName = string.Empty;
        }
    }

    private async void StartErrorLabelTimer(Label errorLabel)
    {
        // Wait for 3 seconds
        await Task.Delay(3000);

        // Hide the error label if it is still visible
        if (errorLabel.IsVisible)
        {
            errorLabel.IsVisible = false;
        }
    }

    private void SfComboBox_SelectionChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
    {
        if (BindingContext is CalendarViewModel viewModel)
        {
            // Assuming you have a UI element named "popupBorder" that needs its background color changed
            if (viewModel.SelectedColor != null)
            {
                ColorComboBox.BackgroundColor = viewModel.SelectedColor.HexValue;
            }
        }
    }
}