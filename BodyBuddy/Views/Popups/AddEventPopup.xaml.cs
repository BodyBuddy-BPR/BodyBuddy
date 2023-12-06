using BodyBuddy.ViewModels.Calendar;
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
        if (/*!EventNameValid.IsValid || */string.IsNullOrEmpty(NameEntry.Text))
        {
            //EventNameError.Text = EventNameValid.Errors.FirstOrDefault().ToString();
            EventNameError.IsVisible = true;
            EventNameError.Text = "Enter a name between 1 and 20 characters";
            // Start the timer to hide the error label after 3 seconds
            StartErrorLabelTimer(EventNameError);
        }
        else if (_viewModel.SelectedColor == null)
        {
            ComboBoxError.IsVisible = true;
            ComboBoxError.Text = "Please select an event color";
            StartErrorLabelTimer(ComboBoxError);
        }
        else if (EventNameValid.IsValid)
        {
            EventNameError.IsVisible = false;

            await _viewModel.CreateEvent();

            await MopupService.Instance.PopAsync();
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
            if (viewModel.SelectedColor != null)
            {
                ColorComboBoxBorder.BackgroundColor = viewModel.SelectedColor.HexValue;
            }
        }
    }
}