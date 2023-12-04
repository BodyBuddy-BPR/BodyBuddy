using BodyBuddy.Dtos;
using BodyBuddy.Services;
using BodyBuddy.Views.Calendar;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.ViewModels.Calendar
{
    public partial class CalendarViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private readonly ICalendarService _calenderService;
        private readonly IWorkoutService _workoutService;

        [ObservableProperty]
        private ObservableCollection<ColorItem> _colorList = new();
        [ObservableProperty] private List<WorkoutDto> _workoutList = new();

        [ObservableProperty]
        private WorkoutDto _selectedWorkout;

        #region Fields

        private bool isUpdatingData = false;

        private ObservableCollection<AppointmentDto>? selectedDateMeetings;

        private DateTime selectedDate = DateTime.Now.Date;

        private bool isToday = true;

        private Color dateTextColor = Colors.White;

        #endregion

        #region Constructor

        public CalendarViewModel(ICalendarService calenderService, IWorkoutService workoutService)
        {
            this.selectedDateMeetings = new ObservableCollection<AppointmentDto>();

            this.DisplayDate = DateTime.Now.Date;


            _calenderService = calenderService;
            _workoutService = workoutService;
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Gets or sets appointments.
        /// </summary>
        [ObservableProperty]
        public ObservableCollection<AppointmentDto>? _events = new();

        /// <summary>
        /// Gets or sets the selected date meetings.
        /// </summary>
        public ObservableCollection<AppointmentDto>? SelectedDateMeetings
        {
            get { return selectedDateMeetings; }
            set
            {
                selectedDateMeetings = value;
                RaiseOnPropertyChanged(nameof(SelectedDateMeetings));
            }
        }

        public bool IsUpdatingData
        {
            get { return isUpdatingData; }
            set
            {
                isUpdatingData = value;
                RaiseOnPropertyChanged("IsUpdatingData");
            }
        }

        /// <summary>
        /// Gets or sets the schedule selected date.
        /// </summary>
        public DateTime SelectedDate
        {
            get { return selectedDate; }
            set
            {
                selectedDate = value;
                RaiseOnPropertyChanged("SelectedDate");
            }
        }

        /// <summary>
        /// Gets or sets the date is today or not.
        /// </summary>
        public bool IsToday
        {
            get { return isToday; }
            set
            {
                isToday = value;
                RaiseOnPropertyChanged("IsToday");
            }
        }

        /// <summary>
        /// Gets or sets the date text color.
        /// </summary>
        public Color DateTextColor
        {
            get { return dateTextColor; }
            set
            {
                dateTextColor = value;
                RaiseOnPropertyChanged("DateTextColor");
            }
        }

        /// <summary>
        /// Gets or sets the schedule display date.
        /// </summary>
        public DateTime DisplayDate { get; set; }

        #endregion

        #region Methods

        public async Task Initialize()
        {
            var events = await _calenderService.GetAppointments();

            await GetWorkouts();

            if (Events != null)
            {
                Events.Clear();
                SelectedDateMeetings.Clear();

                foreach (var item in events)
                {
                    // Check if item.Workout is not null
                    if (item.Workout != null)
                    {
                        // Find the correct workout
                        WorkoutDto matchingWorkout = WorkoutList.FirstOrDefault(w => w.Id == item.Workout.Id);

                        item.Workout = matchingWorkout;
                    }

                    this.Events.Add(item);
                }
            }

            // Wait for GetWorkouts to complete
            await Task.Delay(400);

            SelectedDateMeetings = (this.GetSelectedDateAppointments(this.selectedDate));
        }

        private async Task GetWorkouts()
        {
            if (IsBusy || IsUpdatingData) return;

            try
            {
                IsBusy = true;
                IsUpdatingData = true;

                var fetchedWorkouts = await _workoutService.GetWorkoutPlans(false);

                var preMadeWorkouts = await _workoutService.GetWorkoutPlans(true);

                fetchedWorkouts.AddRange(preMadeWorkouts);

                WorkoutList = fetchedWorkouts;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error!", $"Unable to get workouts {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
                IsUpdatingData = false;
            }
        }

        public ObservableCollection<AppointmentDto> GetSelectedDateAppointments(DateTime date)
        {
            ObservableCollection<AppointmentDto> selectedAppiointments = new ObservableCollection<AppointmentDto>();

            SelectedDateMeetings.Clear();

            for (int i = 0; i < this.Events?.Count; i++)
            {
                DateTime startTime = this.Events[i].From;

                if (date.Day == startTime.Day && date.Month == startTime.Month && date.Year == startTime.Year)
                {
                    selectedAppiointments.Add(this.Events[i]);
                }
            }

            return selectedAppiointments;
        }


        #endregion

        #region Property Changed Event

        /// <summary>
        /// Property changed event handler
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Invoke method when property changed
        /// </summary>
        /// <param name="propertyName">property name</param>
        private void RaiseOnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion



        #region Popup

        [ObservableProperty]
        public string _eventName;

        [ObservableProperty] public TimeSpan _fromTime, _toTime;

        [ObservableProperty] public ColorItem _selectedColor;

        public void InitializePopup()
        {
            this.CreateColors();
        }

        public async Task CreateEvent()
        {
            var newEvent = new AppointmentDto
            {
                EventName = EventName,
                Date = SelectedDate.Date,
                From = SelectedDate.Add(FromTime),
                To = SelectedDate.Date.Add(ToTime),
                Background = SelectedColor.HexValue,
                Workout = SelectedWorkout
            };

            await _calenderService.CreateEvent(newEvent);
            ClearInputData();


            await Initialize();
        }


        [RelayCommand]
        public void DeclineAddEvent()
        {
            ClearInputData();
        }

        public void ClearInputData()
        {
            EventName = string.Empty;
            FromTime = TimeSpan.Zero;
            ToTime = TimeSpan.Zero;
            SelectedColor = null;
            SelectedWorkout = null;
        }

        private void CreateColors()
        {
            ColorList.Clear();

            ColorList.Add(new ColorItem { Name = "Light Blue", HexValue = Color.FromArgb("#FF00ccff") });
            ColorList.Add(new ColorItem { Name = "Blue", HexValue = Color.FromArgb("#FF3366ff") });
            ColorList.Add(new ColorItem { Name = "Green", HexValue = Color.FromArgb("#FF00cc66") });
            ColorList.Add(new ColorItem { Name = "Mint", HexValue = Color.FromArgb("#FF00cc99") });
            ColorList.Add(new ColorItem { Name = "Red", HexValue = Color.FromArgb("#FFe60000") });
            ColorList.Add(new ColorItem { Name = "Orange", HexValue = Color.FromArgb("#FFFF9505") });
            ColorList.Add(new ColorItem { Name = "Purple", HexValue = Color.FromArgb("#FFA800E0") });
            ColorList.Add(new ColorItem { Name = "Lavender", HexValue = Color.FromArgb("#FFB388EB") });
            ColorList.Add(new ColorItem { Name = "Pink", HexValue = Color.FromArgb("#FFff66cc") });
            ColorList.Add(new ColorItem { Name = "Black", HexValue = Color.FromArgb("#FF1F1F1F") });
        }
    }

    #endregion

    public partial class ColorItem : ObservableObject, INotifyPropertyChanged
    {
        private string name;
        private Color hexValue;

        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        public Color HexValue
        {
            get { return hexValue; }
            set
            {
                if (SetProperty(ref hexValue, value))
                {
                    // Convert Color to hex string
                    var hexString = $"#{(int)(value.Red * 255):X2}{(int)(value.Green * 255):X2}{(int)(value.Blue * 255):X2}{(int)(value.Alpha * 255):X2}";

                    // Raise PropertyChanged for HexValue when it changes
                    RaisePropertyChanged(nameof(HexValue));
                    Debug.WriteLine($"HexValue changed to: {hexString}");
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}



