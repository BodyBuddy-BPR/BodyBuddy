﻿using BodyBuddy.Dtos;
using BodyBuddy.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.ViewModels.Calendar
{
    public partial class CalendarViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private readonly ICalendarService _calenderService;

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
        public CalendarViewModel(ICalendarService calenderService)
        {
            //this.subjects = new List<string>();
            //this.colors = new List<Brush>();
            this.selectedDateMeetings = new ObservableCollection<AppointmentDto>();
            //this.CreateSubjects();
            //this.CreateColors();
            //this.IntializeAppoitments();
            this.selectedDateMeetings = this.GetSelectedDateAppointments(this.selectedDate);
            this.DisplayDate = DateTime.Now.Date.AddHours(8).AddMinutes(50);

            _calenderService = calenderService;
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

        /// <summary>
        /// Method to initialize the appointments.
        /// </summary>
        public async Task Initialize()
        {
            //this.Events = new ObservableCollection<AppointmentDto>();
            //Random randomTime = new();
            //List<Point> randomTimeCollection = this.GettingTimeRanges();

            //DateTime date;
            //DateTime dateFrom = DateTime.Now.AddDays(-25);
            //DateTime dateTo = DateTime.Now.AddDays(25);

            //for (date = dateFrom; date < dateTo; date = date.AddDays(1))
            //{
            //    for (int additionalAppointmentIndex = 0; additionalAppointmentIndex < randomTime.Next(4, 7); additionalAppointmentIndex++)
            //    {
            //        var meeting = new AppointmentDto();
            //        var randomTimeIndex = randomTime.Next(2);
            //        int hour = randomTime.Next((int)randomTimeCollection[randomTimeIndex].X, (int)randomTimeCollection[randomTimeIndex].Y);
            //        meeting.From = new DateTime(date.Year, date.Month, date.Day, hour, 0, 0);
            //        meeting.To = meeting.From.AddHours(1);
            //        meeting.EventName = this.subjects[randomTime.Next(9)];
            //        meeting.Background = this.colors[randomTime.Next(10)];

            //        this.Events.Add(meeting);
            //    }
            //}
            var events = await _calenderService.GetAppointments();
            //Get events from database
            foreach (var item in events)
            {
                //item.Background = new SolidColorBrush(Color.FromArgb(item.Background.ToString()));
                this.Events.Add(item);
            }
            //Events = new ObservableCollection<AppointmentDto>(await _calenderService.GetAppointments());
        }

        /// <summary>
        /// Metho to get selected date appointments.
        /// </summary>
        /// <param name="date">The selected date</param>
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

        /// <summary>
        /// Method to create the subject.
        /// </summary>
        //private void CreateSubjects()
        //{
        //    this.subjects.AddRange(new List<string>()
        //    {
        //        "General Meeting",
        //        "Plan Execution",
        //        "Project Plan",
        //        "Consulting",
        //        "Performance Check",
        //        "Support",
        //        "Development Meeting",
        //        "Scrum",
        //        "Project Completion",
        //        "Release updates",
        //        "Performance Check"
        //    });
        //}

        /// <summary>
        /// Method for get timing range.
        /// </summary>
        /// <returns>return time collection</returns>
        //private List<Point> GettingTimeRanges()
        //{
        //    List<Point> randomTimeCollection = new();
        //    randomTimeCollection.Add(new Point(9, 11));
        //    randomTimeCollection.Add(new Point(12, 14));
        //    randomTimeCollection.Add(new Point(15, 17));

        //    return randomTimeCollection;
        //}

        /// <summary>
        /// Method to create the colors.
        /// </summary>
        //private void CreateColors()
        //{
        //    this.colors.AddRange(new List<Brush>()
        //    {
        //        new SolidColorBrush(Color.FromArgb("#FF8B1FA9")),
        //        new SolidColorBrush(Color.FromArgb("#FFD20100")),
        //        new SolidColorBrush(Color.FromArgb("#FFFC571D")),
        //        new SolidColorBrush(Color.FromArgb("#FF36B37B")),
        //        new SolidColorBrush(Color.FromArgb("#FF3D4FB5")),
        //        new SolidColorBrush(Color.FromArgb("#FFE47C73")),
        //        new SolidColorBrush(Color.FromArgb("#FF636363")),
        //        new SolidColorBrush(Color.FromArgb("#FF85461E")),
        //        new SolidColorBrush(Color.FromArgb("#FF0F8644")),
        //        new SolidColorBrush(Color.FromArgb("#FF01A1EF"))
        //    });
        //}

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

        public async Task<bool> CreateEvent()
        {
            return false;
        }


        [RelayCommand]
        public void DeclineAddEvent()
        {
            EventName = string.Empty;
            //WorkoutName = string.Empty;
            //WorkoutDescription = string.Empty;
            //ErrorMessage = string.Empty;
        }

        #endregion

    }
}
