using BodyBuddy.Dtos;
using BodyBuddy.Services;
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

        ///// <summary>
        ///// team management
        ///// </summary>
        //private List<string> subjects;

        /// <summary>
        /// color collection
        /// </summary>
        //private List<Brush> colors;

        /// <summary>
        /// The selected date meetings.
        /// </summary>
        private ObservableCollection<AppointmentDto>? selectedDateMeetings;

        /// <summary>
        /// The selected date
        /// </summary>
        private DateTime selectedDate = DateTime.Now.Date;

        /// <summary>
        /// The bool value.
        /// </summary>
        private bool isToday = true;

        /// <summary>
        /// The date text color.
        /// </summary>
        private Color dateTextColor = Colors.White;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulerDataBindingViewModel" /> class.
        /// </summary>
        public CalendarViewModel(ICalendarService calenderService, IWorkoutService workoutService)
        {
            this.selectedDateMeetings = new ObservableCollection<AppointmentDto>();
            this.selectedDateMeetings = this.GetSelectedDateAppointments(this.selectedDate);
            this.DisplayDate = DateTime.Now.Date.AddHours(8).AddMinutes(50);

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
                RaiseOnPropertyChanged("SelectedDateMeetings");
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

            Events.Clear();

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

        private async Task GetWorkouts()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                WorkoutList = await _workoutService.GetWorkoutPlans(false);

                var preMadeWorkouts = await _workoutService.GetWorkoutPlans(true);

                foreach (var item in preMadeWorkouts)
                {
                    WorkoutList.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error!", $"Unable to get workouts {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public ObservableCollection<AppointmentDto> GetSelectedDateAppointments(DateTime date)
        {
            ObservableCollection<AppointmentDto> selectedAppiointments = new ObservableCollection<AppointmentDto>();

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
        //[ObservableProperty] public ColorItem _selectedColor = new () { HexValue = Color.FromArgb("#0000ccff") };

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



